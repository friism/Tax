using Tax.DataLoad;

namespace Tax
{
	public class Program
	{
		static void Main(string[] args)
		{
			new DataLoader().Load();
		}
	}
}
