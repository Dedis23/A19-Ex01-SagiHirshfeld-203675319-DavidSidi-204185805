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
        private static readonly String sr_SourceFileURL = @"Sprites\Ship01_32x32";
        GameTime m_LastRecordedGameTime;

        public Spaceship(Game i_Game) : base(i_Game, sr_SourceFileURL)
        {
            Velocity = 120;
        }

        public override void Initialize()
        {            
            base.Initialize();
        }

        protected override Vector2 GetDefaultPosition()
        {
            // Get the bottom and center:
            float x = (float)GraphicsDevice.Viewport.Width / 2;
            float y = (float)GraphicsDevice.Viewport.Height;

            // Offset:
            x -= Texture.Width / 2;
            y -= Texture.Height / 2;

            // Put it a little bit higher:
            y -= 30;

            return new Vector2(x, y);
        }
        
        public override void Update(GameTime i_GameTime)
        {
            m_LastRecordedGameTime = i_GameTime;
            base.Update(i_GameTime);
        }

        public void MoveRight()
        {
            float x = this.Position.X + (float)m_LastRecordedGameTime.ElapsedGameTime.TotalSeconds * Velocity;
            x = MathHelper.Clamp(x, 0, Game.GraphicsDevice.Viewport.Width - Texture.Width);
            Position = new Vector2(x, this.Position.Y);
        }

        public void MoveLeft()
        {
            float x = this.Position.X - (float)m_LastRecordedGameTime.ElapsedGameTime.TotalSeconds * Velocity;
            x = MathHelper.Clamp(x, 0, Game.GraphicsDevice.Viewport.Width - Texture.Width);
            Position = new Vector2(x, this.Position.Y);
        }

        public void MoveAccordingToMousePositionDelta(Vector2 i_MousePositionDelta)
        {
            float x = this.Position.X + i_MousePositionDelta.X;
            x = MathHelper.Clamp(x, 0, Game.GraphicsDevice.Viewport.Width - Texture.Width);
            Position = new Vector2(x, this.Position.Y);
        }

        public void FireBullet()
        {
            // TODO: BOOM
        }
    }
}
