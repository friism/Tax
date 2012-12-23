using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax
{
	public static class EnumerableExtensions
	{
		private static readonly Random random = new Random();

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			// Durstenfeld implementation of the Fisher-Yates algorithm for an O(n) unbiased shuffle
			var array = source.ToArray();
			var n = array.Length;
			while (n > 1)
			{
				var k = random.Next(n);
				n--;
				var temp = array[n];
				array[n] = array[k];
				array[k] = temp;
			}

			return array;
		}
	}
}
