using System;

namespace TTRPG.Engine.Exceptions
{
	public class MissingEntityException : Exception
	{
		public MissingEntityException(string message) : base(message)
		{
		}
	}
}
