using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using TTRPG.Engine.Demo.Engine;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.Demo2
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
			var provider = collection.BuildServiceProvider();
			var equationService = provider.GetRequiredService<IEquationService>();

			var loader = new CombatSequenceDataLoader();

			return new CombatDemoForm(equationService, loader);
		}
	}
}
