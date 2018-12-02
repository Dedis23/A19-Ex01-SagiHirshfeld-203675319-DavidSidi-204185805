using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class Spaceship : Sprite, ICollideable, IShooter
    {
        private const int k_FlightVelocity = 120;
        private const int k_MaxBulletsInScreen = 3;
        private const int k_StartingLivesCount = 3;
        private readonly Gun r_Gun;
        private int m_Score;
        public Color BulletsColor { get; } = Color.Red;

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

        public Spaceship(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
        {
            r_Gun = new Gun(this);
            Lives = k_StartingLivesCount;
            m_Score = 0;
            SetDefaultPosition();
        }

        public void SetDefaultPosition()
        {
            // Get the bottom and center:
            float x = 0;
            float y = (float)GraphicsDevice.Viewport.Height;

            // Offset:
            y -= Texture.Height * 1.5f;

            PositionX = x;
            PositionY = y;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            // Clamp the position between screen boundries:
            PositionX = MathHelper.Clamp(PositionX, 0, Game.GraphicsDevice.Viewport.Width - Texture.Width);

            // Reset the velocity after moving due to MoveLeft or MoveRight
            Velocity = Vector2.Zero;
        }

        public void MoveRight()
        {
            Velocity = new Vector2(k_FlightVelocity, 0);
        }

        public void MoveLeft()
        {
            Velocity = new Vector2(-k_FlightVelocity, 0);
        }

        public void MoveAccordingToMousePositionDelta(Vector2 i_MousePositionDelta)
        {
            PositionX += i_MousePositionDelta.X;
        }

        public void Shoot()
        {
            if (r_Gun.NumberOfShotBulletsInScreen < k_MaxBulletsInScreen)
            {
                r_Gun.Shoot(eDirection.Up);
            }
        }
    }
}