using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Bullet : Sprite, ICollideable
    {
        private const int k_BulletsVelocity = 155;
        public eDirection Direction { get; set; }
        public object Shooter { get; set; }

        public Bullet(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
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
