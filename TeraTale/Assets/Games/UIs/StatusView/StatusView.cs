using UnityEngine;
using UnityEngine.UI;

public class StatusView : MonoBehaviour
{
    public AliveEntity target { private get; set; }
    [SerializeField]
    Image _hpBar = null;
    [SerializeField]
    Image _staminaBar = null;
    [SerializeField]
    Text _levelText = null;
    [SerializeField]
    Text _nameText = null;
    [SerializeField]
    Text _moneyText = null;
    [SerializeField]
    Transform _buffsGroup = null;

    void Update()
    {
        if (target == null)
            return;
        //if (target.GetType().IsSubclassOf(typeof(Player)));//따로처리
        _hpBar.fillAmount = target.hp / target.hpMax;
        _staminaBar.fillAmount = target.stamina / target.staminaMax;
        //_levelText.text = _player.level;
        _nameText.text = target.name;
        //_moneyText.text = _player.money;
        //_buffsGroup.AddChildren(_player.GetAllBuffs());
    }
}