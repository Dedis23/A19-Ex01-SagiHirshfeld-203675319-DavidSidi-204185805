using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Player1Spaceship : Spaceship
    {
        private const string k_AssetName = @"Sprites\Ship01_32x32";

        protected override Keys MoveLeftKey
        {
            get
            {
                return Keys.H;
            }
        }

        protected override Keys MoveRightKey
        {
            get
            {
                return Keys.K;
            }
        }

        protected override Keys ShootKey
        {
            get
            {
                return Keys.U;
            }
        }

        public Player1Spaceship(Game i_Game) : base(k_AssetName, i_Game)
        {
        }

        protected override void TakeInput()
        {
            base.TakeInput();

            if (!InputManager.KeyPressed(ShootKey) && InputManager.ButtonPressed(eInputButtons.Left))
            {
                Shoot();
            }

            Position += new Vector2(InputManager.MousePositionDelta.X, 0);
        }
    }
}
