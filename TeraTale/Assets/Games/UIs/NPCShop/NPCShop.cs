using UnityEngine;

public class NPCShop : MonoBehaviour
{
    static public NPCShop instance;
    public ShopSlot[] itemSlots;
    NPC _owner;

    public NPC currentOwner { get { return _owner; } }

    void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Open(NPC caller)
    {
        _owner = caller;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        _owner = null;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].itemStackIndex = i;
    }
}