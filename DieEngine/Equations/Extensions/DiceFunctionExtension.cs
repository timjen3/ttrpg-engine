using org.mariuszgromada.math.mxparser;
using System;
using System.Linq;

namespace DieEngine.Equations.Extensions
{
	public class DiceFunctionExtension : FunctionExtension
	{
		private static readonly Random Gen = new Random();
		const int CountIndex = 0;
		const int MinRangeIndex = 1;
		const int MaxRangeIndex = 2;

		public double Count { get; set; }
		public double MinRange { get; set; }
		public double MaxRange { get; set; }

		public double calculate()
		{
			int roll = Enumerable.Range(1, (int) Math.Floor(Count))
				.Select(i => Gen.Next((int) Math.Floor(MinRange), (int) Math.Floor(MaxRange) + 1))
				.Sum();

			return roll;
		}

		public FunctionExtension clone()
		{
			return new DiceFunctionExtension()
			{
				Count = Count,
				MinRange = MinRange,
				MaxRange = MaxRange
			};
		}

		public string getParameterName(int parameterIndex)
		{
			switch (parameterIndex)
			{
				case CountIndex:
					return nameof(Count);
				case MinRangeIndex:
					return nameof(MinRange);
				case MaxRangeIndex:
					return nameof(MaxRange);
				default:
					throw new ArgumentOutOfRangeException($"{parameterIndex} is not a valid index for the {nameof(DiceFunctionExtension)}.");
			}
		}

		public int getParametersNumber()
		{
			return 3;
		}

		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			switch (parameterIndex)
			{
				case CountIndex:
					Count = parameterValue;
					return;
				case MinRangeIndex:
					MinRange = parameterValue;
					return;
				case MaxRangeIndex:
					MaxRange = parameterValue;
					return;
				default:
					throw new ArgumentOutOfRangeException($"{parameterIndex} is not a valid index for the {nameof(DiceFunctionExtension)}.");
			}
		}
	}
}
