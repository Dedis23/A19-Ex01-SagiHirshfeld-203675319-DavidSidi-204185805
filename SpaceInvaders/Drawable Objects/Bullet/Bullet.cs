using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class Bullet : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\Bullet";
        public static Vector2 FlyingVelocity { get; } = new Vector2(0, 155);

        public object Shooter { get; set; }

        public Bullet(Game i_Game) : base(k_AssetName, i_Game)
        {
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!IsInScreen)
            {
                this.Kill();
            }

            base.Update(i_GameTime);
        }

        protected override void KilledInjectionPoint()
        {
            this.Visible = false;
            this.Enabled = false;
        }
    }
}
