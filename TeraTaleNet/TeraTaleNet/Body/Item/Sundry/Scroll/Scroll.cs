using System.Collections.Generic;

namespace TeraTaleNet
{
    public abstract class Scroll : Sundry
    {
        public class IngradientItem
        {
            string itemName;
            int count;

            public IngradientItem(string itemName, int count)
            {
                this.itemName = itemName;
                this.count = count;
            }
        }

        public sealed override string effectExplanation { get { return ""; } }
        public sealed override string explanation { get { return "조합 재료가 빼곡히 적혀있다."; } }
        public abstract List<IngradientItem> ingredients { get; }

        public Scroll()
        { }
    }
}