namespace Poker.Interfaces
{
    /// <summary>
    /// Represents public methods of a factory which can raise instances of type IHuman
    /// </summary>
    public interface IHumanFactory
    {
        /// <summary>
        /// raises an instance of Human
        /// </summary>
        /// <returns>Human represented as IHuman</returns>
        IHuman Create();
    }
}