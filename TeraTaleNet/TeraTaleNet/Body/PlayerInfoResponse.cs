namespace TeraTaleNet
{
    public class PlayerInfoResponse : IBody
    {
        string _nickName;
        string _world;

        public string nickName { get { return _nickName; } }
        public string world { get { return _world; } }

        public PlayerInfoResponse(string nickName, string world)
        {
            _nickName = nickName;
            _world = world;
        }

        public PlayerInfoResponse(byte[] buffer)
        {
            Deserialize(buffer);
        }

        public Header CreateHeader()
        {
            return new Header(PacketType.PlayerInfoResponse, SerializedSize());
        }

        public int SerializedSize()
        {
            int ret = 0;
            ret += Serializer.SerializedSize(nickName);
            ret += Serializer.SerializedSize(world);
            return ret;
        }

        public byte[] Serialize()
        {
            var nickNameBytes = Serializer.Serialize(nickName);
            var worldBytes = Serializer.Serialize(world);

            var ret = new byte[SerializedSize()];

            int offset = 0;
            nickNameBytes.CopyTo(ret, offset);
            offset += nickNameBytes.Length;
            worldBytes.CopyTo(ret, offset);
            offset += worldBytes.Length;

            return ret;
        }

        public void Deserialize(byte[] buffer)
        {
            int offset = 0;
            _nickName = Serializer.ToString(buffer, offset);
            offset += Serializer.SerializedSize(nickName);
            _world = Serializer.ToString(buffer, offset);
            offset += Serializer.SerializedSize(world);
        }
    }
}