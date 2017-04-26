using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class TestHarness
    {
        public TestHarness()
        {
        }

        [Fact]
        public void ShoulNotFail()
        {
            Assert.True(true);
        }
    }
}
