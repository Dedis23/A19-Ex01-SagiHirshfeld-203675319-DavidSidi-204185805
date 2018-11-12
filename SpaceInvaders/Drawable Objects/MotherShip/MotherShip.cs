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
    public class MotherShip : Drawable2DGameComponent
    {
        public event EventHandler MotherShipLeftTheScreen;
        public event EventHandler MotherShipDestroyed;

        public MotherShip(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
        {
            this.Tint = Color.Red;
            this.Velocity = 110;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            moveMotherShip(gameTime);
        }

        private void moveMotherShip(GameTime i_GameTime)
        {
            if (this.Visible == true)
            {
                if (this.Position.X >= this.GraphicsDevice.Viewport.Width)
                {
                    MotherShipLeftTheScreen?.Invoke(this, EventArgs.Empty);
                }

                else
                {
                    m_Position.X += (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
                }
            }
        }

        protected override Vector2 GetDefaultPosition()
        {
            // Default MotherShip position (coming from the left of the screen)
            float x = -(float)this.Texture.Width;
            float y = (float)this.Texture.Height;

            return new Vector2(x, y);
        }
    }
}