
namespace Tax.Model
{
	public class Company : Entity
	{
		public int Serial { get; set; }
		public int Cvr { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Legislation { get; set; }
		public decimal? TaxPaid { get; set; }
		public decimal? Revenue { get; set; }
		public decimal? Losses { get; set; }
		public decimal? FossilTaxPaid { get; set; }
		public decimal? FossilProfit { get; set; }
		public decimal? FossilLosses { get; set; }
		public string Subsidiaries { get; set; }
		public bool IsSubsidiary { get; set; }
	}
}
