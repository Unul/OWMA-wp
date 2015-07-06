using System.Runtime.Serialization;
using System.Collections.Generic;

namespace OWMA_wp.Common
{
    [DataContract]
    public class ErrorHandler
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public List<Error> errors { get; set; }
    }

    [DataContract]
    public class Error
    {
        [DataMember]
        public string error_code { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public string developper_message { get; set; }
    }
}