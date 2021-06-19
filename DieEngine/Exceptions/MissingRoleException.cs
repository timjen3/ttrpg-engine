using System;

namespace DieEngine.Exceptions
{
	public class MissingRoleException : Exception
	{
		public MissingRoleException(string message) : base(message)
		{
		}
	}
}
