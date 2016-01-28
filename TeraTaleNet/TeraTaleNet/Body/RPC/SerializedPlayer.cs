using TeraTaleNet;

public class SerializedPlayer : RPC
{
    public byte[] bytes;

    public SerializedPlayer(ISerializable player)
        :base(RPCType.All)
    {
        bytes = player.Serialize();
    }

    public SerializedPlayer(byte[] data)
        : base(data)
    { }
}