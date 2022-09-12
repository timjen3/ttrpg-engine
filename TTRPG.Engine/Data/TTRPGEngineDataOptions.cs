using System;
using System.Collections.Generic;
using System.Text;

namespace TTRPG.Engine.Data
{
	public enum DataStorageType
	{
		JsonFile = 0
	}

	public class JsonFileStorageOptions
	{
		public string SequencesFileName { get; set; }

		public string SequenceItemsFileName { get; set; }

		public string RolesFileName { get; set; }
	}

	public class TTRPGEngineDataOptions
	{
		public DataStorageType StorageType { get; set; }

		public JsonFileStorageOptions JsonFileStorageOptions { get; set; }
	}
}
