namespace Poker.Interfaces
{

    /// <summary>
    /// Factory which can raise instances of type IBot
    /// </summary>
    public interface IBotFactory
    {
        /// <summary>
        /// raises an instance of Bot
        /// </summary>
        /// <param name="number">bot ID</param>
        /// <returns>Bot represented as IBot</returns>
        IBot Create(int number);
    }
}