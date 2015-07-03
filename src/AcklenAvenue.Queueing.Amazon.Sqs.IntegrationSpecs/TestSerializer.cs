using Newtonsoft.Json;

namespace AcklenAvenue.Queueing.Amazon.Sqs.Specs.Integration
{
    public class TestSerializer : IMessageSerializer, IMessageDeserializer
    {
        public string Serialize(object message)
        {
            return JsonConvert.SerializeObject(message);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}