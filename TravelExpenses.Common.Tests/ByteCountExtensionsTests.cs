using Shouldly;
using System;
using Xunit;

namespace TravelExpenses.Common.Tests
{
    public class ByteCountExtensionsTests
    {
        [Theory]
        [InlineData(512, "512 Bytes")]
        [InlineData(1023, "1,023 Bytes")]
        [InlineData(1024, "1 KB")]
        [InlineData(512000, "500 KB")]
        [InlineData(1048576, "1 MB")]
        [InlineData(1572864, "1.5 MB")]
        [InlineData(1772864, "1.69 MB")]
        [InlineData(10485760, "10 MB")]
        [InlineData(1073741824, "1 GB")]
        [InlineData(1099511627776L, "1 TB")]
        [InlineData(1499511627776L, "1.36 TB")]
        public void GetByteCountStringTests(long bytes, string result)
        {
            var byteCountString = bytes.GetByteCountString();

            byteCountString.ShouldBe(result);
        }
    }
}
