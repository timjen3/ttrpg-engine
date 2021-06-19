using System;
using System.Linq;

namespace DieEngine.CustomFunctions.Defaults
{
	/// <summary>
	///		Generate 1+ random number between min and max integer
	/// </summary>
	public class RandomFunction : BaseCustomFunction
	{
		private static readonly Random _gen = new Random();

		public override string FunctionName => "Random";
		public override int RequiredParamCount => 3;

		public double DoFunction(int count, int minRange, int maxRange)
		{
			int roll = Enumerable.Range(1, count)
				.Select(i => _gen.Next(minRange, maxRange + 1))
				.Sum();

			return roll;
		}

		public override double DoFunction(params string[] values)
		{
			this.Validate(values);

			int count = this.ParseIntParamOrThrow(values[0], 1, nameof(count));
			int minRange = this.ParseIntParamOrThrow(values[1], 2, nameof(minRange));
			int maxRange = this.ParseIntParamOrThrow(values[2], 3, nameof(maxRange));

			return DoFunction(count, minRange, maxRange);
		}
	}
}
