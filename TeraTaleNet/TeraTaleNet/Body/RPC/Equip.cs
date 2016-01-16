namespace TeraTaleNet
{
    public class Equip : RPC
    {
        public Packet equipment;

        public Equip(Equipment equipment)
            : base(RPCType.All)
        {
            this.equipment = equipment;
        }

        public Equip(byte[] data)
            : base(data)
        { }
    }
}