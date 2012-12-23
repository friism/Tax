using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using Tax.Model;
using Tax.Persistence;

namespace Tax
{
	public class Program
	{
		static void Main(string[] args)
		{
			ServicePointManager.DefaultConnectionLimit = int.MaxValue;

			var parallelism = 10;
			var parallelismSetting = ConfigurationManager.AppSettings["parallelism"];
			int.TryParse(parallelismSetting, out parallelism);

			Console.WriteLine("Parellelism: {0}", parallelism);

			IEnumerable<int> serials;
			do
			{
				serials = GetSerials();
				serials.AsParallel().WithDegreeOfParallelism(parallelism).ForAll(x =>
				{
					try
					{
						var company = new Company()
						{
							Serial = x,
						};

						var cvrNumber = CvrHelper.ToCvr(x);
						if (cvrNumber != -1)
						{
							company.Cvr = cvrNumber;

							var contentHelper = new ContentHelper(Encoding.GetEncoding("ISO-8859-1"));
							var document = contentHelper.GetContent(cvrNumber);

							if (!document.DocumentNode.OuterHtml.Contains("Virksomhedsnavnet eller CVR/SE-nummeret findes ikke på skattelisterne for selskaber 2011"))
							{
								var extractor = new DataExtractor();
								extractor.Extract(company, document);
								Console.WriteLine("{0} Extracted {1} - {2}", company.Serial, company.Cvr, company.Name);
							}
							else
							{
								Console.WriteLine("{0} Disregarded {1}", company.Serial, company.Cvr);
							}
						}

						using (var context = new Context())
						{
							context.Companies.Add(company);
							context.SaveChanges();
						}
					}
					catch
					{
						// just leave that one for later
					}
				});
			}
			while (serials.Any());
		}

		private static IEnumerable<int> GetSerials(int firstSerial = 1000000, int highestSerial = 9999999, int batchSize = 10000)
		{
			var count = highestSerial - firstSerial;
			var allSerials = Enumerable.Range(firstSerial, count);

			Console.WriteLine("Finding serials that have not yet been downloaded...");
			var watch = Stopwatch.StartNew();
			
			IEnumerable<int> knownSerials;
			using (var context = new Context())
			{
				knownSerials = context.Companies.Select(x => x.Serial).ToList();
			}
			var remainingSerials = allSerials.Except(knownSerials).Shuffle().Take(batchSize).ToList();

			watch.Stop();
			Console.WriteLine("Found {0} serials remaining in {1}ms", remainingSerials.Count(), watch.ElapsedMilliseconds);

			return remainingSerials;
		}
	}
}
