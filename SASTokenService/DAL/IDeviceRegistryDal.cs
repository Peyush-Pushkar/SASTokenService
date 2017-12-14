using SasTokenService.Entities;
using System.Collections.Generic;
using System.Web.Http;

namespace SasTokenService.DAL
{
    /// <summary>
    /// Defined Interface For acessing Data Access Layer
    /// </summary>
    public interface IDeviceRegistryDal
    {
    /// <summary>
    /// Method To Delete a Existing Device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns></returns>
        bool DeleteDevice(long deviceId);
        /// <summary>
        /// Method To Get existing Device.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<Device> QueryDevices(DeviceQuery query);
        /// <summary>
        /// Method To  Create a new Deive
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Device Create(Device device);

        /// <summary>
        /// Method To Update a Device 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Device UpdateDevice(Device device);
        /// <summary>
        /// Method To get All Devices
        /// </summary>
        /// <returns></returns>
        IEnumerable<Device> GetDevices();

        SasProviderConfiguration Configuration { get; set; }
        string Id { get; }
    }
}