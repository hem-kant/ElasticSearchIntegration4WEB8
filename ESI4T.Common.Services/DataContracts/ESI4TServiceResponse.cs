using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ESI4T.Common.Services.DataContracts
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T">Service specific response payload data contact</typeparam>
    [DataContract]
    public class ESI4TServiceResponse<T>
    {
        public ESI4TServiceResponse()
        {
            ResponseContext = new ESI4TResponseContext();
        }
        private T servicePayload;


        /// <summary>
        /// The service payload is a generic implementation for any service which would hold the service
        /// response data send to the consumer
        /// </summary>
        [DataMember]
        public T ServicePayload
        {
            get { return servicePayload; }
            set { servicePayload = value; }
        }

        private ESI4TResponseContext responseContext;


        /// <summary>
        /// The response context would hold the data related to response which would be populated by  
        /// underlying service/service client
        /// </summary>
        [DataMember]
        public ESI4TResponseContext ResponseContext
        {
            get { return responseContext; }
            set { responseContext = value; }
        }
    }
}
