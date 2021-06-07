using System;

namespace DieEngine.Exceptions
{
	public class RollConditionFailedException : Exception
	{
		public RollConditionFailedException(string message) : base(message)
		{
		}
	}
}
