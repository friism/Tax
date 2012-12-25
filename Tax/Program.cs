using Tax.Analysis;
using Tax.DataLoad;

namespace Tax
{
	public class Program
	{
		static void Main(string[] args)
		{
			//new DataLoader().Load();
			new Analyzer().Analyze();
		}
	}
}
