namespace TeraTaleNet
{
    public interface IAutoSerializable
    {
        byte[] Serialize();
        void Deserialize(byte[] buffer);
        int SerializedSize();
        Header CreateHeader();
    }
}