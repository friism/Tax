using Tax.Analysis;
using Tax.DataLoad;
using Tax.Export;

namespace Tax
{
	public class Program
	{
		static void Main(string[] args)
		{
			//new DataLoader().Load();
			//new Analyzer().Analyze();
			//new DuplicateRemover().RemoveDuplicates();
			new Exporter().Export();
		}
	}
}
