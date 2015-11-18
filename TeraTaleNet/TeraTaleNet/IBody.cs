namespace TeraTaleNet
{
    public interface IBody : ISerializable
    {
        Header CreateHeader();
        //Can optimize by dirty flag.
        int SerializedSize();
    }
}