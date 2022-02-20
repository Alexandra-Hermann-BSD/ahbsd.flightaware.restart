using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace ahbsd.network.check
{
    /// <summary>
    /// Interface for checking an IPAddress
    /// </summary>
    public interface ICheckIp
    {
        /// <summary>
        /// Gets the IP-Address.
        /// </summary>
        /// <value>The IP-Address</value>
        IList<IPAddress> IpAddresses { get; }

        /// <summary>
        /// Checks if the given IP is available.
        /// </summary>
        /// <param name="timeout">[optional] the wished timeout in ms, by default 100ms are selected</param>
        /// <returns><c>true</c> if the given IP is reachable, otherwise <c>false</c></returns>
        bool TestPing(int timeout=100);
        
        /// <summary>
        /// Gets the Aliases, if available.
        /// </summary>
        /// <value>The Aliases</value>
        [ReadOnly(true)]
        IReadOnlyList<string> Aliases { get; }
        
        /// <summary>
        /// Gets the minimum round trip time.
        /// </summary>
        /// <value>The minimum round trip time</value>
        long MinRoundTripTime { get; }
        
        /// <summary>
        /// Gets the maximum round trip time.
        /// </summary>
        /// <value>The maximum round trip time</value>
        long MaxRoundTripTime { get; }
        
        /// <summary>
        /// Gets the average round trip time.
        /// </summary>
        /// <value>The average round trip time</value>
        double AverageRoundTripTime { get; }
    }
}