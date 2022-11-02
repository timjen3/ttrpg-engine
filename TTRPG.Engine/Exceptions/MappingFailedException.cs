using System;

namespace TTRPG.Engine.Exceptions
{
	public class MappingFailedException : Exception
	{
		public MappingFailedException(string message) : base(message)
		{
		}
	}
}
