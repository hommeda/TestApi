using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Core
{
    public static class PcData
    {
        public static string GetIpAddress(HttpContext httpContext)
        {
            IPAddress remoteIpAddress = httpContext.Request.HttpContext.Connection.RemoteIpAddress;
            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                        .First(x => x.AddressFamily == AddressFamily.InterNetwork);
                }
                return remoteIpAddress.ToString();
            }
            return string.Empty;
        }

        public static string GetPcName(HttpContext httpContext)
        {
           
            try
            {
                string HostName = Dns.GetHostEntry(httpContext.Connection.RemoteIpAddress)?.HostName;
                if (HostName == string.Empty || HostName == null)
                {
                    return "Devolep";
                }
                else
                {
                    return HostName;
                }
            }
            catch (Exception)
            {

                return "Devolep";
            }

        }
    }
}
