using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class Mothership : Sprite, ICollidable2D, IEnemy
    {
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const int k_MotherShipVelocity = 110;
        private const int k_MotherShipPointsValue = 850;

        public int PointsValue { get; set; }

        public Mothership(Game i_Game) : base(k_AssetName, i_Game)
        {
            this.TintColor = Color.Red;
            this.Velocity = Vector2.Zero;
            this.Visible = false;
            this.Vulnerable = true;
            PointsValue = k_MotherShipPointsValue;
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            setDefaultPosition();
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
            this.Position = new Vector2(-this.Width, this.Height);
        }

        public void SpawnAndFly()
        {
            setDefaultPosition();
            this.Vulnerable = true;
            this.Visible = true;
            this.Velocity = new Vector2(k_MotherShipVelocity, 0);
        }

        protected override void KilledInjectionPoint()
        {
            this.Visible = false;
            this.Velocity = Vector2.Zero;
        }
    }
}