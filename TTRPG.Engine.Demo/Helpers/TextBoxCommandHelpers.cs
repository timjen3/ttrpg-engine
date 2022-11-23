using System;

namespace TTRPG.Engine.Demo.Helpers;

internal static class TextBoxCommandHelpers
{
	internal static string GetTextBoxCommandForTargetItem(string targetType, string targetName)
	{
		if (string.IsNullOrWhiteSpace(targetName))
		{
			return "";
		}
		if (targetType == "terrain")
		{
			return $"MineTerrain [miner:miner,{targetName}:terrain]";
		}
		else if (targetType == "animal")
		{
			return $"Hunt [miner:hunter,{targetName}:defender]";
		}
		else if (targetType == "crop")
		{
			return $"Harvest [miner:farmer,{targetName}:crop]";
		}
		throw new NotImplementedException($"Unknown target type {targetType}");
	}
}
