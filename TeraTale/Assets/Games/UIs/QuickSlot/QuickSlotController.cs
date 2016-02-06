using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class QuickSlotController : MonoBehaviour
{
    public QuickSlot[] quickSlots;

    public void Execute(int index)
    {
        quickSlots[index].Use();
    }
}