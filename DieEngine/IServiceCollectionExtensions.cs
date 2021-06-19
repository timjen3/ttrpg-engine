using DieEngine.Equations;
using Microsoft.Extensions.DependencyInjection;

namespace DieEngine
{
	public static class IServiceCollectionExtensions
	{
		/// <summary>
		///		Adds required services for die engine
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddDieEngineServices(this IServiceCollection services)
		{
			services.AddSingleton<IEquationResolver, EquationResolver>();

			return services;
		}
	}
}
