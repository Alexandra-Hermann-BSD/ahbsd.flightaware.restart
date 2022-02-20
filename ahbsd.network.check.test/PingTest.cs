using System.ComponentModel;
using Xunit;

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
        /// <param name="expectedConnect">The expected connect ability</param>
        [Theory]
        [InlineData("127.0.0.1", true)]
        [InlineData("www.google.de", true)]
        [InlineData("123.456.789.300", false)]
        public void PingIp(string address, bool expectedConnect)
        {
            ICheckIp checkIp = new CheckIp(address);
            Assert.Equal(expectedConnect, checkIp.TestPing());
        }
        
        
        /// <summary>
        /// Test with various variables. Especially with <see cref="CheckIp"/>.
        /// </summary>
        /// <param name="address">Address to test</param>
        /// <param name="timeout">Timeout in ms</param>
        /// <param name="expectedConnect">Is it expected, that it work within the given timeout or the address really exists?</param>
        [Theory]
        [InlineData("127.0.0.1", 15, true)]
        [InlineData("localhost", 15, true)]
        [InlineData("www.google.de", 100, true)]
        [InlineData("www.google.de", 1, false)] // this should be nearly impossible...
        [InlineData("123.456.789.300", 600, false)]
        public void PingIpWithTimeout(string address, int timeout, bool expectedConnect)
        {
            ICheckIp checkIp = new CheckIp(address);
            Assert.Equal(expectedConnect, checkIp.TestPing(timeout));
            if (expectedConnect)
                Assert.True(checkIp.MaxRoundTripTime <= timeout);
        }
        
        /// <summary>
        /// Test with various variables. Especially for <see cref="CheckIpComponent"/>.
        /// </summary>
        /// <param name="address">Address to test</param>
        /// <param name="timeout">Timeout in ms</param>
        /// <param name="expectedConnect">Is it expected, that it work within the given timeout or the address really exists?</param>
        [Theory]
        [InlineData("127.0.0.1", 15, true)]
        [InlineData("localhost", 15, true)]
        [InlineData("www.google.de", 300, true)]
        [InlineData("www.google.de", 1, false)] // this should be nearly impossible...
        [InlineData("123.456.789.300", 600, false)]
        public void PingIpWithTimeoutAndContainer(string address, int timeout, bool expectedConnect)
        {
            IContainer container = new Container();
            CheckIpComponent checkIp1 = new CheckIpComponent(container, address);
            ICheckIp checkIp2 = new CheckIpComponent(address);
            Assert.Equal(expectedConnect, checkIp1.TestPing(timeout));
            Assert.Equal(expectedConnect, checkIp2.TestPing(timeout));
            Assert.True(container.Components[0].Equals(checkIp1));
            if (expectedConnect)
            {
                Assert.True(checkIp1.IpAddresses.Count > 0);
                Assert.True(checkIp1.MaxRoundTripTime <= timeout);
            }
        }
    }
}
