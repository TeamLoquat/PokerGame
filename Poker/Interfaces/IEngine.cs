using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Interfaces
{
    /// <summary>
    /// Interface for what a basic program engine should do
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Initial point of entry for the main loop of the project
        /// </summary>
        void Run();
    }
}
