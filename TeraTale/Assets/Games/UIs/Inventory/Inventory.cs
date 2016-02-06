using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] itemSlots;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        var player = Player.mine;
        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].itemStackIndex = i;
    }

    public void ToggleShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}