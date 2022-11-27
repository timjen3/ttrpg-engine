namespace TTRPG.Engine.Data
{
	public enum DataStorageType
	{
		JsonFile = 0
	}

	public class JsonFileStorageOptions
	{
		public string SequencesFileDirectory { get; set; }

		public string EntitiesFileDirectory { get; set; }

		public string RolesFileDirectory { get; set; }

		public string MessageTemplatesDirectory { get; set; }

		public string CommandsDirectory { get; set; }
	}

	public class TTRPGEngineDataOptions
	{
		public DataStorageType StorageType { get; set; }

		public JsonFileStorageOptions JsonFileStorageOptions { get; set; }
	}
}
