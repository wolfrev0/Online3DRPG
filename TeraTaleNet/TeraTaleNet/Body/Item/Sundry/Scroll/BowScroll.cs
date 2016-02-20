using System;
using System.Collections.Generic;

namespace TeraTaleNet
{
    public class BowScroll : Scroll
    {
        public sealed override string ingameName { get { return "고무줄 활 조합서"; } }
        public sealed override int price { get { return 20; } }
        
        public override List<Ingradient> ingredients
        {
            get
            {
                var ret = new List<Ingradient>();
                ret.Add(new Ingradient(new Log(), 10));
                ret.Add(new Ingradient(new IronOre(), 3));
                ret.Add(new Ingradient(new Bone(), 1));
                return ret;
            }
        }

        public sealed override Item output { get { return new Bow(); } }

        public BowScroll()
        { }
    }
}