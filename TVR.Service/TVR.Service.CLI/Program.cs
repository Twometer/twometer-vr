using TVR.Service.Core;

namespace TVR.Service.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new TvrService();
            service.Start();
        }
    }
}
