namespace Poker.Interfaces
{
    public interface IBotFactory
    {
        IBot Create(int number);
    }
}