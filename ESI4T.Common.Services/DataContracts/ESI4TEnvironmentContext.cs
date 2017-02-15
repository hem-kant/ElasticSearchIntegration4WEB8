using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ESI4T.Common.Services.DataContracts
{
    [DataContract]
    public class ESI4TEnvironmentContext
    {
        /// <summary>
        /// This is the base path of the binary file storage location
        /// </summary>
        [DataMember]
        public string BinaryFileBasePath { get; set; }
    }
}
