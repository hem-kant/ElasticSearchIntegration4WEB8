﻿using ESI4T.Common.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ESI4T.Common.Services.DataContracts
{
    /// <summary>
    /// GenericIndexingReposneContext class holds the information about the service response context
    /// like faults(error) information and other useful information related to service response
    /// </summary>
    [DataContract]
    public class ESI4TResponseContext
    {
        public ESI4TResponseContext()
        {
            FaultCollection = new Collection<ESI4TServiceFault>();
        }

        //Contains the Environment Context of the service response
        [DataMember]
        public ESI4TEnvironmentContext EnvironmentContext { get; set; }


        private Collection<ESI4TServiceFault> faultCollection;

        /// <summary>
        /// Fault Collection, a collection of GenericIndexingServiceFault
        /// </summary>
        [DataMember]
        public Collection<ESI4TServiceFault> FaultCollection
        {
            get { return faultCollection; }
            set { faultCollection = value; }
        }

        /// <summary>
        /// public to check if response got any fault
        /// </summary>
        public bool IsFault
        {
            get { return faultCollection.Count > 0; }
            set { }
        }
    }
}
