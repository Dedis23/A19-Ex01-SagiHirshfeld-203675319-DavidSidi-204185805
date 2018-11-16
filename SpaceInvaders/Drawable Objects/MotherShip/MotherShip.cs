using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Mothership : Drawable2DGameComponent, ICollideable
    {
        private const int k_MotherShipVelocity = 110;
        private const int k_MotherShipPointsValue = 850;

        public int PointsValue { get; set; }

        public event Action MothershipLeftTheScreen;

        public event Action MothershipDestroyed;

        public Mothership(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
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
            if (this.PositionX >= this.GraphicsDevice.Viewport.Width)
            {
                MothershipLeftTheScreen?.Invoke();
            }
            else
            {
                PositionX += (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
            }
        }

        public void NotifyDestruction()
        {
            MothershipDestroyed?.Invoke();
        }

        public void setDefaultPosition()
        {
            // Default MotherShip position (coming from the left of the screen)
            float x = -(float)this.Texture.Width;
            float y = (float)this.Texture.Height;
            this.PositionX = x;
            this.PositionY = y;
        }
    }
}