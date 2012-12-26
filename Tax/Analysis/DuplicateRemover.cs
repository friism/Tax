using System.Linq;
using Tax.Persistence;

namespace Tax.Analysis
{
	public class DuplicateRemover
	{
		public void RemoveDuplicates()
		{
			using (var context = new Context())
			{
				var groups = from x in context.Companies
							 group x by x.Cvr into g
							 where g.Count() > 1
							 select g;

				foreach (var group in groups)
				{
					foreach (var company in group.Skip(1))
					{
						context.Companies.Remove(company);
					}
				}

				context.SaveChanges();
			}
		}
	}
}
