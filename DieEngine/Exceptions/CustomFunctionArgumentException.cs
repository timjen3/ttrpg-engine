using System;

namespace DieEngine.Exceptions
{
	public class CustomFunctionArgumentException : Exception
	{
		public CustomFunctionArgumentException(string message) : base(message)
		{
		}
	}
}
