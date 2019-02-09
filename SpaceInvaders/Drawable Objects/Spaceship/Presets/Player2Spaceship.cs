using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Player2Spaceship : Spaceship
    {
        private const string k_AssetName = @"Sprites\Ship02_32x32";

        protected override Keys MoveLeftKey
        {
            get
            {
                return Keys.A;
            }
        }

        protected override Keys MoveRightKey
        {
            get
            {
                return Keys.D;
            }
        }

        protected override Keys ShootKey
        {
            get
            {
                return Keys.W;
            }
        }

        public Player2Spaceship(Game i_Game) : base(k_AssetName, i_Game)
        {
        }
    }
}