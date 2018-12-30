using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class Bullet : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\Bullet";
        private const int k_BulletsVelocity = 155;

        public object Shooter { get; set; }

        public Bullet(Game i_Game) : base(k_AssetName, i_Game)
        {
            Velocity = new Vector2(0, -k_BulletsVelocity);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!IsInScreen)
            {
                this.Kill();
            }

            base.Update(i_GameTime);
        }
    }
}
