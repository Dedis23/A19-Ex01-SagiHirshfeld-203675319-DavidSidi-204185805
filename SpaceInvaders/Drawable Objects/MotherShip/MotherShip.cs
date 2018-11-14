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
    public class MotherShip : Drawable2DGameComponent, ICollideable
    {
        private const int k_MotherShipVelocity = 110;
        private const int k_MotherShipPointsValue = 850;
        public int PointsValue { get; set; }
        public event Action MotherShipLeftTheScreen;
#pragma warning disable CS0067 // The event 'MotherShip.MotherShipDestroyed' is never used
        public event Action MotherShipDestroyed;
#pragma warning restore CS0067 // The event 'MotherShip.MotherShipDestroyed' is never used

        public MotherShip(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
        {
            this.Tint = Color.Red;
            this.Velocity = k_MotherShipVelocity;
            PointsValue = k_MotherShipPointsValue;
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
            if (this.m_Position.X >= this.GraphicsDevice.Viewport.Width)
            {
                MotherShipLeftTheScreen?.Invoke();
            }
            else
            {
                m_Position.X += (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
            }
        }

        public void setDefaultPosition()
        {
            // Default MotherShip position (coming from the left of the screen)
            float x = -(float)this.Texture.Width;
            float y = (float)this.Texture.Height;

            m_Position = new Vector2(x, y);
        }
    }
}