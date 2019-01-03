using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceInvaders
{
    public abstract class Spaceship : Sprite, ICollidable2D, IShooter, IPlayer
    {        
        private const int k_ScorePenaltyForBulletHit = 1100;
        private const int k_VelocityScalar = 120;
        private const int k_MaxBulletsInScreen = 3;        
        private const int k_StartingLivesCount = 3;
        private const float k_LivesDrawingGapModifier = 0.7f;
        private const float k_LivesDrawingScale = 0.5f;
        private const float k_LivesDrawingOpacity = 0.5f;
        private static int s_SpaceshipsCounter;
        private readonly int r_SpaceshipIndex;
        private readonly Gun r_Gun;
        private readonly Vector2 r_ShootingDirectionVector = new Vector2(0, -1);

        private int m_Score;
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

        public Color BulletsColor { get; } = Color.Red;
        protected IInputManager InputManager { get; private set; }
        public int Lives { get; set; }
        public abstract Color ScoreColor { get; }
        public abstract String Name { get; set; }

        public Spaceship(string k_AssetName, Game i_Game) : base(k_AssetName, i_Game)
        {
            r_Gun = new Gun(this, k_MaxBulletsInScreen);
            Lives = k_StartingLivesCount;
            m_Score = 0;
            r_SpaceshipIndex = s_SpaceshipsCounter;
            s_SpaceshipsCounter++;
        }

        public override void Initialize()
        {
            InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
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
                m_Velocity.X = k_VelocityScalar * -1;
            }
            else if (MoveRightDetected())
            {
                m_Velocity.X = k_VelocityScalar;
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
            r_Gun.Shoot(r_ShootingDirectionVector);
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            drawLives();
        }

        private void drawLives()
        {
            Vector2 currentPosition = LivesDrawingStartingPosition;
            Color livesDrawingColor = LivesDrawingColor;

            for (int i = 0; i < Lives; i++)
            {
                m_SpriteBatch.Draw(Texture, currentPosition, null, livesDrawingColor, Rotation, PositionOrigin, k_LivesDrawingScale, SpriteEffects.None, 0);
                currentPosition.X += LivesDrawingOffset.X;
            }
        }

        // Injection points for modifying the drawing of the lives
        protected virtual Vector2 LivesDrawingStartingPosition
        {
            get
            {
                return new Vector2(Game.GraphicsDevice.Viewport.Width + LivesDrawingOffset.X, LivesDrawingOffset.Y * (0.5f) + r_SpaceshipIndex * LivesDrawingOffset.Y);
            }
        }

        protected virtual Color LivesDrawingColor
        {
            get
            {
                return new Color(TintColor, k_LivesDrawingOpacity);
            }
        }

        protected virtual Vector2 LivesDrawingOffset
        {
            get
            {
                return new Vector2(-k_LivesDrawingGapModifier * this.Width, k_LivesDrawingGapModifier * this.Height);
            }
        }


    }
}