using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using org.mariuszgromada.math.mxparser;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.CommandParsing.Parsers;
using TTRPG.Engine.CommandParsing.Processors;
using TTRPG.Engine.Data;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.Engine;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Equations.Extensions;

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
            services.TryAddSingleton(new TTRPGEngineOptions());
            services.AddSingleton(new Function("random", new RandomFunctionExtension()));
            services.AddSingleton(new Function("toss", new CoinTossFunctionExtension()));
            services.AddSingleton<IEquationResolver, EquationResolver>();
            services.AddSingleton<IEquationService, EquationService>();
            services.AddSingleton<IInventoryService, InventoryService>();
            services.AddSingleton<ICommandProcessorFactory, CommandProcessorFactory>();
            services.AddSingleton<ITTRPGCommandProcessor, EquationProcessor>();
            services.AddSingleton<ITTRPGCommandProcessor, InventoryProcessor>();
            services.AddSingleton<ICommandParser, EquationCommandParser>();
            services.AddSingleton<ICommandParser, InventoryCommandParser>();
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
