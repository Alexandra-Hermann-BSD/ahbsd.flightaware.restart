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
        /// <param name="address">The given IP-Address or hostname</param>
        public CheckIp(string address)
        {
            ping = new Ping();
            IpAddresses = new List<IPAddress>();
            IPAddress ip = IPAddress.TryParse(address, out IPAddress tmpIp) ? tmpIp : null;

            if (ip != null)
            {
                IpAddresses.Add(ip);
            }
            else if (!string.IsNullOrEmpty(address))
            {
                SetIpAddress(address);
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
                Console.WriteLine($"[DoResolve] Exception: {e.Message}");
            }
            
        }
        
        #region implementation of ICheckIp
        /// <summary>
        /// Gets the IP-Address.
        /// </summary>
        /// <value>The IP-Address</value>
        public IList<IPAddress> IpAddresses { get; private set; }

        /// <summary>
        /// Checks if the given IP is reachable.
        /// </summary>
        /// <param name="timeout">[optional] the wished timeout in ms, by default 100ms are selected</param>
        /// <returns><c>true</c> if the given IP is reachable, otherwise <c>false</c></returns>
        public bool TestPing(int timeout = 100)
        {
            IList<PingReply> results = new List<PingReply>();

            // ReSharper disable once InvertIf
            if (IpAddresses?.Count > 0)
            {
                foreach (IPAddress address in IpAddresses)
                {
                    results.Add(ping.Send(address, timeout));
                }
            }

            return results.Any(reply => reply.Status == IPStatus.Success);
        }
        #endregion
    }
}