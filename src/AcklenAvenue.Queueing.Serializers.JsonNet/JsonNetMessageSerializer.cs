using AcklenAvenue.Queueing.Amazon.Sqs;
using Newtonsoft.Json;

namespace AcklenAvenue.Queueing.Serializers.JsonNet
{
    public class JsonNetMessageSerializer : IMessageSerializer, IMessageDeserializer
    {
        readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonNetMessageSerializer()
        {
            _jsonSerializerSettings = new JsonSerializerSettings();
        }

        public JsonNetMessageSerializer(JsonSerializerSettings settings)
        {
            _jsonSerializerSettings = settings;
        }

        public string Serialize(object message)
        {
            return JsonConvert.SerializeObject(message, _jsonSerializerSettings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
        }
    }
}