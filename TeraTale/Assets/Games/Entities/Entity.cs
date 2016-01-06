public class Entity : NetworkScript
{
    protected new void Start()
    {
        StartCoroutine(base.Start());
    }
}