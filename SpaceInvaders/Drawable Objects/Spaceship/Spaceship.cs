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
        private const int k_BulletsVelocity = 120;
        private Gun m_Gun;

        public Spaceship(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
        {
            Velocity = k_SpaceShipVelocity;
            m_Gun = new Gun(i_Game);
            SetDefaultPosition();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void SetDefaultPosition()
        {
            // Get the bottom and center:
            float x = (float)GraphicsDevice.Viewport.Width / 2;
            float y = (float)GraphicsDevice.Viewport.Height;

            // Offset:
            x -= Texture.Width / 2;
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
            Vector2 positionToShootFrom = new Vector2(this.Position.X + 0.5f * this.Width, this.Top -  1);
            m_Gun.Shoot(positionToShootFrom, eShootingDirection.Up, k_BulletsVelocity, Color.Red);
        }
    }
}