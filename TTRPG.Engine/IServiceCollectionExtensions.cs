using TTRPG.Engine.Equations;
using Microsoft.Extensions.DependencyInjection;

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
			services.AddSingleton<IEquationResolver, EquationResolver>();

			return services;
		}
	}
}
