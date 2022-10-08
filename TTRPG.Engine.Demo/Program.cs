using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using TTRPG.Engine.Data;
using TTRPG.Engine.Demo.Engine;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.Demo
{
	static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(GetForm());
		}

		private static Form GetForm()
		{
			var collection = new ServiceCollection();
			collection.AddTTRPGEngineServices();
			collection.AddTTRPGEngineDataLayer(new TTRPGEngineDataOptions
			{
				StorageType = DataStorageType.JsonFile,
				JsonFileStorageOptions = new JsonFileStorageOptions
				{
					RolesFileDirectory = "DataFiles/Roles",
					SequencesFileDirectory = "DataFiles/Sequences",
					SequenceItemsFileDirectory = "DataFiles/SequenceItems",
					MessageTemplatesDirectory = "DataFiles/MessageTemplates"
				}
			});
			collection.AddScoped<GameObject>();
			var provider = collection.BuildServiceProvider();
			var equationService = provider.GetRequiredService<IEquationService>();
			var inventoryService = provider.GetRequiredService<IInventoryService>();
			var gameObject = provider.GetRequiredService<GameObject>();

			return new CombatDemoForm(equationService, inventoryService, gameObject);
		}
	}
}
