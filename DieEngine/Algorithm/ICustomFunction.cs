namespace DieEngine.Algorithm
{
	public interface ICustomFunction
	{
		string FunctionName { get; }

		double DoFunction(params string[] values);
	}
}
