using UnityEngine;

namespace TeraTaleNet
{
    public class ItemSolidArgument : Body
    {
        public Item item;
        public Vector3 spawnPos;
        public float xzAngle;
        public float xzSpeed;

        public ItemSolidArgument(Item item, Vector3 spawnPos, float xzAngle, float xzSpeed)
        {
            this.item = item;
            this.spawnPos = spawnPos;
            this.xzAngle = xzAngle;
            this.xzSpeed = xzSpeed;
        }

        public ItemSolidArgument()
        { }
    }
}