using TTRPG.Engine.Equations;
using Microsoft.Extensions.DependencyInjection;
using org.mariuszgromada.math.mxparser;
using TTRPG.Engine.Equations.Extensions;

namespace TTRPG.Engine
{
	public static class IServiceCollectionExtensions
	{
		/// <summary>
		///		Adds required services for ttrpg engine
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddTTRPGEngineServices(this IServiceCollection services)
		{
			services.AddSingleton(new Function("random", new RandomFunctionExtension()));
			services.AddSingleton(new Function("toss", new CoinTossFunctionExtension()));
			services.AddSingleton<IEquationResolver, EquationResolver>();
			services.AddSingleton<EquationService>();

			return services;
		}
	}
}
