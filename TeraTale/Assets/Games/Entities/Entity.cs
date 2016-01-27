using TeraTaleNet;

public abstract class Entity : NetworkScript
{
    protected void OnEnable()
    {
        Sync("transform.localPosition");
        Sync("transform.localEulerAngles");
    }
}