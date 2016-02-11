using UnityEngine;
using System;
using UnityEngine.UI;
using TeraTaleNet;

//추후에 Attackable과 Damagable로 인터페이스 분리하려면 해라. 근데 필요할지는 의문.
public abstract class AliveEntity : Entity, Attackable, Damagable, Movable, IAutoSerializable
{
    static int[] _expMaxByLevel = new int[] { 1, 100, 160, 250, 400, 999 };
    
    [SerializeField]
    Image _hpBar = null;
    [SerializeField]
    Image _staminaBar = null;
    [SerializeField]
    Text _levelText = null;
    public float _hp;
    public float hp
    {
        get { return _hp; }
        protected set
        {
            if (value > hpMax)
                value = hpMax;
            if (value < 0)
                value = 0;
            _hp = value;
            _hpBar.fillAmount = hp / hpMax;
            if (hp == 0)
                Die();
        }
    }
    public abstract float hpMax { get; }
    public float _stamina;
    public float stamina
    {
        get { return _stamina; }
        private set
        {
            _stamina = value;
            _staminaBar.fillAmount = stamina / staminaMax;
        }
    }
    public abstract float staminaMax { get; }
    public float attackDamage { get { return baseAttackDamage + bonusAttackDamage; } }
    public abstract float baseAttackDamage { get; }
    public abstract float bonusAttackDamage { get; }
    public float attackSpeed { get { return baseAttackSpeed + bonusAttackSpeed; } }
    public abstract float baseAttackSpeed { get; }
    public abstract float bonusAttackSpeed { get; }
    public float abilityPower { get; set; }
    public float healthRegen { get; set; }
    public float defence { get; set; }
    public float magicRegistance { get; set; }
    public float moveSpeed { get; set; }
    public float castingTimeDecrease { get; set; }
    public float coolTimeDecrease { get; set; }
    public int _level = 1;
    public int level
    {
        get { return _level; }
        private set
        {
            if (expMax < 1)
                return;
            if (value < _level)
                throw new ArgumentException("level can not decreased.");
            if (level >= levelMax)
                return;
            _level = value;
            _levelText.text = "LV." + _level;
        }
    }
    int levelMax { get { return _expMaxByLevel.Length - 1; } }
    public float _exp;
    public float exp
    {
        get { return _exp; }
        private set
        {
            _exp = value;
            while (_exp >= expMax)
            {
                if (level == levelMax)
                {
                    _exp = expMax - 1;
                }
                else
                {
                    _exp -= expMax;
                    level = level + 1;
                }
            }
        }
    }
    public float expMax { get { return _expMaxByLevel[level]; } }

    public Vector3 _syncedPos;
    public Vector3 _syncedRot;
    Vector3 _posError;
    Vector3 _rotError;
    [SerializeField]
    Transform _uiRoot;

    static ParticleSystem _pfHealFX;
    static DamageView _pfDamageView;

    public Weapon.Type weaponType
    {
        get
        {
            Weapon.Type weaponType = Weapon.Type.none;

            Player player = this as Player;
            if (player)
                weaponType = player.weapon.weaponType;

            return weaponType;
        }
    }

    protected virtual float CalculateHeal(Heal heal) { return heal.amount; }
    protected virtual float CalculateDamage(Damage damage) { return damage.amount; }

    protected void Start()
    {
        if (_pfHealFX == null)
            _pfHealFX = Resources.Load<ParticleSystem>("Prefabs/Heal");
        if (_pfDamageView == null)
            _pfDamageView = Resources.Load<DamageView>("Prefabs/DamageView");
        if (isServer)
            InvokeRepeating("PeriodicSync", UnityEngine.Random.Range(0f, 5f), 5f);
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        if (isServer)
        {
            hp = hpMax;//Initialize property call
            stamina = staminaMax;
            level = level;
        }
        else
        {
            Sync("level");
            Sync("hp");
            Sync("stamina");
            Sync("exp");
        }
    }

    protected virtual void PeriodicSync()
    {
        if (gameObject.activeSelf == false)
            return;
        _syncedPos = transform.position;
        _syncedRot = transform.eulerAngles;

        Sync s = new Sync(RPCType.Others, "", "_syncedPos");
        s.signallerID = networkID;
        s.sender = userName;
        Sync(s);

        s = new Sync(RPCType.Others, "", "_syncedRot");
        s.signallerID = networkID;
        s.sender = userName;
        Sync(s);
    }

    protected override void OnSynced(Sync sync)
    {
        switch(sync.member)
        {
            case "_syncedPos":
                _posError = _syncedPos - transform.position;
                break;
            case "_syncedRot":
                _rotError = _syncedRot - transform.eulerAngles;
                break;
        }
    }

    protected void Update()
    {
        if (isLocal)
        {
            transform.position += _posError / 6;
            _posError = _posError * 5 / 6;
            transform.eulerAngles += _rotError / 6;
            _rotError = _rotError * 5 / 6;
        }
    }

    public byte[] Serialize()
    {
        return Serializer.Serialize(this as IAutoSerializable);
    }

    public void Deserialize(byte[] buffer)
    {
        Serializer.Deserialize(this as IAutoSerializable, buffer);
    }

    public int SerializedSize()
    {
        return Serializer.SerializedSize(this as IAutoSerializable);
    }

    public Header CreateHeader()
    {
        return Serializer.CreateHeader(this as IAutoSerializable);
    }

    public void Heal(Heal heal)
    {
        if (isServer)
            Send(heal);
        if (heal.amount < 0)
            throw new ArgumentException("Healing amount should be bigger than 0.");
        hp += CalculateHeal(heal);
        OnHealed(heal);

        ParticleSystem _particle = Instantiate(_pfHealFX);
        _particle.transform.SetParent(transform);
        _particle.transform.localPosition = Vector3.zero;
        Destroy(_particle.gameObject, _particle.duration);
    }
    protected virtual void OnHealed(Heal heal) { }

    public void Damage(Damage damage)
    {
        if (isServer)
            Send(damage);
        if (damage.amount < 0)
            throw new ArgumentException("Damage amount should be bigger than 0.");
        var calculatedDamage = CalculateDamage(damage);
        hp -= calculatedDamage;
        OnDamaged(damage);

        if (damage.knockdown)
            Knockdown();

        var damageView = Instantiate(_pfDamageView);
        damageView.SetDamage(calculatedDamage);
        damageView.transform.position = transform.position + Vector3.up;
        damageView.transform.SetParent(_uiRoot);

        var ro = UnityEngine.Random.Range(0f, 0.5f);
        var phi = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
        var theta = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
        damageView.transform.Translate(ro * Mathf.Sin(phi) * Mathf.Cos(theta), ro * Mathf.Sin(phi) * Mathf.Sin(theta), ro * Mathf.Cos(phi));
    }
    protected virtual void OnDamaged(Damage damage) { }

    public void ExpUp(ExpUp expUp)
    {
        if (isServer)
            Send(expUp);
        exp += expUp.amount;
    }

    protected abstract void Die();
    protected abstract void Knockdown();
}