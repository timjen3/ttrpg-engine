using System;

namespace DieEngine.Algorithm
{
	public abstract class BaseCustomFunction : ICustomFunction
	{
		protected void Validate(string[] parameters)
		{
			if (parameters == null || parameters.Length < RequiredParamCount)
				throw new ArgumentException($"The {FunctionName} equation requires {RequiredParamCount} parameters.");
		}

		protected int ParseIntParamOrThrow(string value, int paramNumber, string paramName)
		{
			if (int.TryParse(value, out int result))
				return result;

			throw new ArgumentException($"Parameter number {paramNumber} '{paramName}' could not be parsed to an integer! Value: {value}.");
		}

		public abstract int RequiredParamCount { get; }

		public abstract string FunctionName { get; }

		public abstract double DoFunction(params string[] values);
	}
}
