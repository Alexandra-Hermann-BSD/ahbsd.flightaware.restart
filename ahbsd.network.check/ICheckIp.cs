using System.Collections.Generic;
using System.Net;

namespace ahbsd.network.check
{
    /// <summary>
    /// Interface for checking an IPAddress
    /// </summary>
    public interface ICheckIp
    {
        /// <summary>
        /// Gets the IP-Adress.
        /// </summary>
        /// <value>The IP-Adress</value>
        IList<IPAddress> IpAddresses { get; }

        /// <summary>
        /// Checks if the given IP is available.
        /// </summary>
        /// <returns><c>true</c> if the given IP is reachable, otherwise <c>false</c></returns>
        bool TestPing();
    }
}