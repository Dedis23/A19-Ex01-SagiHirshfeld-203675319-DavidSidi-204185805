using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Player2Spaceship : Spaceship
    {
        private const string k_AssetName = @"Sprites\Ship02_32x32";
        public Player2Spaceship(Game i_Game) : base(k_AssetName, i_Game)
        {
        }

        protected override bool MoveLeftDetected()
        {
            return m_InputManager.KeyboardState.IsKeyDown(Keys.A);
        }

        protected override bool MoveRightDetected()
        {
            return m_InputManager.KeyboardState.IsKeyDown(Keys.D);
        }

        protected override bool ShootDetected()
        {
            return m_InputManager.KeyPressed(Keys.W);
        }
    }
}