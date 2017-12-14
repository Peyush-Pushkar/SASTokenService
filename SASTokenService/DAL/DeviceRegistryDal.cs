using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SasTokenService.Entities;

namespace SasTokenService.DAL
{
    /// <summary>
    /// Class to delete , Create , and list all devices. 
    /// </summary>
    public class DeviceRegistryDal : IDeviceRegistryDal
    {
        PersistenceProvider<Device> m_Persistence;
        /// <summary>
        /// Constructor
        /// </summary>
        public DeviceRegistryDal()
        {
            m_Persistence = new PersistenceProvider<Device>();
        }
        /// <summary>
        /// Method to Create a new Device in Table
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Device Create(Device device)
        {
           return m_Persistence.Create(device);
        }
        /// <summary>
        /// Method To delete The Device From Table
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool DeleteDevice(long deviceId)
        {
            var device = m_Persistence.GetItems().FirstOrDefault(d => d.DeviceID == deviceId);
            return m_Persistence.Delete(device);
        }
        /// <summary>
        /// Method to Get All Devices from Database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Device> GetDevices()
        {
            return m_Persistence.GetItems().ToList();
        }
        /// <summary>
        /// Method to list All Devices
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<Device> QueryDevices(DeviceQuery query)
        {
            var devices = m_Persistence.GetItems();

            if (query == null)
                return devices;

            if (query.ID.HasValue)
                devices= devices.Where(d => d.DeviceID == query.ID);
            if (!String.IsNullOrEmpty(query.IP))
                devices = devices.Where(d => d.IP == query.IP);
            if (!String.IsNullOrEmpty(query.Name))
                devices = devices.Where(d => d.Name == query.Name);
            if (!String.IsNullOrEmpty(query.Uri))
                devices = devices.Where(d => d.Uri == query.Uri);
            if (!String.IsNullOrEmpty(query.DeviceKey))
                devices = devices.Where(d => d.DeviceKey == query.DeviceKey);

            return devices.Take(query.MaxResults).ToList();
        }
        /// <summary>
        /// Method To update the Context.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Device UpdateDevice(Device device)
        {
            return m_Persistence.Update(device);
        }

        public string Id
        {
            get { return "DAL.USEDB"; }
        }

        public SasProviderConfiguration Configuration { get; set; }
    }
}