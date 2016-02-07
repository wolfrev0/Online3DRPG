public abstract class Entity : NetworkScript
{
    protected new void OnEnable()
    {
        base.OnEnable();
        Sync("transform.localPosition");
        Sync("transform.localEulerAngles");
    }
}