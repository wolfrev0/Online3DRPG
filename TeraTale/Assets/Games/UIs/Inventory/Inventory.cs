using UnityEngine;

public class Inventory : MonoBehaviour
{
    static public Inventory instance;
    public InventorySlot[] itemSlots;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].itemStackIndex = i;
    }

    public void ToggleShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}