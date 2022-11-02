namespace TTRPG.Engine.CommandParsing.Processors
{
    public interface ITTRPGCommandProcessor
    {
        /// processes command and returns messages
        ProcessedCommand Process();

        bool IsValid();
    }
}
