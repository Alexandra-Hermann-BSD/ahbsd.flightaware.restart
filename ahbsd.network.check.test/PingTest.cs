﻿using Xunit;

namespace ahbsd.network.check.test
{
    /// <summary>
    /// Testing <see cref="ICheckIp"/> and <see cref="CheckIp"/>
    /// </summary>
    public class PingTest
    {
        /// <summary>
        /// Testing with IP- and Host-Addresses
        /// </summary>
        /// <param name="address">The IP- or Host-Address</param>
        /// <param name="expectedConnect">The expected connectability</param>
        [Theory]
        [InlineData("127.0.0.1", true)]
        [InlineData("www.google.de", true)]
        [InlineData("123.456.789.300", false)]
        public void PingIp(string address, bool expectedConnect)
        {
            ICheckIp checkIp = new CheckIp(address);
            Assert.Equal(expectedConnect, checkIp.TestPing());
        }
    }
}