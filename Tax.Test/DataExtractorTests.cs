using System.Text;
using Tax.Model;
using Xunit;
using Xunit.Extensions;

namespace Tax.Test
{
	public class DataExtractorTests
	{
		[Theory]
		[InlineData(29604274, "FORLAGET SOHN ApS")]
		[InlineData(15731249, "SAXO BANK A/S")]
		public void Test(int cvrNumber, string name)
		{
			var contentHelper = new ContentHelper(Encoding.GetEncoding("ISO-8859-1"));
			var document = contentHelper.GetContent(cvrNumber);

			var company = new Company();
			var extractor = new DataExtractor();
			extractor.Extract(company, document);

			Assert.Equal(name, company.Name);
		}
	}
}
