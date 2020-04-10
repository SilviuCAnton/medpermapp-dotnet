using Newtonsoft.Json;

namespace medpermapp.api.Web
{
    public class SocketMessage<T>
    {
        public string MessageType { get; set; }
        public T Payload { get; set; }
    }
}