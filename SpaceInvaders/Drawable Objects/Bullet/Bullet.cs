using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class Bullet : Drawable2DGameComponent, ICollideable
    {
        private const int k_BulletsVelocity = 155;
        public eDirection Direction { get; set; }
        public object Shooter { get; set; }

        public Bullet(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
        {
            this.Velocity = k_BulletsVelocity;
        }

        public override void Update(GameTime i_GameTime)
        {
            float yDelta = (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity;
            PositionY = Direction == eDirection.Up ? PositionY - yDelta : PositionY + yDelta;

            if (!IsInScreen)
            {
                this.Kill();
            }

            base.Update(i_GameTime);
        }
    }
}
