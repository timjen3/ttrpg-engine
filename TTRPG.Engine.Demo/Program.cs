using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using TTRPG.Engine.Data;

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
			var provider = collection.BuildServiceProvider();
			var gameObject = provider.GetRequiredService<GameObject>();
			var engine = provider.GetRequiredService<TTRPGEngine>();

			return new CombatDemoForm(gameObject, engine);
		}
	}
}
