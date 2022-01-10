using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;

namespace ahbsd.network.check
{
    /// <summary>
    /// Class to check whether a given address is reachable.
    /// </summary>
    public class CheckIp : ICheckIp
    {
        /// <summary>
        /// The ping object
        /// </summary>
        private readonly Ping ping;
        
        /// <summary>
        /// Constructor with a given IP-Address or hostname.
        /// </summary>
        /// <param name="adress">The given IP-Address or hostname</param>
        public CheckIp(string adress)
        {
            ping = new Ping();
            IpAddresses = new List<IPAddress>();
            IPAddress ip = IPAddress.TryParse(adress, out IPAddress tmpIp) ? tmpIp : null;

            if (ip != null)
            {
                IpAddresses.Add(ip);
            }
            else if (!string.IsNullOrEmpty(adress))
            {
                SetIpAddress(adress);
            }
            
        }

        /// <summary>
        /// Try's to get all IP-Addresses of a host.
        /// </summary>
        /// <param name="host">The given host</param>
        private void SetIpAddress(string host)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(host);

                IpAddresses = hostEntry.AddressList.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[DoResolve] Exception: {e}");
            }
            
        }
        
        /// <summary>
        /// Gets the IP-Adress.
        /// </summary>
        /// <value>The IP-Adress</value>
        public IList<IPAddress> IpAddresses { get; private set; }

        /// <summary>
        /// Checks if the given IP is available.
        /// </summary>
        /// <returns><c>true</c> if the given IP is reachable, otherwise <c>false</c></returns>
        public bool TestPing()
        {
            IList<PingReply> results = new List<PingReply>();

            if (IpAddresses != null)
            {
                foreach (IPAddress address in IpAddresses)
                {
                    results.Add(ping.Send(address, 100));
                }
            }

            bool result = false;
            foreach (PingReply reply in results)
            {
                if (reply.Status == IPStatus.Success)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}