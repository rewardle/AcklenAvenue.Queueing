using AcklenAvenue.Queueing.Amazon.Sqs.Builder;

namespace AcklenAvenue.Queueing.Serializers.JsonNet
{
    public static class BusBuilderHelper
    {
        public static BusBuilder UseJsonNetMessageSerializer(this BusBuilder busBuilder)
        {
            return busBuilder.UseDeserializer<JsonNetMessageSerializer>().UseSerializer<JsonNetMessageSerializer>();
        }
    }
}