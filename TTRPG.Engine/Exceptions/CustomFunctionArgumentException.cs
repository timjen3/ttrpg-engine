using System;

namespace TTRPG.Engine.Exceptions
{
	public class CustomFunctionArgumentException : Exception
	{
		private static string MakeExceptionMessage(string equation, Exception innerException)
			=> $"{innerException.Message}\nEquation: {equation}";

		public CustomFunctionArgumentException(string message, Exception innerException) : base(MakeExceptionMessage(message, innerException))
		{
		}
	}
}
