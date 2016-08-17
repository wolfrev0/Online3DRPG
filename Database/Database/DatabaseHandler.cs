using System;
using System.IO;
using TeraTaleNet;

namespace Database
{
    class InvalidLoginException : Exception
    {
        public string message;
        public InvalidLoginException(string message)
        { this.message = message; }
    }

    class DatabaseHandler : MessageHandler
    {
        static string accountPath = "Accounts";
        static string serializedPlayerPath = "SerializedPlayer";

        void SignUp(Messenger messenger, string key, SignUp packet)
        {
            try
            {
                using (var account = new StreamWriter(new FileStream(accountPath + "\\" + packet.id, FileMode.Create)))
                {
                    account.WriteLine(packet.pw);
                    account.WriteLine(packet.id);
                }
            }
            catch (Exception e)
            {
                History.Log(e.ToString());
            }
        }

        void LoginQuery(Messenger messenger, string key, LoginQuery query)
        {
            try
            {
                try
                {
                    using (var account = new StreamReader(new FileStream(accountPath + "\\" + query.id, FileMode.Open)))
                    {
                        var pw = account.ReadLine();
                        var name = account.ReadLine();
                        if (query.pw == pw)
                        {
                            messenger.Send("Login", new LoginAnswer(query.confirmID, true, name, "Town"));
                        }
                        else
                        {
                            throw new InvalidLoginException("PW가 다릅니다.");
                        }
                    }
                }
                catch (IOException)
                {
                    throw new InvalidLoginException("존재하지 않는 ID입니다.");
                }
            }
            catch (InvalidLoginException e)
            {
                var answer = new LoginAnswer(query.confirmID, false, "", "");
                answer.message = e.message;
                messenger.Send("Login", answer);
            }
        }

        void MessageHandler.RPCHandler(RPC rpc)
        {
            throw new NotImplementedException();
        }

        void SerializedPlayerQuery(Messenger messenger, string key, SerializedPlayerQuery query)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var data = File.ReadAllBytes(serializedPlayerPath + "\\" + query.player);
                        messenger.Send(query.sender, new SerializedPlayerAnswer(query.player, data));
                        return;
                    }
                    catch (FileNotFoundException)
                    { throw; }
                    catch (IOException)
                    { }
                }
            }
            catch (FileNotFoundException)
            {

                try
                {
                    while (true)
                    {
                        try
                        {
                            var data = File.ReadAllBytes(serializedPlayerPath + "\\Default");
                            messenger.Send(query.sender, new SerializedPlayerAnswer(query.player, data));
                            return;
                        }
                        catch (FileNotFoundException)
                        { throw; }
                        catch (IOException)
                        { }
                    }
                }
                catch (FileNotFoundException)
                {
                    messenger.Send(query.sender, new SerializedPlayerAnswer(query.player, new byte[1]));
                }
            }
        }

        void SerializedPlayerSave(Messenger messenger, string key, SerializedPlayerSave data)
        {
            File.WriteAllBytes(serializedPlayerPath + "\\" + data.player, data.bytes);
        }
    }
}
