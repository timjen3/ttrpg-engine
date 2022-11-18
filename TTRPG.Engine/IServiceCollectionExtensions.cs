using System;
using System.Linq;
using Jace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.CommandParsing.Parsers;
using TTRPG.Engine.CommandParsing.Processors;
using TTRPG.Engine.Data;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.Engine;
using TTRPG.Engine.Engine.Events;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine
{
	/// <summary>
	///		Add TTRPG.Engine services to an IServiceCollection.
	/// </summary>
	public static class IServiceCollectionExtensions
	{
		/// <summary>
		///		Adds required services for ttrpg engine
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddTTRPGEngineServices(this IServiceCollection services)
		{
			services.AddSingleton(sp =>
			{
				var random = new Random();
				var engine = new CalculationEngine();
				// add a couple extra functions
				engine.AddFunction("rnd", (count, minRange, maxRange)
					=> Enumerable.Range(1, (int) Math.Floor(count))
						.Select(i => random.Next((int)Math.Floor(minRange), (int)Math.Floor(maxRange) + 1))
						.Sum(), false);
				engine.AddFunction("toss", () => random.Next(0, 1), false);
				engine.AddFunction("roundx", (number, digits) => Math.Round(number, (int) digits));

				return engine;
			});
			services.AddSingleton<IEquationResolver, EquationResolver>();
			services.AddSingleton<IEquationService, EquationService>();
			services.AddSingleton<IInventoryService, InventoryService>();
			services.AddSingleton<IRoleService, RoleService>();
			services.AddSingleton<ICommandProcessorFactory, CommandProcessorFactory>();
			services.AddSingleton<ITTRPGCommandProcessor, EquationProcessor>();
			services.AddSingleton<ITTRPGCommandProcessor, InventoryProcessor>();
			services.AddSingleton<ICommandParser, EquationCommandParser>();
			services.AddSingleton<ICommandParser, InventoryCommandParser>();
			services.TryAddSingleton(new AutomaticCommandFactoryOptions());
			services.AddSingleton<IAutomaticCommandFactory, AutomaticCommandFactory>();
			services.AddSingleton<ITTRPGEventHandler, TTRPGEventHandler>();
			services.AddSingleton<GameObject>();
			services.AddSingleton<TTRPGEngine>();

			return services;
		}

		/// <summary>
		///		Adds data layer to services
		/// </summary>
		/// <param name="services"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static IServiceCollection AddTTRPGEngineDataLayer(this IServiceCollection services, TTRPGEngineDataOptions options)
		{
			services.AddSingleton(options);
			if (options.StorageType == DataStorageType.JsonFile)
			{
				services.AddSingleton<ITTRPGDataRepository, JsonTTRPGDataRepository>();
			}

			return services;
		}
	}
}
