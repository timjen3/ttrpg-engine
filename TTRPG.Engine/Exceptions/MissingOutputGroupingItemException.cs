using System;

namespace TTRPG.Engine.Exceptions
{
	public class MissingOutputGroupingItemException : Exception
	{
		public MissingOutputGroupingItemException(string message) : base(message)
		{
		}
	}
}
