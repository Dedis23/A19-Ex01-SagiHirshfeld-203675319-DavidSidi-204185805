using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Mothership : Drawable2DGameComponent, ICollideable, IEnemy
    {
        private const int k_MotherShipVelocity = 110;
        private const int k_MotherShipPointsValue = 850;

        public int PointsValue { get; set; }

        public Mothership(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
        {
            this.Tint = Color.Red;
            this.Velocity = k_MotherShipVelocity;
            PointsValue = k_MotherShipPointsValue;
            setDefaultPosition();
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
            if (this.PositionX >= this.GraphicsDevice.Viewport.Width)
            {
                this.Kill();
            }
            else
            {
                PositionX += (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
            }
        }

        private void setDefaultPosition()
        {
            // Default MotherShip position (coming from the left of the screen)
            float x = -(float)this.Texture.Width;
            float y = (float)this.Texture.Height;
            this.PositionX = x;
            this.PositionY = y;
        }
    }
}