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
        
        
        [Theory]
        [InlineData("127.0.0.1", 5, true)]
        [InlineData("localhost", 8, true)]
        [InlineData("www.google.de", 100, true)]
        [InlineData("www.google.de", 1, false)] // this should be nearly impossible...
        [InlineData("123.456.789.300", 600, false)]
        public void PingIpWithTimeout(string address, int timeout, bool expectedConnect)
        {
            ICheckIp checkIp = new CheckIp(address);
            Assert.Equal(expectedConnect, checkIp.TestPing(timeout));
        }
        
        [Theory]
        [InlineData("127.0.0.1", 5, true)]
        [InlineData("localhost", 8, true)]
        [InlineData("www.google.de", 100, true)]
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
        }
    }
}
