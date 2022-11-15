using System.Collections.Generic;
using Newtonsoft.Json;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.Engine.Events;
using TTRPG.Engine.SequenceItems;

namespace TTRPG.Engine.Sequences
{
	public class Sequence
	{
		public string Name { get; set; }

		public string Example { get; set; }

		public List<SequenceItem> Items { get; set; } = new List<SequenceItem>();

		[JsonProperty(ItemConverterType = typeof(TTRPGEventConfigJsonConverter))]
		public List<EventConfig> Events { get; set; } = new List<EventConfig>();

		public List<Condition> Conditions { get; set; } = new List<Condition>();

		public List<EntityCondition> EntityConditions { get; set; } = new List<EntityCondition>();

		public List<Mapping> Mappings { get; set; } = new List<Mapping>();

		public List<string> Categories { get; set; } = new List<string>();

		public Dictionary<string, Dictionary<string, string>> CategoryParams { get; set; } = new Dictionary<string, Dictionary<string, string>>();
	}
}
