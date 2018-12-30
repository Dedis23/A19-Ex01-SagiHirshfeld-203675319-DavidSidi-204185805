using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public class Mothership : Sprite, ICollideable, IEnemy
    {
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const int k_MotherShipVelocity = 110;
        private const int k_MotherShipPointsValue = 850;

        public int PointsValue { get; set; }

        public Mothership(Game i_Game) : base(k_AssetName, i_Game)
        {
            this.TintColor = Color.Red;
            this.Velocity = new Vector2(k_MotherShipVelocity, 0);
            PointsValue = k_MotherShipPointsValue;
            setDefaultPosition();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (this.Position.X >= this.GraphicsDevice.Viewport.Width)
            {
                this.Kill();
            }
        }

        private void setDefaultPosition()
        {
            // Default MotherShip position (coming from the left of the screen)
            float x = -(float)this.Texture.Width;
            float y = (float)this.Texture.Height;
            this.Position = new Vector2(x, y);
        }
    }
}