using System;

namespace DieEngine.Exceptions
{
	public class ConditionFailedException : Exception
	{
		public ConditionFailedException(string message) : base(message)
		{
		}
	}
}
