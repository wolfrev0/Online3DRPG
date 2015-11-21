namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Database server = new Database())
                server.Execute();
        }
    }
}
