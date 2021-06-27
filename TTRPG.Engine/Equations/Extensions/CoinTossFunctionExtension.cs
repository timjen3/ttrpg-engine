using org.mariuszgromada.math.mxparser;
using System;

namespace TTRPG.Engine.Equations.Extensions
{
	public class CoinTossFunctionExtension : FunctionExtension
	{
		private static readonly Random Gen = new Random();

		public double calculate()
		{
			int toss = Gen.Next(0, 2);

			return toss;
		}

		public FunctionExtension clone()
		{
			return new RandomFunctionExtension()
			{
			};
		}

		public string getParameterName(int parameterIndex)
		{
			switch (parameterIndex)
			{
				case 0:
					return "placeholder";
				default:
					throw new ArgumentOutOfRangeException($"{nameof(CoinTossFunctionExtension)} does not accept any parameters.");
			}
		}

		public int getParametersNumber()
		{
			return 1;
		}

		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			switch (parameterIndex)
			{
				case 0:
					return;
				default:
					throw new ArgumentOutOfRangeException($"{nameof(CoinTossFunctionExtension)} does not accept any parameters.");
			}
		}
	}
}
