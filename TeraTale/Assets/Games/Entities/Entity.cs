public abstract class Entity : NetworkScript
{
    protected new void Start()
    {
        StartCoroutine(base.Start());
        Sync("transform.localPosition");
        Sync("transform.localEulerAngles");
    }
}