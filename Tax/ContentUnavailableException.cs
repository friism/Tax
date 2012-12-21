using System;

namespace Tax
{
	public class ContentUnavailableException : Exception
	{
		public ContentUnavailableException(string message)
			: base(message)
		{
		}
	}
}
