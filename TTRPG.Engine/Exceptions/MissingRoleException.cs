using System;

namespace TTRPG.Engine.Exceptions
{
	public class MissingRoleException : Exception
	{
		public MissingRoleException(string message) : base(message)
		{
		}
	}
}
