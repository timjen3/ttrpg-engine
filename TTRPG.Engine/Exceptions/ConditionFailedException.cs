using System;

namespace TTRPG.Engine.Exceptions
{
	public class ConditionFailedException : Exception
	{
		public ConditionFailedException(string message) : base(message)
		{
		}
	}
}
