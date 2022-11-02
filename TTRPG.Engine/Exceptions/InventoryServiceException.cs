using System;

namespace TTRPG.Engine.Exceptions
{
    public class InventoryServiceException : Exception
    {
        public InventoryServiceException(string message) : base(message)
        {
        }
    }
}
