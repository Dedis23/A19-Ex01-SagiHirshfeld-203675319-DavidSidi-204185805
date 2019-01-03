using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public sealed class BulletsFactory : GameService
    {
        Stack<Bullet> m_BulletsStack = new Stack<Bullet>();

        public BulletsFactory(Game i_Game) : base(i_Game)
        {
        }

        public Bullet GetBullet()
        {
            Bullet newBullet;

            if (m_BulletsStack.Count != 0)
            {
                newBullet = m_BulletsStack.Pop();
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
            m_BulletsStack.Push(i_Bullet as Bullet);
        }
    }
}