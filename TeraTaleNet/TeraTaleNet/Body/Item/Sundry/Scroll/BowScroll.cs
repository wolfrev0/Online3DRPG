using System.Collections.Generic;

namespace TeraTaleNet
{
    public class BowScroll : Scroll
    {
        public sealed override int price { get { return 20; } }
        
        public override List<IngradientItem> ingredients
        {
            get
            {
                var ret = new List<IngradientItem>();
                ret.Add(new IngradientItem("Log", 10));
                ret.Add(new IngradientItem("IronOre", 3));
                ret.Add(new IngradientItem("Bone", 1));
                return ret;
            }
        }

        public BowScroll()
        { }
    }
}