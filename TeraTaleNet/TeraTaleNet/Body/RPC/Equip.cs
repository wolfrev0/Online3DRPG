namespace TeraTaleNet
{
    public class Equip : RPC
    {
        public IAutoSerializable equipment;

        public Equip(Equipment equipment)
            : base(RPCType.All)
        {
            this.equipment = equipment;
        }

        public Equip()
            : base(RPCType.All)
        { }
    }
}