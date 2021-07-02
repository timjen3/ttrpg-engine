using System;
using System.Collections.Generic;
using System.Text;

namespace TTRPG.Engine.Demo.Demo
{
	public static class SequenceResultHandler
	{
		public static void HandleSequenceResult(SequenceResult result)
		{
			switch (result.Sequence.Name)
			{
				case "Attack":
				{

					break;
				}
				case "UsePotion":
				{

					break;
				}
				default:
				{
					throw new NotImplementedException("Unknown case: " + result.Sequence.Name);
				}
			}
		}
	}
}
