using System;

namespace SasTokenService.Entities
{
    /// <summary>
    /// Class To Holds Details For Service Bus Details
    /// </summary>
    public class ServiceBusTokenArgs 
    {
        /// <summary>
        /// Getter and Setter for Field Service Bus name Space. 
        /// </summary>
        public string SbNamespace { get; set; }
        /// <summary>
        /// Getter and Setter for Type of Service Bus Requested
        /// </summary>
        public string EntityType{ get; set; }
        /// <summary>
        /// Getter and Setter for Path Of Entity
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Getter and Setter for Key
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Getter and Setter for Provider of Service Bus
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Varuable to Store Connection String of Service Bus
        /// </summary>
        public string ConnectionString { get; set; }





    }
    /// <summary>
    /// Class to Hold Details for Storage Service Details
    /// </summary>
    public class StorageTokenArgs 
    {
        /// <summary>
        /// Storage account name. This can be alias of storage account,
        /// which can be mapped to the real storage account.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Name of queue, blab, container or table.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Getter and Setter for Account Key Details
        /// </summary>
        public string Accountkey { get; set; }
       /// <summary>
       /// Gettter and Setter for Connection Details For Storage Services
       /// </summary>
        public string ConnectionString { get; set; }



    }
    /// <summary>
    /// Class to Hold Iot Details
    /// </summary>
    public class IoTHubTokenArgs 
    {
        /// <summary>
        /// Getter and Setter for IOT Uri 
        /// </summary>
        public string IoTHubUri { get; set; }
        /// <summary>
        /// Getter and Setter for Device Name
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// Getter and Setter for Device Key
        /// </summary>
        public string DeviceKey { get; set; }
        /// <summary>
        /// Getter and Setter for Duration Field
        /// </summary>
        public TimeSpan Duration { get; internal set; }
    }
}