namespace TeraTaleNet
{
    public class ItemSolidArgument : Body
    {
        public Item item;
        public float xzAngle;
        public float xzSpeed;

        public ItemSolidArgument(Item item, float xzAngle, float xzSpeed)
        {
            this.item = item;
            this.xzAngle = xzAngle;
            this.xzSpeed = xzSpeed;
        }

        public ItemSolidArgument()
        { }
    }
}