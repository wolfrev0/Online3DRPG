using TeraTaleNet;

public class SerializedPlayer : RPC
{
    public byte[] data;

    public SerializedPlayer(ISerializable player)
        :base(RPCType.All)
    {
        data = player.Serialize();
    }

    public SerializedPlayer(byte[] data)
        : base(data)
    { }
}