using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using SasTokenService.Entities;
using SasTokenService.Model;
using System;

namespace SasTokenService
{
    /// <summary>
    /// Class To Obtain Storage Token
    /// </summary>

    public class StorageSasTokenProvider : ISasTokenProvider
    {
        /// <summary>
        /// Constant String Blob
        /// </summary>
        public const string BLOB = "BLOB";
        /// <summary>
        /// Constant String Table
        /// </summary>
        public const string TABLE = "TABLE";
        /// <summary>
        /// Constant String Queue
        /// </summary>
        public const string QUEUE = "QUEUE";
        /// <summary>
        /// Constant String For RequestValidationError
        /// </summary>
        public const string RequestValidationError = "Request is missing key values to obtain the Requested Token";
        /// <summary>
        /// Constant String For HTTPS
        /// </summary>
        public const string HTTPS = "http://";
        /// <summary>
        /// Constant String For Newline
        /// </summary>
        public const string Newline = "\n";
     


        /// <summary>
        /// Method To Get Id and Destription
        /// </summary>
        public SasProviderConfiguration Configuration { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return "STORAGE.SAS.01"; }
        }

        /// <summary>
        /// Method to return Description value
        /// </summary>
        public string Description
        {
            get { return "Generates SAS tokens for Microsoft Azure Service Bus."; }
        }



        /// <summary>
        /// This method is to identify which SAS token to  be retieved based on the user input.
        /// </summary>
        /// <param name="tokenArgs"></param>
        /// <param name="configuration"></param>
        /// <returns>token of Type Token</returns>
        public Token CreateToken(CreateTokenRequest tokenArgs, SasProviderConfiguration configuration)
        {
            StorageTokenArgs args = getArgs(tokenArgs);
             var token = new Token();

            if (tokenArgs.TokenIdentifier.ToLower() == BLOB.ToLower())
            {
                args.ConnectionString = configuration.Values["ConnectionStringBloB"];
                token = getkeyFromAzureForBlob(args);
               
            }

            else if (tokenArgs.TokenIdentifier.ToLower() == TABLE.ToLower())
            {
                args.Accountkey = configuration.Values["AccountKey_TableStorage"];
                token = getSASForTableStorage(args);
            }

            else if (tokenArgs.TokenIdentifier.ToLower() == QUEUE.ToLower())
            {
                args.Accountkey = configuration.Values["AccountKey_QueueStorage"];
                token = getSASForQueueStorage(args);
            }
            else
                throw new SasTokenServiceException("Unknown token identifier found in request.");
            
            return token;
        }

        #region Private Methods

        /// <summary>
        /// Creates ServiceBusTokenArgs from dynamic type.
        /// </summary>
        /// <param name="tokenvalue"></param>
        /// <returns>tokenArgs of Type ServiceBusTokenArgs</returns>
        private StorageTokenArgs getArgs(CreateTokenRequest tokenvalue)
        {
            var tokenArgs = new StorageTokenArgs();


            if (tokenvalue.TokenIdentifier != null)
            {
                String TokenIdentifier = tokenvalue.TokenIdentifier;
             
                if (tokenvalue.TokenIdentifier == BLOB)
                {
                    createArgsForBlob(tokenvalue, tokenArgs);

                }

                if (tokenvalue.TokenIdentifier == TABLE)
                {
                    createArgsForTable(tokenvalue, tokenArgs);

                }

                if (tokenvalue.TokenIdentifier == QUEUE)
                {

                    createArgsForQueue(tokenvalue, tokenArgs);
                }
            }


            return tokenArgs;
        }

        /// <summary>
        /// Creates ServiceBusTokenArgs object  for Blob
        /// </summary>
        /// <param name="tokenvalue"></param>
        /// <param name="tokenArgs"></param>
        private static void createArgsForBlob(CreateTokenRequest tokenvalue, StorageTokenArgs tokenArgs)
        {
            if (tokenvalue.AccountName != null)
            {
                tokenArgs.AccountName = tokenvalue.AccountName;
            }
            else throw new SasTokenServiceException(RequestValidationError);
            if (tokenvalue.Name != null)
            {
                tokenArgs.Name = tokenvalue.Name;
            }
            else throw new SasTokenServiceException(RequestValidationError);

        }

        /// <summary>
        /// Creates ServiceBusTokenArgs object  for Table
        /// </summary>
        /// <param name="tokenvalue"></param>
        /// <param name="tokenArgs"></param>
        private static void createArgsForTable(CreateTokenRequest tokenvalue, StorageTokenArgs tokenArgs)
        {
            if (tokenvalue.AccountName != null)
            {
                tokenArgs.AccountName = tokenvalue.AccountName;
            }
            else throw new SasTokenServiceException(RequestValidationError);
            if (tokenvalue.Name != null)
            {
                tokenArgs.Name = tokenvalue.Name;
            }
            else throw new SasTokenServiceException(RequestValidationError);

        }

        /// <summary>
        /// Creates ServiceBusTokenArgs object  for Queue
        /// </summary>
        /// <param name="tokenvalue"></param>
        /// <param name="tokenArgs"></param>
        private static void createArgsForQueue(CreateTokenRequest tokenvalue, StorageTokenArgs tokenArgs)
        {
            if (tokenvalue.AccountName != null)
            {
                tokenArgs.AccountName = tokenvalue.AccountName;
            }
            else throw new SasTokenServiceException(RequestValidationError);
            if (tokenvalue.Name != null)
            {
                tokenArgs.Name = tokenvalue.Name;
            }
            else throw new SasTokenServiceException(RequestValidationError);

        }

        #endregion

        // get key for blob
        /// <summary>
        /// Method to Create token for Azure blob
        /// </summary>
        /// 
        /// <param name="request"></param>
        /// <returns>Raw Token of Type Token</returns>
        public Token getkeyFromAzureForBlob(StorageTokenArgs request)
        {

            string StorageConnectionString = request.ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(request.AccountName);
            var rawToken = getBlobSasUri(container, request.Name);
            return new Token()
            {

                RawToken = rawToken
            };


        }
        /// <summary>
        /// Method to get SAS URI for Blob
        /// </summary>
        /// <param name="container"></param>
        /// <param name="filename"></param>
        /// <returns> Blob uri and Shared Access Signature</returns>

        static string getBlobSasUri(CloudBlobContainer container, string filename)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);
            return blob.Uri + blob.GetSharedAccessSignature(createAzureBlobPolicy());
        }


        /// <summary>
        /// Method to get SAS token for Azure Table
        /// </summary>
        /// <param name="args"></param>
        /// <returns>RawToken of Type Token</returns>
        static Token getSASForTableStorage(StorageTokenArgs args)
        {


            StorageCredentials creds = new StorageCredentials(args.AccountName, args.Accountkey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(args.Name);
            return new Token()
            {

                RawToken = table.Uri.ToString() + table.GetSharedAccessSignature(createAzureTablePolicy())
            };
        }
        /// <summary>
        /// Method to get SAS token for Azure Queue
        /// </summary>
        /// <param name="args"></param>
        /// <returns>RawToken of Type Token</returns>
        static Token getSASForQueueStorage(StorageTokenArgs args)
        {

            StorageCredentials creds = new StorageCredentials(args.AccountName, args.Accountkey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudQueueClient client = account.CreateCloudQueueClient();
            CloudQueue queue = client.GetQueueReference(args.Name);

            return new Token()
            {

                RawToken = queue.Uri.ToString() + queue.GetSharedAccessSignature(createAzureQueuePolicy())
            };
        }



        /// <summary>
        /// Method to set SharedAccessQueuePolicy object with proper Access
        /// </summary>
        /// <returns>SharedAccessQueuePolicy</returns>
        private static SharedAccessQueuePolicy createAzureQueuePolicy()
        {

            SharedAccessQueuePolicy policy = new SharedAccessQueuePolicy();
            policy.SharedAccessExpiryTime = DateTime.UtcNow;
            policy.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(300);
            policy.Permissions = SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Read;
            return policy;
        }

        /// <summary>
        /// Method to set SharedAccessTablePolicy object with proper Access
        /// </summary>
        /// <returns>SharedAccessTablePolicy</returns>
        private static SharedAccessTablePolicy createAzureTablePolicy()
        {

            SharedAccessTablePolicy policy = new SharedAccessTablePolicy();
            policy.SharedAccessExpiryTime = DateTime.UtcNow;
            policy.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(300);
            policy.Permissions = SharedAccessTablePermissions.Add
            | SharedAccessTablePermissions.Query
            | SharedAccessTablePermissions.Update
            | SharedAccessTablePermissions.Delete;
            return policy;
        }
        /// <summary>
        /// Method to set SharedAccessBlobPolicy object with proper Access
        /// </summary>
        /// <returns>SharedAccessBlobPolicy</returns>
        private static SharedAccessBlobPolicy createAzureBlobPolicy()
        {

            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read |
                SharedAccessBlobPermissions.Write;
            return sasConstraints;
        }
    }
}