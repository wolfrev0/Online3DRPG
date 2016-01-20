using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Cell[] cells;

    void OnEnable()
    {
        var player = Player.mine;
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].SetItemStack(player.itemStacks[i]);
        }
    }

    public void ToggleShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}