using System;
using System.Linq;

namespace Tax.DataLoad
{
	public static class CvrHelper
	{
		private static int[] digitWeights = { 2, 7, 6, 5, 4, 3, 2 };

		public static int ToCvr(int serial)
		{
			var digits = serial.ToString().Select(x => int.Parse(x.ToString()));
			var sum = digits.Select((x, y) => x * digitWeights[y]).Sum();
			var modulo = sum % 11;
			if (modulo == 1)
			{
				return -1;
			}
			if (modulo == 0)
			{
				modulo = 11;
			}
			var checkDigit = 11 - modulo;
			return serial * 10 + checkDigit;
		}
	}
}
