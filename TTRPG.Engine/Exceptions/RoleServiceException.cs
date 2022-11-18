using System;

namespace TTRPG.Engine.Exceptions
{
	public class RoleServiceException : Exception
	{
		public RoleServiceException(string message) : base(message)
		{
		}
	}
}
