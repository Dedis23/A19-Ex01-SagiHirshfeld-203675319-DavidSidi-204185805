using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Player1Spaceship : Spaceship
    {
        private const string k_AssetName = @"Sprites\Ship01_32x32";
        public Player1Spaceship(Game i_Game) : base(k_AssetName ,i_Game)
        {
        }

        protected override void TakeInput()
        {
            base.TakeInput();
            moveAccordingToMousePositionDelta(m_InputManager.MousePositionDelta);
        }

        private void moveAccordingToMousePositionDelta(Vector2 i_MousePositionDelta)
        {
            Position += new Vector2(i_MousePositionDelta.X, 0);
        }

        protected override bool MoveLeftDetected()
        {
            return m_InputManager.KeyboardState.IsKeyDown(Keys.H);
        }

        protected override bool MoveRightDetected()
        {
            return m_InputManager.KeyboardState.IsKeyDown(Keys.K);
        }

        protected override bool ShootDetected()
        {
            return m_InputManager.KeyPressed(Keys.U) || m_InputManager.ButtonPressed(eInputButtons.Left);
        }
    }
}
