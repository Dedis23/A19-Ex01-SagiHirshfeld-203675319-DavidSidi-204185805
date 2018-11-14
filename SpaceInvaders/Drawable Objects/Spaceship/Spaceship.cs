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
    class Spaceship : Drawable2DGameComponent
    {
        private const int k_SpaceShipVelocity = 120;
        public Spaceship(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
        {
            Velocity = k_SpaceShipVelocity;
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

            Position = new Vector2(x, y);
        }

        public override void Update(GameTime i_GameTime)
        {
            // Clamp the position between screen boundries:
            Position.X = MathHelper.Clamp(Position.X, 0, Game.GraphicsDevice.Viewport.Width - Texture.Width);

            base.Update(i_GameTime);
        }

        public void MoveRight(GameTime i_GameTime)
        {
            Position.X += (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
        }

        public void MoveLeft(GameTime i_GameTime)
        {
            Position.X -= (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
        }

        public void MoveAccordingToMousePositionDelta(GameTime i_GameTime, Vector2 i_MousePositionDelta)
        {
            Position.X += i_MousePositionDelta.X;
        }

        public void FireBullet(GameTime i_GameTime)
        {
            // TODO: BOOM
        }
    }
}