using System;

namespace DieEngine.Exceptions
{
	public class MappingFailedException : Exception
	{
		public MappingFailedException(string message) : base(message)
		{
		}
	}
}
