using System;
using System.Net.Sockets;

namespace LoboNet
{
    public class Connection : IDisposable
    {
        Socket _socket;
        NetworkStream _stream;
        bool disposed = false;

        public Connection(Socket socket)
        {
            _socket = socket;
            _stream = new NetworkStream(socket);
        }

        public void Read(byte[] buffer, int size, int offset = 0)
        {
            do
            {
                int readLen = _stream.Read(buffer, offset, size);
                offset += readLen;
                size -= readLen;
            } while (size > 0);
        }

        public void Write(byte[] buffer, int size, int offset = 0)
        {
            _stream.Write(buffer, offset, size);
        }

        public bool PollRead()
        {
            return _socket.Poll(0, SelectMode.SelectRead);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            _socket.Shutdown(SocketShutdown.Both);
            _stream.Close();
            disposed = true;
        }
    }
}
