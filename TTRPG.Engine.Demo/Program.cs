using TTRPG.Engine.Demo.Demo;

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
