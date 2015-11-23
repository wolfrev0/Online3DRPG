using System;
using System.IO;
using TeraTaleNet;

namespace Database
{
    class DatabaseHandler : MessageHandler
    {
        static string accountLocation = "Accounts\\";
        static string playerInfoLocation = "PlayerInfo\\";
        
        void LoginRequest(Messenger messenger, string key, LoginRequest request)
        {
            if (key != "Login")
                throw new ArgumentException("Received from unexpected key.");
            
            LoginResponse response;
            try
            {
                using (var stream = new StreamReader(new FileStream(accountLocation + request.id, FileMode.Open)))
                {
                    string pw = stream.ReadLine();
                    string nickName = stream.ReadLine();
                    if (request.pw == pw)
                    {
                        response = new LoginResponse(true, RejectedReason.Accepted, nickName, request.confirmID);
                    }
                    else
                    {
                        response = new LoginResponse(false, RejectedReason.InvalidPW, "Login", request.confirmID);
                    }
                }
            }
            catch (IOException)
            {
                response = new LoginResponse(false, RejectedReason.InvalidID, "Login", request.confirmID);
            }
            messenger.Send(key, response);
        }
        
        void PlayerInfoRequest(Messenger messenger, string key, PlayerInfoRequest request)
        {
            if (key != "GameServer")
                throw new ArgumentException("Received from unexpected key.");
            
            using (var stream = new StreamReader(new FileStream(playerInfoLocation + request.nickName, FileMode.Open)))
            {
                string world = stream.ReadLine();

                PlayerInfoResponse response = new PlayerInfoResponse(request.nickName, world);
                messenger.Send(key, response);
            }
        }
    }
}
