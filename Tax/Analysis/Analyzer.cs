using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Tax.Persistence;

namespace Tax.Analysis
{
	public class Analyzer
	{
		public void Analyze()
		{
			using (var context = new Context())
			{
				Console.WriteLine("TAXES");
				PrintDigitDistribution(context.Companies.Where(x => x.TaxPaid.HasValue && x.TaxPaid != 0).Select(x => x.TaxPaid.Value));
				Console.WriteLine("LOSSES");
				PrintDigitDistribution(context.Companies.Where(x => x.Losses.HasValue && x.Losses != 0).Select(x => x.Losses.Value));
				Console.WriteLine("REVENUE");
				PrintDigitDistribution(context.Companies.Where(x => x.Revenue.HasValue && x.Revenue != 0).Select(x => x.Revenue.Value));
			}

			Console.WriteLine("Press the any key...");
			Console.ReadKey();
		}

		public void PrintDigitDistribution(IEnumerable<decimal> numbers)
		{
			ConcurrentDictionary<int, int> result = new ConcurrentDictionary<int, int>();

			numbers.AsParallel().ForAll(number =>
			{
				var digit = Convert.ToInt32(Math.Abs(number).ToString().First().ToString());
				result.AddOrUpdate(digit, 1, (key, value) => ++value);
			});

			var total = (decimal)result.Values.Sum();
			foreach (var digit in result.Keys.OrderBy(x => x))
			{
				Console.WriteLine("{0}: {1}%", digit, (((decimal)result[digit])/total) * 100m);
			}
		}
	}
}
