using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public sealed class BulletsFactory : GameService
    {
        private readonly Stack<Bullet> r_BulletsStack;

        public BulletsFactory(Game i_Game) : base(i_Game)
        {
            r_BulletsStack = new Stack<Bullet>();
        }

        public Bullet GetBullet()
        {
            Bullet newBullet;

            if (r_BulletsStack.Count != 0)
            {
                newBullet = r_BulletsStack.Pop();
            }
            else
            {
                newBullet = new Bullet(this.Game);
                newBullet.SpriteKilled += onBulletDestroyed;
            }

            return newBullet;
        }

        private void onBulletDestroyed(object i_Bullet)
        {
            r_BulletsStack.Push(i_Bullet as Bullet);
        }
    }
}