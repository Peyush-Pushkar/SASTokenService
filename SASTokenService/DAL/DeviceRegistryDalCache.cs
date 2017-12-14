using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SasTokenService.Entities;

namespace SasTokenService.DAL
{
    /// <summary>
    /// Class to Implement the cache mechanism . This class stores device details in a static list
    /// </summary>
   
    public class DeviceRegistryDalCache : IDeviceRegistryDal
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DeviceRegistryDalCache()
        {
           LoadCachedData();

        }
        /// <summary>
        /// Static List of Device to hold pre defined values.
        /// </summary>
       public static List<Device> deviceList = new List<Device>();
        /// <summary>
        /// Method to add a new Device to List
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Device Create(Device device)
        {

            device.DeviceID = deviceList.Count()+1;
            deviceList.Add(device);
            return device;


        }
        /// <summary>
        /// Method to remove a device details from List.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public bool DeleteDevice(long deviceId)
        {
            Device deviceToBeDelted = new Device();
            foreach (Device device in deviceList)
            {
                if (device.DeviceID == deviceId)
                {
                    deviceToBeDelted = device;
                    
                }
            }
            deviceList.Remove(deviceToBeDelted);
            return true;
        }


        /// <summary>
        /// Method To get List of Devices.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Device> GetDevices()
        {
            return deviceList;
        }

        /// <summary>
        /// Method To return list of devices based on Query Details.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<Device> QueryDevices(DeviceQuery query)
        {
            // required to store in local list to avoid any where condition on static list .
            var myLocalList = new List<Device>(deviceList);

            if (query.ID.HasValue)
                myLocalList = deviceList.Where(d => d.DeviceID == query.ID).ToList();
            if (!String.IsNullOrEmpty(query.IP))
                myLocalList = deviceList.Where(d => d.IP == query.IP).ToList();
            if (!String.IsNullOrEmpty(query.Name))
                myLocalList = deviceList.Where(d => d.Name == query.Name).ToList();
            if (!String.IsNullOrEmpty(query.Uri))
                myLocalList = deviceList.Where(d => d.Uri == query.Uri).ToList();
            if (!String.IsNullOrEmpty(query.DeviceKey))
                myLocalList = deviceList.Where(d => d.DeviceKey == query.DeviceKey).ToList();

            return myLocalList.Take(query.MaxResults).ToList();
        }

        /// <summary>
        /// Method to update a existing record from the List.
        /// </summary>
        /// <param name="updatedDevice"></param>
        /// <returns></returns>
        public Device UpdateDevice(Device updatedDevice)
        {
            Device modifiedDevice = new Device();
            foreach (Device device in deviceList)
            {
                if (device.DeviceID==updatedDevice.DeviceID)
                {
                    modifiedDevice = device;
                    device.DeviceID = updatedDevice.DeviceID;
                    device.Name = updatedDevice.Name;
                    device.DeviceKey = updatedDevice.DeviceKey;
                    device.KeyValidity = updatedDevice.KeyValidity;
                    device.Owner = updatedDevice.Owner;
                    device.IP = updatedDevice.IP;
                    device.GeoPosition = updatedDevice.GeoPosition;
                    device.ChangedAt = Convert.ToDateTime(DateTime.Now);
                }

            }
            return modifiedDevice;
        }

        /// <summary>
        /// ID of Class
        /// </summary>
        public string Id
        {
            get { return "DAL.CACHE"; }
        }

    
       private static List<Device>  LoadCachedData()
        {
           // List<Device> deviceList = new List<Device>();

            string dateTime = "01/01/2020 14:50:50.42";
            string dateTimeCreated = "01/01/2017 14:50:50.42";
            Device dev01 = new Device();
            dev01.DeviceID =01;
            dev01.Name = "Device01";
            dev01.DeviceKey = "zgmq9745ZG";
            dev01.KeyValidity = Convert.ToDateTime(dateTime);
            dev01.Owner = "FRA UAS";
            dev01.IP = "192.168.98.109";
            dev01.GeoPosition = "FrankFurt";
            dev01.CreatedAt= Convert.ToDateTime(dateTimeCreated);
            dev01.ChangedAt= Convert.ToDateTime(dateTimeCreated);

            Device dev02 = new Device();
            dev02.DeviceID = 02;
            dev02.Name = "Device02";
            dev02.DeviceKey = "zumq5645ZG";
            dev02.KeyValidity = Convert.ToDateTime(dateTime);
            dev02.Owner = "FRA UAS";
            dev02.IP = "192.168.98.110";
            dev02.GeoPosition = "Berlin";
            dev02.CreatedAt = Convert.ToDateTime(dateTimeCreated);
            dev02.ChangedAt = Convert.ToDateTime(dateTimeCreated);

            Device dev03 = new Device();
            dev03.DeviceID = 03;
            dev03.Name = "Device03";
            dev03.DeviceKey = "yymq5845ZG";
            dev03.KeyValidity = Convert.ToDateTime(dateTime);
            dev03.Owner = "FRA UAS";
            dev03.IP = "192.168.68.112";
            dev03.GeoPosition = "Delhi";
            dev03.CreatedAt = Convert.ToDateTime(dateTimeCreated);
            dev03.ChangedAt = Convert.ToDateTime(dateTimeCreated);

            Device dev04 = new Device();
            dev04.DeviceID = 04;
            dev04.Name = "Device04";
            dev04.DeviceKey = "uymq5895UG";
            dev04.KeyValidity = Convert.ToDateTime(dateTime);
            dev04.Owner = "FRA UAS";
            dev04.IP = "192.168.68.119";
            dev04.GeoPosition = "Mumbai";
            dev04.CreatedAt = Convert.ToDateTime(dateTimeCreated);
            dev04.ChangedAt = Convert.ToDateTime(dateTimeCreated);

            deviceList.Add(dev01);
            deviceList.Add(dev02);
            deviceList.Add(dev03);
            deviceList.Add(dev04);
            return deviceList;



        }
        /// <summary>
        /// Getter and Setter for Configuration values.
        /// </summary>
        public SasProviderConfiguration Configuration { get; set; }

    }
}