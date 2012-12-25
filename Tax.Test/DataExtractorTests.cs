using System.Text;
using Tax.DataLoad;
using Tax.Model;
using Xunit;
using Xunit.Extensions;

namespace Tax.Test
{
	public class DataExtractorTests
	{
		[Theory]
		[InlineData(29604274, "FORLAGET SOHN ApS", 0L, -52598L, null, null, null, null)]
		[InlineData(15731249, "SAXO BANK A/S", 25426135L, 257969357L, null, null, null, null)]
		[InlineData(87197719, "SHELL OLIE- OG GASUDVINDING DANMARK B.V. (HOLLAND), DANSK  FILIAL", 3825964525L, 15303858194L, null, 5213476840L, 10025917000L, 0L)]
		[InlineData(29520852, "NORECO DENMARK A/S", 185342975L, 741371927L, 12179356L, 0L, -72864162L, 119751074L)]
		public void Test(int cvrNumber, string name, long? tax, long? profit, long? losses, long? fossilTax, long? fossilProfit, long? fossilLosses)
		{
			var contentHelper = new ContentHelper(Encoding.GetEncoding("ISO-8859-1"));
			var document = contentHelper.GetContent(cvrNumber);

			var company = new Company();
			var extractor = new DataExtractor();
			extractor.Extract(company, document);

			Assert.Equal(name, company.Name);

			Assert.Equal((decimal?)tax, company.TaxPaid);
			Assert.Equal((decimal?)profit, company.Revenue);
			Assert.Equal((decimal?)losses, company.Losses);

			Assert.Equal((decimal?)fossilTax, company.FossilTaxPaid);
			Assert.Equal((decimal?)fossilProfit, company.FossilProfit);
			Assert.Equal((decimal?)fossilLosses, company.FossilLosses);
		}
	}
}
