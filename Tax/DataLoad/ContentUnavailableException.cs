using System;

namespace Tax.DataLoad
{
	public class ContentUnavailableException : Exception
	{
		public ContentUnavailableException(string message)
			: base(message)
		{
		}
	}
}
