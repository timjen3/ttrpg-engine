using System;

namespace TTRPG.Engine.Exceptions
{
	public class EquationResolverException : Exception
	{
		private static string MakeExceptionMessage(string equation, Exception innerException)
			=> $"{innerException.Message}\nEquation: {equation}";

		public EquationResolverException(string equation, Exception innerException) : base(MakeExceptionMessage(equation, innerException))
		{
		}
	}
}
