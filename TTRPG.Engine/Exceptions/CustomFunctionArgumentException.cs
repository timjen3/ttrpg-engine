using System;

namespace TTRPG.Engine.Exceptions
{
    public class CustomFunctionArgumentException : Exception
    {
        public CustomFunctionArgumentException(string message) : base(message)
        {
        }
    }
}
