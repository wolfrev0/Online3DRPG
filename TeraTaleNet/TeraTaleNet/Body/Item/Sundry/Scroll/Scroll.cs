using System.Collections.Generic;

namespace TeraTaleNet
{
    public abstract class Scroll : Sundry
    {
        public delegate void OnUse(Item item, object player);
        static public OnUse onUse;

        public class Ingradient
        {
            public Item item;
            public int count;

            public Ingradient(Item item, int count)
            {
                this.item = item;
                this.count = count;
            }
        }

        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return "조합 재료가 빼곡히 적혀있다."; } }
        public abstract List<Ingradient> ingredients { get; }
        public abstract Item output { get; }

        public Scroll()
        { }

        public override void Use(object player)
        {
            onUse(this, player);
        }
    }
}