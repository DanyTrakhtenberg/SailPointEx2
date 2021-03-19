using ServiceExercise;
using System;

namespace ConnectionPool {
    public class Program {

        private const int CONNETION_COUNT = 4;

        static void Main(string[] args) {
            IService service = new Service(CONNETION_COUNT); /*** CREATE YOUR SERVICE HERE ***/

            RequestSender requestSender = new RequestSender(service);

            requestSender.sendRequests();

            int result = service.getSummary();

            Console.WriteLine($"Result is {result}");
        }
    }
}
