namespace TeraTaleNet
{
    public interface ISerializable
    {
        byte[] Serialize();
        void Deserialize(byte[] buffer);
    }
}