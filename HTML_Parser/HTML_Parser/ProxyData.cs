using System.Runtime.Serialization;

namespace HTML_Parser
{
    [DataContract]
    public class ProxyData
    {
        [DataMember]
        public string Host { get; set; }
        [DataMember]
        public string Port { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Password { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
