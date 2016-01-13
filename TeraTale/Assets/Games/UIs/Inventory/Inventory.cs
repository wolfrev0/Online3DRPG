using UnityEngine;

public class Inventory : NetworkScript
{
    public Cell[] cells;

    void OnEnable()
    {
        var player = Player.FindPlayerByName(userName);
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