using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public interface IPlayer
    {
        int Score { get; }

        Color ScoreColor { get; }

        string Name { get; }
    }
}
