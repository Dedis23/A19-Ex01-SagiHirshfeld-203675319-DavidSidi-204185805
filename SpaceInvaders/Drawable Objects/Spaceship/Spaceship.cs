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

        public Spaceship(Game i_Game) : base(i_Game, sr_SourceFileURL)
        {
        }

        public override void Initialize()
        {            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
    }
}
