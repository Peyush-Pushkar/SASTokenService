using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using SasTokenService.DAL;

namespace SasTokenService
{
    /// <summary>
    /// Class To validate Users and its Key
    /// </summary>
    public class DeviceKeyGenerator
    {
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /// You can even combine the two methods - RandomNumber and RandomString to generate a combination of random string and /// numbers. For example, the following code generates a password of length 10 with first 4 letters lowercase, next 4 
        /// letters numbers, and last 2 letters as uppercase.

        public string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }


        /// <summary>
        /// Method To get key for a Device
        /// </summary>
        /// <returns></returns>
        public Tuple<string, DateTime> GetKey()
        {
            String DeviceKey = GetPassword();


            //DateTime validity = DateTime.Today.AddMonths(1);
            DateTime validity = DateTime.UtcNow.AddMonths(1);

            Debug.WriteLine("validity format:" + validity);

            return Tuple.Create(DeviceKey, validity);


        }

        /// <summary>
        /// It validates device based on its id and key.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="deviceKey"></param>
        /// <returns>TRUE if device is successfully validated.</returns>

        public static bool GetDeviceValidator(long deviceId, string deviceKey)
        {
            try
            {
                IDeviceRegistryDal dal = new DeviceRegistryDal();

                var devices = dal.QueryDevices(new Entities.DeviceQuery { ID = deviceId, DeviceKey = deviceKey });

                if (devices.Count() == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in GetDeviceValidator: " + ex);
                return true;
            }
        }

    }
}