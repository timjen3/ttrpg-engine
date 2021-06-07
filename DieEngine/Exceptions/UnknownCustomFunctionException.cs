using System;

namespace DieEngine.Exceptions
{
	public class UnknownCustomFunctionException : Exception
	{
		public UnknownCustomFunctionException(string message) : base(message){}
	}
}
