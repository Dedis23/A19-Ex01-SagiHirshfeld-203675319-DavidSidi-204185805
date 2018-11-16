using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Spaceship : Drawable2DGameComponent, ICollideable
    {
        private const int k_SpaceShipVelocity = 120;
        private const int k_MaxBulletsInScreen = 3;
        private const int k_StartingLivesCount = 3;
        private int m_Score;
        private readonly Gun r_Gun;
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
            Velocity = k_SpaceShipVelocity;
            Lives = k_StartingLivesCount;
            m_Score = 0;
            r_Gun = new Gun(i_Game, this);
            SetDefaultPosition();
        }

        public void SetDefaultPosition()
        {
            // Get the bottom and center:
            float x = 0;
            float y = (float)GraphicsDevice.Viewport.Height;

            // Offset:
            y -= Texture.Height / 2;

            // Put it a little bit higher:
            y -= 32;

            m_Position = new Vector2(x, y);
        }

        public override void Update(GameTime i_GameTime)
        {
            // Clamp the position between screen boundries:
            m_Position.X = MathHelper.Clamp(m_Position.X, 0, Game.GraphicsDevice.Viewport.Width - Texture.Width);

            base.Update(i_GameTime);
        }

        public void MoveRight(GameTime i_GameTime)
        {
            m_Position.X += (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
        }

        public void MoveLeft(GameTime i_GameTime)
        {
            m_Position.X -= (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
        }

        public void MoveAccordingToMousePositionDelta(GameTime i_GameTime, Vector2 i_MousePositionDelta)
        {
            m_Position.X += i_MousePositionDelta.X;
        }

        public void Shoot(GameTime i_GameTime)
        {
            if (r_Gun.NumberOfShotBulletsInScreen < k_MaxBulletsInScreen)
            {
                Vector2 positionToShootFrom = new Vector2(this.Position.X + 0.5f * this.Width, this.Top - 1);
                r_Gun.Shoot(positionToShootFrom, eShootingDirection.Up, Color.Red);
            }
        }
    }
}