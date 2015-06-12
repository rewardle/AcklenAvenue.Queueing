namespace AcklenAvenue.Queueing.LocalFile
{
    public class FileSendReponse : ISendResponse
    {
        public string Status { get; set; }

        public FileSendReponse(string status)
        {
            Status = status;
        }
    }
}