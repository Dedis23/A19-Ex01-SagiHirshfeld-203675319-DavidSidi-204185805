using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class Bullet : Sprite, ICollideable
    {
        private const string k_AssetName = @"Sprites\Bullet";
        private const int k_BulletsVelocity = 155;

        public eDirection Direction { get; set; }
        public object Shooter { get; set; }

        public Bullet(Game i_Game, string i_SourceFileURL) : base(k_AssetName, i_Game)
        {
        }

        public override void Initialize()
        {
            float yVelocity = Direction == eDirection.Down ? k_BulletsVelocity : -k_BulletsVelocity;
            Velocity = new Vector2(0, yVelocity);
            base.Initialize();
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
