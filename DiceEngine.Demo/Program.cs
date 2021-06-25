using DieEngine.Demo.Demos;
using DieEngine.Equations;
using DieEngine.Mappings;
using DieEngine.SequencesItems;
using System;
using System.Collections.Generic;

namespace DieEngine.Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			var demo = new CombatDemo();
			demo.DoDemo();
		}
	}
}
