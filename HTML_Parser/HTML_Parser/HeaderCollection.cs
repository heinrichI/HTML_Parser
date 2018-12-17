using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser
{
    [DataContract]
    class HeaderCollection
    {
        [DataMember]
        private Dictionary<string, string> headers = new Dictionary<string, string>();
    }
}
