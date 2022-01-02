using ahbsd.flightaware.piaware;
using Xunit;

namespace ahbsd.flightaware.piawareTest
{
    public class ModuleTest
    {
        [Theory]
        [InlineData("PiAware master process (piaware) is running with pid 25826.", PiAwareModule.piaware, true, 25826, "")]
        [InlineData("PiAware ADS-B client (faup1090) is running with pid 25876.", PiAwareModule.faup1090, true, 25876, "")]
        [InlineData("PiAware ADS-B UAT client (faup978) is not running (disabled by configuration settings)", PiAwareModule.faup978, false, -1, "disabled by configuration settings")]
        [InlineData("PiAware mlat client (fa-mlat-client) is running with pid 3737.", PiAwareModule.fa_mlat_client, true, 3737, "")]
        [InlineData("Local ADS-B receiver (dump1090-fa) is running with pid 3739.", PiAwareModule.dump1090_fa, true, 3739, "")]
        public void TestFromOutputString(string line, PiAwareModule expectedModule, bool expectedRunning, int expectedPid, string expectedNotice)
        {
            uint? expectedPidNullable;

            if (expectedPid <= 0)
            {
                expectedPidNullable = null;
            }
            else
            {
                expectedPidNullable = (uint?)expectedPid;
            }

            IModule module = new Module(line);
            Assert.Equal(expectedModule, module.ModuleType);
            Assert.Equal(expectedRunning, module.IsRunning);
            Assert.Equal(expectedPidNullable, module.Pid);
            Assert.Equal(expectedNotice, module.Notice);
        }
    }
}
