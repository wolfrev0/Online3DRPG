using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    public Image cover;
    public Sprite swordIcon;
    public Sprite bowIcon;
    Image icon;

    void Awake()
    {
        icon = GetComponent<Image>();
    }

    void Update()
    {
        if(Player.mine)
        {
            switch(Player.mine.weaponType)
            {
                case TeraTaleNet.Weapon.Type.knife:
                    icon.sprite = swordIcon;
                    icon.enabled = true;
                    break;
                case TeraTaleNet.Weapon.Type.bow:
                    icon.sprite = bowIcon;
                    icon.enabled = true;
                    break;
                default:
                    icon.enabled = false;
                    break;
            }
            cover.fillAmount = Player.mine.skillCoolTimeLeft / Player.mine.skillCoolTime;
        }
    }
}