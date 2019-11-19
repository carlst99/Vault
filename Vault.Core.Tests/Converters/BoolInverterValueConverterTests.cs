using Vault.Core.Converters;
using Xunit;

namespace Vault.Core.Tests.Converters
{
    public class BoolInverterValueConverterTests
    {
        [Fact]
        public void TestConvert()
        {
            BoolInverterValueConverter converter = new BoolInverterValueConverter();
            Assert.False((bool)converter.Convert(true, typeof(bool), null, null));
        }

        [Fact]
        public void TestConvertBack()
        {
            BoolInverterValueConverter converter = new BoolInverterValueConverter();
            Assert.True((bool)converter.ConvertBack(false, typeof(bool), null, null));
        }
    }
}
