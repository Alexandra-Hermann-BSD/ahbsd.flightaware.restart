using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using ahbsd.lib.TLDCheck;

namespace ahbsd.network.check
{
    /// <summary>
    /// Class to check whether a given address is reachable.
    /// </summary>
    public class CheckIp : ICheckIp
    {
        /// <summary>
        /// Static list of TLD's from IANA.
        /// </summary>
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private static IIANA_TLD tld;

        /// <summary>
        /// Static constructor to enable the TLD-List <see cref="tld"/>.
        /// </summary>
        static CheckIp() => tld = new IANA_TLD(waitToReload: new TimeSpan(hours: 3, minutes: 0, seconds: 0));

        /// <summary>
        /// The ping object
        /// </summary>
        private readonly Ping ping;

        /// <summary>
        /// The HostEntry, if available.
        /// </summary>
        private IPHostEntry hostEntry;

        /// <summary>
        /// Protected default constructor.
        /// </summary>
        /// <remarks>Needed for all other constructors.</remarks>
        protected CheckIp()
        {
            ping = new Ping();
            IpAddresses = new List<IPAddress>();

            ping.PingCompleted += Ping_PingCompleted;
        }

        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            //
        }

        /// <summary>
        /// Constructor with a given IP-Address or hostname.
        /// </summary>
        /// <param name="address">The given IP-Address or hostname</param>
        public CheckIp(string address)
            : this()
        {
            IPAddress ip = IPAddress.TryParse(address, out IPAddress tmpIp) ? tmpIp : null;

            if (ip != null)
            {
                SetIpAddress(ip);
            }
            else if (!string.IsNullOrEmpty(address))
            {
                IList<string> parts = address.Split('.').ToList();
                if (tld.CheckTLD(parts.Last()) || address.ToLower().Equals("localhost"))
                {
                    SetIpAddress(address);
                }
            }
            
        }
        
        /// <summary>
        /// Constructor with a given IP-Address or hostname.
        /// </summary>
        /// <param name="address">The given IP-Address or hostname</param>
        /// <exception cref="ArgumentNullException">If the given IP-Address is <c>null</c></exception>
        public CheckIp(IPAddress address)
            : this()
        {

            if (address != null)
            {
                SetIpAddress(address);
            }
            else 
            {
                throw new ArgumentNullException($"The given IPAddress is unfortunatly null ('{address}')");
            }
            
        }

        /// <summary>
        /// Try's to get all IP-Addresses of a host.
        /// </summary>
        /// <param name="host">The given host</param>
        private void SetIpAddress(string host)
        {
            hostEntry = Dns.GetHostEntry(host);
            
            IpAddresses = hostEntry.AddressList.ToList();
        }

        /// <summary>
        /// Adds the given IP-Address to the list of Addresses.
        /// </summary>
        /// <param name="address">The given IP-Address</param>
        private void SetIpAddress(IPAddress address)
        {
            try
            {
                hostEntry = Dns.GetHostEntry(address);
            }
            catch (Exception e)
            {
                Console.WriteLine($"A {e.GetType().Name} happened: {e.Message}");
            }

            if (!IpAddresses.Contains(address))
            {
                IpAddresses.Add(address);
            }
        }
        
        #region implementation of ICheckIp
        /// <summary>
        /// Gets the IP-Address.
        /// </summary>
        /// <value>The IP-Address</value>
        public IList<IPAddress> IpAddresses { get; private set; }

        /// <summary>
        /// Gets the Aliases, if available.
        /// </summary>
        /// <value>The Aliases</value>
        [ReadOnly(true)]
        public IReadOnlyList<string> Aliases
        {
            get
            {
                IReadOnlyList<string> result = null;

                if (hostEntry != null && hostEntry.Aliases.Length > 0)
                {
                    result = hostEntry.Aliases.ToList();
                }
                else if (hostEntry != null && hostEntry.Aliases.Length == 0)
                {
                    result = new List<string>();
                }
                
                return result;
            }
        }

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