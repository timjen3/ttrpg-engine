using System;

namespace TTRPG.Engine.Exceptions
{
	public class UnknownCustomFunctionException : Exception
	{
		public UnknownCustomFunctionException(string message) : base(message) { }
	}
}
