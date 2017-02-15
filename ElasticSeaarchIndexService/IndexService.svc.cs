using ESI4T.Common.ExceptionManagement;
using ESI4T.Common.Logging;
using ESI4T.Common.Services;
using ESI4T.Common.Services.DataContracts;
using ESI4T.Common.Services.Helper;
using ESI4T.IndexService.BAL;
using ESI4T.IndexService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace ElasticSearchIndexService
{
    /// <summary>
    /// This class exposes the MongoDB Index Service methods
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class IndexService : IIndexService
    {
        /// <summary>
        /// This method creates an index in Mongo 
        /// </summary>
        /// <param name="query">An object of IndexRequest need to be passed</param>
        /// <returns>Object of type IndexResponse is returned which has field Result as 0 for success and 1 for failure</returns>
        [WebInvoke(UriTemplate = "/AddDocument/", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public ESI4TServiceResponse<IndexResponse> AddDocument(ESI4TServiceRequest<IndexRequest> query)
        {

            ESI4TLogger.WriteLog(ELogLevel.INFO, "Enter into method IndexService.AddDocumnet()");
            ESI4TServiceResponse<IndexResponse> serviceResponse = new ESI4TServiceResponse<IndexResponse>();
            try
            {

                string language = query.ServicePayload.LanguageInRequest;
                IndexResponse resultValue;

                ESI4TIndexManager indexManager = new ESI4TIndexManager(language);
                resultValue = indexManager.AddDocument(query.ServicePayload);
                ESI4TLogger.WriteLog(ELogLevel.INFO, "AddDocumnet is called publish is true");
                serviceResponse.ServicePayload = resultValue;
            }
            catch (Exception ex)
            {

                serviceResponse.ServicePayload = new IndexResponse();
                serviceResponse.ServicePayload.Result = 1;
                serviceResponse.ServicePayload.ErrorMessage = "AddDocumnet is not called ispublish is false";
                ESI4TLogger.WriteLog(ELogLevel.INFO, "AddDocumnet is not called ispublish is false" + ex.Message);
            }
            return serviceResponse;
        }

        /// <summary>
        /// This method removes an index from ES
        /// </summary>
        /// <param name="query">An object of IndexRequest need to be passed</param>
        /// <returns>Object of type IndexResponse is returned which has field Result as 0 for success and 1 for failure</returns>
        [WebInvoke(UriTemplate = "/RemoveDocument/", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public ESI4TServiceResponse<IndexResponse> RemoveDocument(ESI4TServiceRequest<IndexRequest> query)
        {
            ESI4TLogger.WriteLog(ELogLevel.INFO, "Entering into method IndexService.RemoveDocument");
            ESI4TServiceResponse<IndexResponse> serviceResponse = new ESI4TServiceResponse<IndexResponse>();
            try
            {
                //serviceResponse.ServicePayload = new IndexResponse();
                //serviceResponse.ServicePayload.Result = 1;
                ESI4TLogger.WriteLog(ELogLevel.INFO, "RemoveDocument is  called publish is true1 ");

                IndexResponse resultValue;
                ESI4TIndexManager indexManager = new ESI4TIndexManager(query.ServicePayload.LanguageInRequest);
                resultValue = indexManager.RemoveDocument(query.ServicePayload);
                serviceResponse.ServicePayload = resultValue;
                ESI4TLogger.WriteLog(ELogLevel.INFO, "RemoveDocument is  called publish is true 2");
            }
            catch (Exception ex)
            {
                serviceResponse.ServicePayload = new IndexResponse();
                serviceResponse.ServicePayload.Result = 0;
                serviceResponse.ServicePayload.ErrorMessage = "RemoveDocument is not called ispublish is false";
                ESI4TLogger.WriteLog(ELogLevel.INFO, "RemoveDocument is not called ispublish is false");
                string logString = ESI4TServiceConstants.LOG_MESSAGE + Environment.NewLine;
                string request = query != null ? query.ToJSONText() : "Request = NULL";
                logString = string.Concat(logString, string.Format("Service Request: {0}", request),
                                            Environment.NewLine, string.Format("{0}{1}", ex.Message, ex.StackTrace));
                ESI4TLogger.WriteLog(ELogLevel.ERROR, logString);
                CatchException<IndexResponse>(ex, serviceResponse);
            }
            ESI4TLogger.WriteLog(ELogLevel.INFO, "Exiting from method IndexService.RemoveDocument");
            return serviceResponse;
        }

        private void CatchException<T>(Exception ex, ESI4TServiceResponse<T> serviceResponse)
        {
            ESI4TServiceFault fault = new ESI4TServiceFault();
            ExceptionHelper.HandleException(ex, out fault);
            serviceResponse.ResponseContext.FaultCollection.Add(fault);
        }
    }
}
