using DieEngine.CustomFunctions;
using DieEngine.CustomFunctions.Defaults;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
			// load default custom functions (w/ parameterless constructors) from this assembly using reflection	
			var defaultEquations = typeof(DiceFunction).Assembly
				.GetTypes()
				.Where(x => !x.IsAbstract && !x.IsInterface && typeof(ICustomFunction).IsAssignableFrom(x)
					&& x.GetConstructor(Type.EmptyTypes) != null)
				.Select(x => (ICustomFunction)Activator.CreateInstance(x));
			foreach(var equation in defaultEquations)
			{
				services.AddSingleton(equation);
			}

			services.AddSingleton<ICustomFunctionRunner, CustomFunctionRunner>();

			services.AddSingleton<IEquationResolver, EquationResolver>();

			return services;
		}
	}
}
