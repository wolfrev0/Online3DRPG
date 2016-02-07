using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class QuickSlotController : MonoBehaviour
{
    static public QuickSlotController instance;
    public QuickSlot[] quickSlots;

    void Awake()
    {
        instance = this;
    }

    public void Execute(int index)
    {
        quickSlots[index].Use();
    }
}