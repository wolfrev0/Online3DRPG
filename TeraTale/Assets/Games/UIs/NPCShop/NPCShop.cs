using UnityEngine;

public class NPCShop : Modal
{
    static public NPCShop instance;
    public ShopSlot[] itemSlots;
    NPC _owner;

    public NPC currentOwner { get { return _owner; } }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
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

    protected new void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < itemSlots.Length; i++)
            itemSlots[i].itemStackIndex = i;
    }
}