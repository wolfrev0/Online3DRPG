using UnityEngine;
using System;
using UnityEngine.UI;
using TeraTaleNet;

//추후에 Attackable과 Damagable로 인터페이스 분리하려면 해라. 근데 필요할지는 의문.
public abstract class AliveEntity : Entity, Attackable, Damagable, Movable
{
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
        private set
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
    public float _hpMax;
    public float hpMax
    {
        get { return _hpMax; }
        private set
        {
            _hpMax = value;
            _hpBar.fillAmount = hp / hpMax;
        }
    }
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
    public float _staminaMax;
    public float staminaMax
    {
        get { return _staminaMax; }
        private set
        {
            _staminaMax = value;
            _staminaBar.fillAmount = stamina / staminaMax;
        }
    }
    public float _attackDamage = 0;
    public float attackDamage
    {
        get { return _attackDamage; }
        private set { _attackDamage = value; }
    }
    public float abilityPower { get; set; }
    public float healthRegen { get; set; }
    public float defence { get; set; }
    public float magicRegistance { get; set; }
    public float moveSpeed { get; set; }
    public float attackSpeed { get; set; }
    public float castingTimeDecrease { get; set; }
    public float coolTimeDecrease { get; set; }
    public int _level;
    public int level
    {
        get { return _level; }
        private set
        {
            if (value < _level)
                throw new ArgumentException("level can not decreased.");
            _level = value;
            _levelText.text = "LV." + _level;
        }
    }
    public float _exp;
    public float exp
    {
        get { return _exp; }
        private set
        {
            _exp = value;
            if (_exp >= expMax)
            {
                _exp -= expMax;
                level = level + 1;
            }
        }
    }
    public float _expMax;
    public float expMax { get { return _expMax; } private set { _expMax = value; } }

    public Vector3 _syncedPos;
    public Vector3 _syncedRot;
    Vector3 _posError;
    Vector3 _rotError;

    static ParticleSystem _pfHealFX;

    static AliveEntity()
    {
        _pfHealFX = Resources.Load<ParticleSystem>("Prefabs/Heal");
    }

    protected new void Start()
    {
        base.Start();
        if (isServer)
            InvokeRepeating("PeriodicSync", UnityEngine.Random.Range(0f, 3f), 3f);
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
            Sync("hp");
            Sync("hpMax");
            Sync("stamina");
            Sync("staminaMax");
            Sync("attackDamage");
            Sync("abilityPower");
            Sync("healthRegen");
            Sync("defence");
            Sync("magicRegistance");
            Sync("moveSpeed");
            Sync("attackSpeed");
            Sync("castingTimeDecrease");
            Sync("coolTimeDecrease");
            Sync("level");
            Sync("exp");
            Sync("expMax");
        }
    }

    void PeriodicSync()
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

    protected sealed override void OnSynced(Sync sync)
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

    public virtual void Heal(Heal heal)
    {
        if (isServer)
            Send(heal);
        if (heal.amount < 0)
            throw new ArgumentException("Healing amount should be bigger than 0.");
        hp += heal.amount;

        ParticleSystem _particle = Instantiate(_pfHealFX);
        _particle.transform.SetParent(transform);
        _particle.transform.localPosition = Vector3.zero;
        Destroy(_particle.gameObject, _particle.duration);
    }

    public virtual void Damage(Damage dmg)
    {
        if (isServer)
            Send(dmg);
        if (dmg.amount < 0)
            throw new ArgumentException("Damage amount should be bigger than 0.");
        hp -= dmg.amount;

        if (dmg.knockdown)
            Knockdown();
    }

    public virtual void ExpUp(ExpUp expUp)
    {
        if (isServer)
            Send(expUp);
        exp += expUp.amount;
    }

    protected abstract void Die();
    protected abstract void Knockdown();
}