using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : MonoBehaviour
{
    static public QuickSlotController instance;
    public QuickSlot[] quickSlots;
    public GridLayoutGroup grid;

    void Awake()
    {
        instance = this;
    }

    public void Execute(int index)
    {
        quickSlots[index].Use();
    }
}