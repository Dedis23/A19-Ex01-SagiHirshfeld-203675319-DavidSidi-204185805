using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceInvaders
{
    public abstract class Spaceship : Sprite, ICollidable2D, IShooter
    {
        private const int k_ScorePenaltyForBulletHit = 1100;
        private const int k_Velocity = 120;
        private const int k_MaxBulletsInScreen = 3;
        private const int k_StartingLivesCount = 3;
        private readonly Gun r_Gun;
        private int m_Score;
        public Color BulletsColor { get; } = Color.Red;
        protected IInputManager m_InputManager;

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

        public Spaceship(string k_AssetName, Game i_Game) : base(k_AssetName, i_Game)
        {
            r_Gun = new Gun(this, k_MaxBulletsInScreen);
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
            base.Update(i_GameTime);

            TakeInput();

            // Clamp the position between screen boundries:
            float x = MathHelper.Clamp(Position.X, 0, this.GameScreenBounds.Width - this.Width);
            Position = new Vector2(x, Position.Y);
        }

        protected virtual void TakeInput()
        {
            if (MoveLeftDetected())
            {
                m_Velocity.X = k_Velocity * -1;
            }
            else if (MoveRightDetected())
            {
                m_Velocity.X = k_Velocity;
            }
            else
            {
                m_Velocity.X = 0;
            }

            if (ShootDetected())
            {
                Shoot();
            }
        }

        // Injection points that are meant to be overriden by inheritors
        protected virtual bool MoveLeftDetected()
        {
            return false;
        }
        protected virtual bool MoveRightDetected()
        {
            return false;
        }
        protected virtual bool ShootDetected()
        {
            return false;
        }

        private void Shoot()
        {
            r_Gun.Shoot();
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