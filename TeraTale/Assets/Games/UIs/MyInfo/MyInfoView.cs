using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyInfoView : Modal, IDragHandler
{
    public Camera playerBodyCamera;
    public Image accessoryView;
    public Image weaponView;
    public Text ADView;
    public Text ASView;
    public Text MSView;
    float theta = 0;

    protected new void OnEnable()
    {
        playerBodyCamera.enabled = true;
        playerBodyCamera.transform.SetParent(Player.mine.transform);
        accessoryView.sprite = Player.mine.accessory.sprite;
        weaponView.sprite = Player.mine.weapon.sprite;
        ADView.text = Player.mine.attackDamage + " (" + Player.mine.baseAttackDamage + "+" + Player.mine.bonusAttackDamage + ")";
        ASView.text = Player.mine.attackSpeed + " (" + Player.mine.baseAttackSpeed + "+" + Player.mine.bonusAttackSpeed + ")";
        MSView.text = Player.mine.moveSpeed + " (" + Player.mine.baseMoveSpeed + "+" + Player.mine.bonusMoveSpeed + ")";
    }

    protected new void OnDisable()
    {
        playerBodyCamera.enabled = false;
    }

    void Update()
    {
        playerBodyCamera.transform.localPosition = Quaternion.Euler(0, theta, 0) * new Vector3(0, 1f, -0.9f);
        playerBodyCamera.transform.localEulerAngles = new Vector3(0, theta, 0);
    }

    public void ToggleShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            theta += eventData.delta.x;
    }
}
