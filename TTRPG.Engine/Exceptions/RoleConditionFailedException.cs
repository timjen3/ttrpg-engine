using System;

namespace TTRPG.Engine.Exceptions
{
	public class RoleConditionFailedException : Exception
	{
		public RoleConditionFailedException(string message) : base(message)
		{
		}
	}
}
