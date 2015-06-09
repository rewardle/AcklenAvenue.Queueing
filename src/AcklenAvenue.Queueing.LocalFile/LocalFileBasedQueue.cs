using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace AcklenAvenue.Queueing.LocalFile
{
    public abstract class LocalFileBasedQueue<T> : IQueuePusher<T>,
                                                   IQueuePuller<T>,
                                                   IMessageReceiver<T>,
                                                   IMessageSender<T>
    {
        readonly string _queueFilePath;

        protected LocalFileBasedQueue(string queueFilePath)
        {
            if (string.IsNullOrEmpty(queueFilePath))
            {
                throw new ArgumentNullException("queueFilePath");
            }

            _queueFilePath = queueFilePath;
        }

        public IEnumerable<IMessageReceived<T>> Receive()
        {
            return new[] { new FileMessageRecived<T>(Pull()) };
        }

        public ISendResponse Send(T message)
        {
            Push(message);

            return new FileSendReponse("Ok");
        }

        public T Pull()
        {
            return PullFromTopOfFile(_queueFilePath);
        }

        public void Push(T @event)
        {
            AddToBottomOfFile(_queueFilePath, @event);
        }

        protected void AddToBottomOfFile(string filename, T @event)
        {
            CreateIfNotExists(filename);
            List<T> commandQueueItems = GetObjectsFromFile(filename);
            commandQueueItems.Add(@event);
            List<T> itemsForReturnToTextFile = PrepareItemsForReturnToTextFile(commandQueueItems);
            ReplaceWithNewListOfObjects(filename, itemsForReturnToTextFile);
        }

        protected T PullFromTopOfFile(string filename)
        {
            CreateIfNotExists(filename);
            List<T> commandQueueItems = GetObjectsFromFile(filename);
            T firstOrDefault = commandQueueItems.FirstOrDefault();

            if (firstOrDefault == null)
            {
                return default(T);
            }

            commandQueueItems.Remove(firstOrDefault);
            ReplaceWithNewListOfObjects(filename, PrepareItemsForReturnToTextFile(commandQueueItems));
            return firstOrDefault;
        }

        protected abstract List<T> PrepareItemsForReturnToTextFile(List<T> list);

        void ReplaceWithNewListOfObjects(string filename, List<T> commandQueueItems)
        {
            TryFileOperation(
                () =>
                    {
                        string json = SerializeQueueItems(commandQueueItems);
                        File.WriteAllText(filename, json);
                        return null;
                    });
        }

        protected virtual string SerializeQueueItems(List<T> commandQueueItems)
        {
            return JsonConvert.SerializeObject(commandQueueItems);
        }

        List<T> GetObjectsFromFile(string filename)
        {
            string jsonFromFile = TryFileOperation(() => File.ReadAllText(filename));
            List<T> commandQueueItems = string.IsNullOrEmpty(jsonFromFile)
                                            ? new List<T>()
                                            : DeserializeQueueItems(jsonFromFile);
            return PrepareItemsForReturnToTextFile(commandQueueItems);
        }

        protected virtual List<T> DeserializeQueueItems(string jsonFromFile)
        {
            return JsonConvert.DeserializeObject<List<T>>(jsonFromFile);
        }

        static string TryFileOperation(Func<string> func)
        {
            string text = "";
            DateTime start = DateTime.Now;
            bool gotValue = false;
            while (!gotValue && (start - DateTime.Now).TotalSeconds < 10)
            {
                try
                {
                    text = func();
                    gotValue = true;
                }
                catch (IOException)
                {
                }
            }
            return text;
        }

        static void CreateIfNotExists(string filename)
        {
            if (!File.Exists(filename))
            {
                using (StreamWriter streamWriter = File.AppendText(filename))
                {
                    streamWriter.Close();
                }
            }
        }
    }
}