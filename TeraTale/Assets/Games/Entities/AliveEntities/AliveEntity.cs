using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public struct HealInfo
{
    public string healer;
    public float amount;
}

public struct DamageInfo
{
    public enum Type
    {
        Physical,
        Magic,
    }
    public Type type;
    public float amount;
    public float knockback;
    public bool fallDown;
}

//추후에 Attackable과 Damagable로 인터페이스 분리하려면 해라. 근데 필요할지는 의문.
public abstract class AliveEntity : Entity, Attackable, Damagable, Movable
{
    [SerializeField]
    Image _hpBar = null;
    [SerializeField]
    Image _staminaBar = null;
    [SerializeField]
    Text _levelText = null;
    [SerializeField]
    float _hp;
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
        }
    }
    [SerializeField]
    float _hpMax;
    public float hpMax
    {
        get { return _hpMax; }
        private set
        {
            _hpMax = value;
            _hpBar.fillAmount = hp / hpMax;
        }
    }
    [SerializeField]
    float _stamina;
    public float stamina
    {
        get { return _stamina; }
        private set
        {
            _stamina = value;
            _staminaBar.fillAmount = stamina / staminaMax;
        }
    }
    [SerializeField]
    float _staminaMax;
    public float staminaMax
    {
        get { return _staminaMax; }
        private set
        {
            _staminaMax = value;
            _staminaBar.fillAmount = stamina / staminaMax;
        }
    }
    [SerializeField]
    float _attackDamage = 0;
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
    [SerializeField]
    int _level;
    public int level
    {
        get { return _level; }
        private set { _level = value; _levelText.text = "LV." + _level; }
    }
    public float exp { get; set; }
    public float expMax { get; set; }

    protected new void Start()
    {
        base.Start();
        hp = hp;//Initialize property call
        stamina = stamina;
        level = level;
    }

    public virtual void Heal(HealInfo healInfo)
    {
        if (healInfo.amount < 0)
            throw new ArgumentException("Healing amount should be bigger than 0.");
        hp += healInfo.amount;
    }

    public virtual void Damage(DamageInfo dmgInfo)
    {
        if (dmgInfo.amount < 0)
            throw new ArgumentException("Damage amount should be bigger than 0.");
        hp -= dmgInfo.amount;
    }
}