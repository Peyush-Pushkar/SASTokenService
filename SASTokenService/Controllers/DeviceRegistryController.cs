using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;
using System.IO;
using System.Web.Http.Cors;
using System.Data.Entity.Core.Objects;
using SasTokenService.Entities;
using SasTokenService.DAL;

namespace SasTokenService.Controllers
{
    /// <summary>
    /// Class to create , Find, Delete a device.
    /// </summary>
    [RoutePrefix("api/DeviceRegistry")]
    public class DeviceRegistryController : ApiController
    {
        private static IDeviceRegistryDal m_DeviceDal;
        private static List<IDeviceRegistryDal> m_DeviceDalList = new List<IDeviceRegistryDal>();

        private static List<SasProviderConfiguration> m_Configs;

        /// <summary>
        /// Constructor.
        /// </summary>
        static DeviceRegistryController()
        {
            loadConfigForDeviceRegistry();
            buildTokenPoviders();
        }

        private IDeviceRegistryDal loadDeviceRegistry()
        {
            IDeviceRegistryDal dal = null;
            var dalqName = System.Configuration.ConfigurationManager.AppSettings.Get("DeviceRegistryQN");

            if (String.IsNullOrEmpty(dalqName))
                throw new ArgumentException("DeviceRegistryQN not configured in AppSettings!");

            var tp = Type.GetType(dalqName);

            // null check before using the type
            if (tp != null)
            {
                dal = Activator.CreateInstance(tp) as IDeviceRegistryDal;
            }
            else
            {
                // TODO: throw error
            }



            return dal;
        }

        /// <summary>
        /// Load the differnet Device Details Providers. One interacts with DB another uses Cache
        /// </summary>
        private static void loadConfigForDeviceRegistry()
        {
            try
            {
                m_Configs = new List<SasProviderConfiguration>()
            {
                new SasProviderConfiguration()
                {
                    ProviderId = "DAL.USEDB",

                     ProviderQN = typeof(DeviceRegistryDal).AssemblyQualifiedName,

                },

                new SasProviderConfiguration()
                {
                    ProviderQN = typeof(DeviceRegistryDalCache).AssemblyQualifiedName,

                    ProviderId = "DAL.CACHE",
                }, 
            };

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while getting Account Details" + ex);


            }
        }

        private static void buildTokenPoviders()
        {
            foreach (var cfg in m_Configs)
            {
                var tp = Type.GetType(cfg.ProviderQN);

                // null check before using the type
                if (tp != null)
                {
                    IDeviceRegistryDal provider = Activator.CreateInstance(tp) as IDeviceRegistryDal;
                    provider.Configuration = cfg;
                    m_DeviceDalList.Add(provider);
                }
            }
        }
        private void traceError(string msg, Exception ex)
        {
            Debug.WriteLine($"{msg}:{ex}");
        }

        /// <summary>
        /// TO search for a string related to any device and fetch the devices
        /// </summary>
        /// <param name="query"></param>
        /// <returns>List of devices</returns>
        [HttpGet]
       [Route("")]
      
        public IEnumerable<Device> QueryDevices([FromUri]DeviceQuery query)
        {
            try
            {
                 var provider = m_DeviceDalList.FirstOrDefault(x => x.Id == query.ProviderID);
                    var deviceList = provider.QueryDevices(query);
                    return deviceList;
                
               
            }
            catch (Exception ex)
            {
                traceError("Error when getting list of devices.", ex);
                return null;
            }
        }



        /// <summary>
        /// To get the list of devices based device ID
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>List of devices based on deviceID</returns>
        [HttpGet]
        [Route("{deviceId}")]
        public Device GetDeviceById(int deviceId)
        {
            try
            {

                
                var device = m_DeviceDal.QueryDevices(new DeviceQuery { ID = deviceId }).FirstOrDefault();

                return device;
            }
            catch (Exception ex)
            {
                traceError($"Error when getting Device By ID {deviceId}", ex);
                return null;
            }
        }


        /// <summary>
        /// Registers a new device in device registry.
        /// </summary>
        /// <param name="newdevice">param of Type Device which Holds deatils of Device</param>

        [HttpPost]
        [Route("Register")]
        public long? RegisterNewDevice(DeviceQuery newdevice)
        {
            try
            {
                var dKG = new DeviceKeyGenerator();

                Tuple<string, DateTime> deviceKV = dKG.GetKey();

                String deviceKey = deviceKV.Item1;

                DateTime keyValidity = deviceKV.Item2;

                Debug.WriteLine("inserted keyvalidity: " + keyValidity);

                var device = new Device { DeviceKey = deviceKV.Item1, KeyValidity = deviceKV.Item2, GeoPosition = newdevice.GeoPosition, IP = newdevice.IP, Name = newdevice.Name, Owner = newdevice.Owner };
                var provider = m_DeviceDalList.FirstOrDefault(x => x.Id == newdevice.ProviderID);
                device = provider.Create(device);

                return device.DeviceID;
            }
            catch (Exception ex)
            {
                traceError("Error While Creating the New Device.", ex);
                return null;
            }
        }

        /// <summary>
        /// To update the device data in the database 
        /// </summary>
        /// <param name="deviceQuery"></param>
        /// <returns>Information related to updation of device data</returns>
        [HttpPut]
        [Route("")]
        public Device UpdateDevice(DeviceQuery deviceQuery)
        {
            bool dateDifference = true;
            try
            {
                Device device = new Device
                {
                    Name = deviceQuery.Name,
                    IP = deviceQuery.IP,
                    DeviceID = deviceQuery.ID.Value,
                    DeviceKey = deviceQuery.DeviceKey,
                    GeoPosition = deviceQuery.GeoPosition,
                    Owner = deviceQuery.Owner,
                    KeyValidity = deviceQuery.KeyValidity,
                    Uri = deviceQuery.Uri,


                };
                var provider = m_DeviceDalList.FirstOrDefault(x => x.Id == deviceQuery.ProviderID);
                if (dateDifference == true)
                {
                   
                    
                    Debug.WriteLine("updating key " + dateDifference);
                    var dKG = new DeviceKeyGenerator();
                    Tuple<string, DateTime> deviceKV = dKG.GetKey();
                    device.DeviceKey = deviceKV.Item1;
                    device.KeyValidity = deviceKV.Item2;

                    return provider.UpdateDevice(device);
                }
                else
                {
                    return provider.UpdateDevice(device);
                }
            }
            catch (Exception ex)
            {
                traceError("Error While Updating The Device.", ex);
                return null;
            }
        }

        /// <summary>
        /// To delete device from the database based on deviceId
        /// </summary>
        /// <param name="devicetoBeDeleted"></param>
        /// <returns>Information related to deletion of device</returns>
        [HttpPut]

        [Route("Delete")]
        public bool? DeleteDevice(DeviceQuery devicetoBeDeleted)
        {
            try
            {
                var provider = m_DeviceDalList.FirstOrDefault(x => x.Id == devicetoBeDeleted.ProviderID);
               
                var sucess= provider.DeleteDevice(devicetoBeDeleted.ID.Value);
                return sucess;
            }
            catch (Exception ex)
            {
                traceError("Error While Deleting The Device.", ex);
                return null;
            }
        }

    }
}