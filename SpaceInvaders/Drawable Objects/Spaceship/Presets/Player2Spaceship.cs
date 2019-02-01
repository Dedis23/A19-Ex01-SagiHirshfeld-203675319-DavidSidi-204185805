using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Player2Spaceship : Spaceship
    {
        private const string k_AssetName = @"Sprites\Ship02_32x32";

        public override Color ScoreColor { get; } = Color.Green;

        public override string Name { get; set; } = "P2";

        public Player2Spaceship(Game i_Game) : base(k_AssetName, i_Game)
        {
        }
    }
}