using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Cell[] cells;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        var player = Player.mine;
        for (int i = 0; i < cells.Length; i++)
            cells[i].SetItemStack(i);
    }

    public void ToggleShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}