﻿using ESI4T.Common.Configuration;
using ESI4T.Common.Configuration.Interface;
using ESI4T.Common.Logging;
using ESI4T.Common.Services;
using ESI4T.Common.Services.Helper;
using ESI4T.IndexService.BAL.ContentMapper;
using ESI4T.IndexService.DataContracts;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ESI4T.IndexService.BAL
{
    public class ESI4TIndexManager
    {
        private IPropertyConfiguration propertyConfiguration;
        private static IPropertyConfiguration propConfiguration;
        private static object containerLock;
        public static Uri node;
        public static ConnectionSettings settings;
        public static ElasticClient client;
        public object ServiceConstants { get; private set; }

        /// <summary>
        /// Singleton ESI4TIndexManager static constructor
        /// </summary>
        static ESI4TIndexManager()
        {
            try
            {
                string MongoDBIndexConfigPath = Utility.GetConfigurationValue("SearchIndexServiceConfig");
                propConfiguration = ConfigurationManager.GetInstance().GetConfiguration(MongoDBIndexConfigPath)
                    as IPropertyConfiguration;
                containerLock = new object();
                ESI4TLogger.WriteLog(ELogLevel.DEBUG, "Config Path: " + MongoDBIndexConfigPath);
            }
            catch (Exception ex)
            {
                ESI4TLogger.WriteLog(ELogLevel.ERROR, ex.Message + ex.StackTrace);
                throw ex;
            }
            ESI4TLogger.WriteLog(ELogLevel.DEBUG, "Exiting MI4TIndexManager.MI4TIndexManager()");
        }
        public ESI4TIndexManager(string Langauge)
        {

            ESI4TLogger.WriteLog(ELogLevel.INFO, "Entering MI4TIndexManager:" +
            Langauge);
            try
            {
                string MongoDBURL = propConfiguration.GetString(ESI4TServicesConstants.mongoDB_URL);
                ESI4TLogger.WriteLog(ELogLevel.INFO, "Mongo URL: " + MongoDBURL);
            }
            catch (Exception ex)
            {
                ESI4TLogger.WriteLog(ELogLevel.ERROR, ex.Message + ex.StackTrace);
                throw;
            }
        }
        public DataContracts.IndexResponse AddDocument(IndexRequest query)
        {
            ESI4TLogger.WriteLog(ELogLevel.INFO,
           "Entering MI4TIndexManager.AddDocument for TCM URI: " +
           query.ItemURI);

            DataContracts.IndexResponse response = new DataContracts.IndexResponse();

            OperationResult result = OperationResult.Failure;
            try
            {
                XmlDocument doc = new XmlDocument();
                string ID = string.Empty;
                doc.LoadXml(Utility.UpdateContentTypeXML(Regex.Replace(query.DCP.ToString(), @"\b'\b", "")));
                var bln = Deserialize<Esnews>(doc);

                var conString = propConfiguration.GetString(ESI4TServicesConstants.mongoDB_URL);
                ESI4TLogger.WriteLog(ELogLevel.INFO, "conString: " +
           conString);

                node = new Uri("http://localhost:9200");
                settings = new ConnectionSettings(node);
                settings.DefaultIndex("fromelasticstoweb8");
                var client = new Nest.ElasticClient(settings);
                var responseBool = client.Index(bln);
                result = OperationResult.Success;

            }
            catch (Exception ex)
            {
                string logString = ESI4TServiceConstants.LOG_MESSAGE + Environment.NewLine;

                logString = string.Concat(logString,
                                          Environment.NewLine,
                                          string.Format("{0}{1}", ex.Message, ex.StackTrace));

                ESI4TLogger.WriteLog(ELogLevel.ERROR, logString);
                result = OperationResult.Failure;
            }
            response.Result = (int)result;
            ESI4TLogger.WriteLog(ELogLevel.INFO,
                                  "Exiting MI4TIndexManager.AddDocument, Result: " +
                                   result.ToString());

            return response;
        }

        /// <summary>
        /// This method removes an index from Mongo 
        /// </summary>
        /// <param name="query">IndexRequest containing delete criteria</param>
        /// <returns>IndexResponse indicating success or failure</returns>
        public DataContracts.IndexResponse RemoveDocument(IndexRequest query)
        {
            ESI4TLogger.WriteLog(ELogLevel.INFO, "Entering MI4TIndexManager.RemoveDocument for TCM URI: " +
                                 query.ItemURI);
            DataContracts.IndexResponse response = new DataContracts.IndexResponse();

            OperationResult result = OperationResult.Failure;
            try
            {
                XmlDocument doc = new XmlDocument();
                string ID = string.Empty;
                doc.LoadXml(Utility.UpdateContentTypeXML(Regex.Replace(query.DCP.ToString(), @"\b'\b", "")));
                var bln = Deserialize<Esnews>(doc);
                node = new Uri("http://localhost:9200");

                settings = new ConnectionSettings(node);

                settings.DefaultIndex("fromelasticstoweb8");
                var client = new Nest.ElasticClient(settings);
                var re = client.Delete<Esnews>(bln.Title);



                result = OperationResult.Success;
                ESI4TLogger.WriteLog(ELogLevel.INFO, "Exit MI4TIndexManager.RemoveDocument for TCM URI: " +
                                 query.ItemURI + " result " + result);
            }
            catch (Exception ex)
            {
                string logString = ESI4TServiceConstants.LOG_MESSAGE + Environment.NewLine;
                logString = string.Concat(logString,
                                          string.Format("Item URI : {0}", query.ItemURI),
                                          Environment.NewLine, string.Format("{0}{1}", ex.Message, ex.StackTrace));
                ESI4TLogger.WriteLog(ELogLevel.ERROR, logString);
                result = OperationResult.Failure;
            }

            response.Result = (int)result;

            ESI4TLogger.WriteLog(ELogLevel.INFO,
                                  "Exiting MI4TIndexManager.RemoveDocument, Result: " +
                                  result.ToString());

            return response;
        }
        public static T Deserialize<T>(XmlDocument xmlDocument)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            StringReader reader = new StringReader(xmlDocument.InnerXml);
            XmlReader xmlReader = new XmlTextReader(reader);
            //Deserialize the object.
            return (T)ser.Deserialize(xmlReader);
        }
    }
}