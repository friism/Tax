using FileHelpers;
using System.Collections.Generic;
using System.Linq;
using Tax.Persistence;

namespace Tax.Export
{
	public class Exporter
	{
		public void Export()
		{
			IEnumerable<Record> records;
			using (var context = new Context())
			{
				records = context.Companies.Select(x => new Record {
					Name = x.Name,
					Cvr = x.Cvr,
					Type = x.Type,
					Legislation = x.Legislation,
					Profit = x.Revenue,
					Losses = x.Losses,
					Tax = x.TaxPaid,
					FossilProfit = x.FossilProfit,
					FossilLosses = x.FossilLosses,
					FossilTax = x.FossilTaxPaid,
					IsSubsidiary = x.IsSubsidiary,
					Subsidiaries = x.Subsidiaries,
				}).ToList();
			}

			var colummnHeaders = new[] { "Name", "Cvr", "Type", "Legislation", "Profit", "Losses", "Tax", "FossilProfit", "FossilLosses", "FossilTax", "IsSubsidiary", "Subsidiaries" };

			var engine = new FileHelperEngine<Record>()
			{
				HeaderText = string.Join(",", colummnHeaders),
			};
			engine.WriteFile("out.csv", records);
		}
	}

	[DelimitedRecord(",")]
	class Record
	{
		[FieldQuoted('"', QuoteMode.AlwaysQuoted)]
		public string Name;
		public int Cvr;
		[FieldQuoted('"', QuoteMode.AlwaysQuoted)]
		public string Type;
		[FieldQuoted('"', QuoteMode.AlwaysQuoted)]
		public string Legislation;
		public decimal? Profit;
		public decimal? Losses;
		public decimal? Tax;
		public decimal? FossilProfit;
		public decimal? FossilLosses;
		public decimal? FossilTax;
		public bool IsSubsidiary;
		public string Subsidiaries;
	}
}
