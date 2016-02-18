using UnityEngine;

public abstract class Modal : MonoBehaviour
{
    static int modalOpenCount;

    protected void OnEnable()
    {
        //++modalOpenCount;
        //if (InputHandler.instance)
        //    InputHandler.instance.enabled = false;
    }

    protected void OnDisable()
    {
        //if (--modalOpenCount == 0)
        //{
        //    if (InputHandler.instance)
        //        InputHandler.instance.enabled = true;
        //}
    }
}