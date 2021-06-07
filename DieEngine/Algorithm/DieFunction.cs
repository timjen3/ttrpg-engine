using System;
using System.Linq;

namespace DieEngine.Algorithm
{
	/// <summary>
	///		Roll 1+ same-sided die
	/// </summary>
	public class DiceFunction : BaseCustomFunction
	{
		private readonly Random _gen = new Random();

		public override string FunctionName => "Dice";
		public override int RequiredParamCount => 2;

		public override double DoFunction(params string[] values)
		{
			this.Validate(values);

			int dieCount = this.ParseIntParamOrThrow(values[0], 1, nameof(dieCount));
			int maxRange = this.ParseIntParamOrThrow(values[1], 2, nameof(maxRange));
			int roll = Enumerable.Range(1, dieCount)
				.Select(i => _gen.Next(1, maxRange + 1))
				.Sum();

			return roll;
		}
	}
}
