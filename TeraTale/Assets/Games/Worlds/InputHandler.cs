using System;
using TeraTaleNet;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    static public InputHandler instance;

    Action onLeftClick = () => {  };
    Action onRightClick = () =>
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Terrain")))
            Player.mine.Send(new Navigate(hit.point));
    };
    Action on1 = () => { };
    Action on2 = () => { };
    Action on3 = () => { };
    Action on4 = () => { };
    Action on5 = () => { };
    Action on6 = () => { };
    Action on7 = () => { };
    Action on8 = () => { };
    Action on9 = () => { };
    Action on0 = () => { };
    Action onCtrl = () => { Player.mine.Send(new Attack()); };
    Action onZ = () => { };
    Action onX = () => { };
    Action onC = () => { Player.mine.Send(new BackTumbling()); };
    Action onV = () => { };

    void Awake()
    {
        instance = this;
        on1 = () => { QuickSlotController.instance.Execute(0); };
        on2 = () => { QuickSlotController.instance.Execute(1); };
        on3 = () => { QuickSlotController.instance.Execute(2); };
        on4 = () => { QuickSlotController.instance.Execute(3); };
        on5 = () => { QuickSlotController.instance.Execute(4); };
        on6 = () => { QuickSlotController.instance.Execute(5); };
        on7 = () => { QuickSlotController.instance.Execute(6); };
        on8 = () => { QuickSlotController.instance.Execute(7); };
        on9 = () => { QuickSlotController.instance.Execute(8); };
        on0 = () => { QuickSlotController.instance.Execute(9); };
    }

    void OnDestroy()
    {
        instance = null;
    }

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            onLeftClick();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            onRightClick();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            on1();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            on2();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            on3();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            on4();
        if (Input.GetKeyDown(KeyCode.Alpha5))
            on5();
        if (Input.GetKeyDown(KeyCode.Alpha6))
            on6();
        if (Input.GetKeyDown(KeyCode.Alpha7))
            on7();
        if (Input.GetKeyDown(KeyCode.Alpha8))
            on8();
        if (Input.GetKeyDown(KeyCode.Alpha9))
            on9();
        if (Input.GetKeyDown(KeyCode.Alpha0))
            on0();
        if (Input.GetKeyDown(KeyCode.LeftControl))
            onCtrl();
        if (Input.GetKeyDown(KeyCode.Z))
            onZ();
        if (Input.GetKeyDown(KeyCode.X))
            onX();
        if (Input.GetKeyDown(KeyCode.C))
            onC();
        if (Input.GetKeyDown(KeyCode.V))
            onV();
    }
}
