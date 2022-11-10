using System;

namespace TTRPG.Engine.Exceptions
{
	public class EntityConditionFailedException : Exception
	{
		public EntityConditionFailedException(string message) : base(message)
		{
		}
	}
}
