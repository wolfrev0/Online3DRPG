using TeraTaleNet;

public class SerializedPlayer : RPC
{
    public byte[] data;

    public SerializedPlayer(IAutoSerializable player)
        :base(RPCType.Others)
    {
        data = player.Serialize();
    }

    public SerializedPlayer()
    { }
}