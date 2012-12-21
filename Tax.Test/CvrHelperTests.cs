using Xunit;
using Xunit.Extensions;

namespace Tax.Test
{
	public class CvrHelperTests
	{
		[Theory]
		[InlineData(2960427, 29604274)]
		[InlineData(1573124, 15731249)]
		public void Test(int serial, int expected)
		{
			var result = CvrHelper.ToCvr(serial);
			Assert.Equal(expected, result);
		}
	}
}
