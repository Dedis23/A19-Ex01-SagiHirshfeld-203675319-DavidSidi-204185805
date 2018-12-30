using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceInvaders
{
    public class Spaceship : Sprite, ICollidable2D, IShooter
    {
        private const string k_AssetName = @"Sprites\Ship01_32x32";
        private const int k_ScorePenaltyForBulletHit = 1100;
        private const int k_Velocity = 120;
        private const int k_MaxBulletsInScreen = 3;
        private const int k_StartingLivesCount = 3;
        private readonly Gun r_Gun;
        private int m_Score;
        public Color BulletsColor { get; } = Color.Red;
        private IInputManager m_InputManager;

        public int Lives { get; set; }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value > 0 ? value : 0;
            }
        }

        public Spaceship(Game i_Game) : base(k_AssetName, i_Game)
        {
            r_Gun = new Gun(this);
            Lives = k_StartingLivesCount;
            m_Score = 0;
        }

        public override void Initialize()
        {
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            base.Initialize();
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            SetDefaultPosition();
        }

        public void SetDefaultPosition()
        {
            // Get the bottom and center:
            float x = 0;
            float y = (float)GraphicsDevice.Viewport.Height;

            // Offset:
            y -= Texture.Height * 1.5f;

            Position = new Vector2(x, y);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                m_Velocity.X = k_Velocity * -1;
            }
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                m_Velocity.X = k_Velocity;
            }
            else
            {
                m_Velocity.X = 0;
            }

            if (m_InputManager.KeyPressed(Keys.Enter) || m_InputManager.ButtonPressed(eInputButtons.Left))
            {
                Shoot();
            }

            base.Update(i_GameTime);

            MoveAccordingToMousePositionDelta(m_InputManager.MousePositionDelta);

            // Clamp the position between screen boundries:
            float x = MathHelper.Clamp(Position.X, 0, this.GameScreenBounds.Width - this.Width);
            Position = new Vector2(x, Position.Y);
        }

        public void MoveAccordingToMousePositionDelta(Vector2 i_MousePositionDelta)
        {
            Position += new Vector2(i_MousePositionDelta.X, 0);
        }

        public void Shoot()
        {
            if (r_Gun.NumberOfShotBulletsInScreen < k_MaxBulletsInScreen)
            {
                r_Gun.Shoot();
            }
        }

        public void TakeBulletHit()
        {
            Lives--;
            Score -= k_ScorePenaltyForBulletHit;
            if(Lives == 0)
            {
                this.Kill();
            }

            SetDefaultPosition();
        }
    }
}