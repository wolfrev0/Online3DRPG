using System.Collections.Generic;
using System.Threading;
using LoboNet;

namespace TeraTaleNet
{
    public class NetworkManager
    {
        //static readonly NetworkManager _instance = new NetworkManager();
        //Queue<IBody> _sendQueue = new Queue<IBody>();
        //Queue<IBody> _recvQueue = new Queue<IBody>();
        //Thread _accepter;
        //Thread _messenger;
        //bool _stopAccepter;
        //bool _stopMessenger;
        //TcpServer server;//어떻게 할까. 차라리 Connection 배열을 인자로 받아서 관리하는것도...

        //public static NetworkManager instance { get { return _instance; } }
        
        //public void OpenServer()
        //{
        //    RunAccepter();
        //    RunMessenger();
        //}

        //public void CloseServer()
        //{
        //    StopAccepter();
        //    StopMessenger();
        //}

        //public void Connect()
        //{
        //    //TODO : Connect at server code
        //    RunMessenger();
        //}

        //public void Disconnect()
        //{
        //    //TODO : Disconnect at server code
        //    StopMessenger();
        //}

        //void RunAccepter()
        //{
        //    _stopAccepter = false;
        //    _accepter = new Thread(Accepter);
        //    _accepter.Start();
        //}

        //void RunMessenger()
        //{
        //    _stopMessenger = false;
        //    _messenger = new Thread(Messenger);
        //    _messenger.Start();
        //}

        //void StopAccepter()
        //{
        //    _stopAccepter = true;
        //    _accepter.Join();
        //    _accepter = null;
        //}

        //public void StopMessenger()
        //{
        //    _stopMessenger = true;
        //    _messenger.Start();
        //    _messenger = null;
        //}

        //void Accepter()
        //{
        //    while(_stopAccepter == false)
        //    {

        //    }
        //}

        //void Messenger()
        //{
        //    while (_stopMessenger == false)
        //    {

        //    }
        //}
    }
}