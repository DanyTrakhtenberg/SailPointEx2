using ConnectionPool;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ServiceExercise
{
    public class Service : IService
    {
        BlockingCollection<int> _tasksBlockingCollection = new BlockingCollection<int>();
        Task<int>[] _tasks;
        public Service(int taskCount)
        {
            if (taskCount<0 )
            {
                throw new Exception("Number of connections must be a positive number");
            }
            _tasks = new Task<int>[taskCount];
            // Create and start new Task for each consumer.
            for (int i = 0; i < taskCount; i++)
            {
                _tasks[i] = Task.Factory.StartNew(()=>Consumer(new Connection()));
            }

        }
        public int getSummary()
        {
            _tasksBlockingCollection.CompleteAdding();
            int sum = 0;
            foreach (var task in _tasks)
            {
                sum += task.Result;
            }
            return sum;
        }

        int Consumer(Connection connection)
        {
            int localSum = 0;
            foreach (int number in _tasksBlockingCollection.GetConsumingEnumerable())
            {
                int connectionResult = connection.runCommand(number);
                localSum += connectionResult;
            }
            return localSum;
        }

        public void sendRequest(Request request)
        {
            _tasksBlockingCollection.Add(request.Command);
        }
    }
}
