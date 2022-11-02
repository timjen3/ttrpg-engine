using System.Collections.Generic;

namespace TTRPG.Engine.Equations
{
    /// <summary>
    ///		Resolves equations
    /// </summary>
    public interface IEquationResolver
    {
        /// <summary>
        ///		Resolves an mxParser expression
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="inputs">Arguments to inject into the equation</param>
        /// <returns></returns>
        double Process(string equation, IDictionary<string, string> inputs);
    }
}
