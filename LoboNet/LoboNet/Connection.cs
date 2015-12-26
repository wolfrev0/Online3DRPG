using System;
using System.Net.Sockets;

namespace LoboNet
{
    public class Connection : IDisposable
    {
        Socket _socket;
        NetworkStream _stream;
        bool _disposed = false;

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
                if (readLen <= 0)
                    throw new Exception("Disconnected.");
            } while (size > 0);
        }

        public void Write(byte[] buffer, int size, int offset = 0)
        {
            _stream.Write(buffer, offset, size);
        }

        public bool CanRead()
        {
            return _socket.Poll(0, SelectMode.SelectRead);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _stream.Close();
                }

            }
            _disposed = true;
        }

        ~Connection()
        {
            Dispose(false);
        }
    }
}
