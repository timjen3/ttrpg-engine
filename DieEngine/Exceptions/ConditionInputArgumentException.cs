using System;

namespace DieEngine.Exceptions
{
	public class DieInputArgumentException : Exception
	{
		public DieInputArgumentException(string message) : base(message)
		{
		}
	}
}
