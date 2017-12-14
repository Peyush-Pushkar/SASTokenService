# SASTokenService
Shared access signature (SAS) is a powerful way to grant limited
access to objects in your storage account to other Clients, without
having to expose your account key. Shared Access Signature (SAS)Token 
Service for AzureServices is a very dynamic project, which
deals with generation and returning the generated SAS to the User via UI. 
## Getting Started
This Service aims to provide users shared access signatures for the azure service bus , storage services and Iot Hub.

## Technologies/ Framework used
This Application makes use of below given technologies and framework.
Html  
1.C#  
2.Angular JS  
3.SQL Server  
4.Web API  
5.Azure Cloud  

## Azure Service Bus
Service Bus is a multi-tenant cloud service, which means that the service is shared by multiple users. Each user, such as an application developer, creates a namespace, then defines the communication mechanisms she needs within that namespace. 
This Service facilitates SAS token for EventHub, Queue and Topic services getting hosted on service bus platform.

## Azure Storage
Azure Storage is the cloud storage solution for modern applications that rely on durability ,availability, and
scalability to meet the needs of their customers. Cloud computing enables new scenarios for applications requiring scalable, durable ,and highly available storage for their data. Azur eStorage makes it possible for
developers to build large-scale applications to support new scenarios, Azure Storage also provides the storage
foundation f or Azure Virtual Machines ,a further testament to its robustness.
This Service facilitates SAS token for Queue ,Table and Blob Storage services getting hosted on  Azure Storage platform. 

**Blob Storage** -stores unstructured object data. A blob can be any type of text or binary data, such as a document, media file, or application installer. Blob storage is also referred to as Object storage.

**Table Storage** -stores structured datasets. Table storage is a NoSQL key-attribute data store, which allows for rapid development and fast access to large quantities of data.

**Queue Storage** -provides reliable messaging for workflow processing and for communication between components of cloud services
## Azure IOT Hub
Azure IoT Hub is a fully managed service that enables reliable and secure bidirectional communications between millions of IoT devices and a solution back end.
This Service facilitates SAS token for a device connected with Iot Hub

##   Register Your Device
User must have its own device registered with this application in order to utilize its SAS creation functionality.
User can register its device from the Device Registry Tab. click on the Add device button and fill the register device page to register your device. 
You will be provided with DeviceId and Device key which must be used on order to use the SAS creation functionality of application.

## Customize the code 
This application can be customize for other account details . Application reads all
account related information from **application.json** file which can be configured for any other account .application.Json file has 3 providers Section. Below are the details
ProviderId: "STORAGE.SAS.01" this section holds all details about  Storage Account
ProviderId: "SB.SAS.01" this section holds all details about Service Bus Account.
ProviderId: "IoTHub.SAS.01" this section holds all details about IOT hub Account.

###  Configuration for Storage Account
In order to configure the application for a different Storage account please provide below details in the application Json file.  

**ProviderId**:STORAGE.SAS.01(must not change this value).  
**AccountKey_TableStorage**:  Details of account key for Table Storage to be stored here.  
**AccountKey_QueueStorage**:Details of account key for Queue Storage to be stored here.  
**ConnectionStringBloB**:Details of connection string for Blob Storage to be stored here.  


###  Configuration for Service Bus Account

In order to confugure the application for a different Service account please provide below details in the application Json file.  

**ProviderId**:STORAGE.SAS.01(must not change this value).  
**ConnectionStringServiceBus**: Details of connection String for Storage account to be stored here.  
**keyName_Topic**:Key name of Topic Service Bus account to be stored here.  
**keyName_Queue**:Key name of Queue Service Bus account to be stored here.  
**keyName_EventHub**:Key name of Event Hub Service Bus account to be stored here.  
**Provider**:Provider of Storage account to be stored here.  


### Configuration for IOT Hub Account
In order to configure the application for a different IOT Hub account please provide below details in the application Json file.  

**ProviderId**:"IoTHub.SAS.01"(must not change this value).  
**IoTHubUri**:IoTHubUri for the IOT Hub Account.  
**DeviceKey**:Device Key for IOT Hub Device.  


## Request Parameters  
Apart from generic details of account which is stored in application.Json file User need to provide specific information based on SAS it requires .
below section describes the parameters must be provided by user to consume the service 

### Request Parameters  For Topic Service Bus
User must provide below details when accessing token generation feature of Service for Topic Service Bus.  

**providerId**: SB.SAS.01(must not change this value).  
**sbNamespace**: Service bus Namespace where desired Topic is available.   
**path**: Name of Topic whose access is to be Shared.   
**entityType**: Topic(must not change this value).  

### Request Parameters  For Queue Service Bus
User must provide below details when accessing token generation feature of Service for Queue Service Bus.  

**providerId**: SB.SAS.01(must not change this value).  
**sbNamespace**: Service bus Namespace where desired Queue is available.  
**path**: Name of Queue whose access is to be Shared.  
**entityType**: QueueSB(must not change this value).  

### Request Parameters  For EventHub Service Bus
User must provide below details when accessing token generation feature of Service for Event hub Service Bus.  

**providerId**: SB.SAS.01(must not change this value).  
**sbNamespace**: Service bus Namespace where desired eventhub is available.  
**path**: Name of eventhub whose access is to be Shared.   
**entityType**: eventhub(must not change this value).  

### Request Parameters  For Queue Storage Account
User must provide below details when accessing token generation feature of Service for Queue Storage Account.    

**providerId**: 'STORAGE.SAS.01'(must not change this value).  
**accountName**:Azure storage service account Name where desired Queue is available.  
**name**: Storage queue name whose access is to be given.  

### Request Parameters  For Table Storage Account
User must provide below details when accessing token generation feature of Service for Table Storage Account.    

**providerId**: 'STORAGE.SAS.01'(must not change this value).  
**accountName**:Azure storage service account Name where desired table is available.  
**name**: Storage Table name whose access is to be given.  

### Request Parameters  For Blob  Storage Account
User must provide below details when accessing token generation feature of Service for Blob  Storage Account.  

**providerId**: 'STORAGE.SAS.01'(must not change this value).  
**accountName**:Azure storage service account Name where desired Blob is available.  
**name**: file name stored in blob whose access is to be given.  

### Request Parameters  For IOT Hub Account
User must provide below details when accessing token generation feature of Service for Blob  Storage Account.  

**providerId**: 'IoTHub.SAS.01'(must not change this value).  
**deviceName**:IOT device name whose access need to be shared.


