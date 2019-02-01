using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Player1Spaceship : Spaceship
    {
        private const string k_AssetName = @"Sprites\Ship01_32x32";

        public override Color ScoreColor { get; } = Color.Blue;

        public override string Name { get; set; } = "P1";

        public Player1Spaceship(Game i_Game) : base(k_AssetName, i_Game)
        {
        }
    }
}
