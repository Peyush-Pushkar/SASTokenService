using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SasTokenService.DAL
{
    
    /// <summary>
    /// Extends EF context for extra functonality required bu creating of new device.
    /// </summary>
    public partial class SasDeviceContext : DbContext
    {
        /// <summary>
        /// Inserts the device in registry database and returns the identifier.
        /// </summary>
        /// <param name="deviceKey"></param>
        /// <param name="keyValidity"></param>
        /// <param name="deviceName"></param>
        /// <param name="ip"></param>
        /// <param name="owner"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public int InsertNewDevice(string deviceKey, Nullable<System.DateTime> keyValidity, string deviceName, string ip, string owner, string location)
        {
            SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["sasDb"].ConnectionString);

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Validate", SqlConn);
            cmd.CommandType = CommandType.StoredProcedure;

            string returnValue = string.Empty;

            try
            {
                SqlConn.Open();

                SqlParameter prmKey = new SqlParameter("@DeviceKey", SqlDbType.VarChar);
                prmKey.Direction = ParameterDirection.Input;
                prmKey.Value = deviceKey;
                cmd.Parameters.Add(prmKey);

                SqlParameter prmDevName = new SqlParameter("@DeviceName", SqlDbType.VarChar);
                prmDevName.Direction = ParameterDirection.Input;
                prmDevName.Value = deviceName;
                cmd.Parameters.Add(prmDevName);

                SqlParameter prmOwner = new SqlParameter("@Owner", SqlDbType.VarChar);
                prmOwner.Direction = ParameterDirection.Input;
                prmOwner.Value = owner;
                cmd.Parameters.Add(prmOwner);

                SqlParameter prmIp = new SqlParameter("@Ip", SqlDbType.VarChar);
                prmIp.Direction = ParameterDirection.Input;
                prmIp.Value = ip;
                cmd.Parameters.Add(prmIp);

                SqlParameter prmLoc = new SqlParameter("@Location", SqlDbType.VarChar);
                prmLoc.Direction = ParameterDirection.Input;
                prmLoc.Value = owner;
                cmd.Parameters.Add(prmLoc);

                SqlParameter prmValid = new SqlParameter("@KeyValidity", SqlDbType.DateTime);
                prmValid.Direction = ParameterDirection.Input;
                prmValid.Value = keyValidity;
                cmd.Parameters.Add(prmValid);

                SqlParameter retval = cmd.Parameters.Add("@ID", SqlDbType.Int);
                retval.Direction = ParameterDirection.ReturnValue;

                cmd.CommandText = @"USP_SasDevice_Insert";

                var res = cmd.ExecuteNonQuery();

                int id = (int)cmd.Parameters["@ID"].Value;

                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}