using TTRPG.Engine.Demo.Demos;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Mappings;
using TTRPG.Engine.SequenceItems;
using System;
using System.Collections.Generic;

namespace TTRPG.Engine.Demo
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
