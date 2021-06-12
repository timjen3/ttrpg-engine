namespace DieEngine.CustomFunctions
{
	public interface ICustomFunction
	{
		string FunctionName { get; }

		double DoFunction(params string[] values);
	}
}
