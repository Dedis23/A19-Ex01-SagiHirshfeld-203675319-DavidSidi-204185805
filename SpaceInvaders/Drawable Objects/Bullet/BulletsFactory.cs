using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    public sealed class BulletsFactory
    {
        private static BulletsFactory s_Instance;

        private static readonly object sr_CreationLock = new object();
        public static BulletsFactory Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (sr_CreationLock)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new BulletsFactory();
                        }
                    }
                }

                return s_Instance;
            }
        }

        Stack<Bullet> m_BulletsStack = new Stack<Bullet>();

        public Bullet GetBulletForShooter(IShooter i_Shooter)
        {
            Bullet newBullet;

            if (m_BulletsStack.Count != 0)
            {
                newBullet = m_BulletsStack.Pop();
                newBullet.Visible = true;
                newBullet.Enabled = true;
            }
            else
            {
                newBullet = new Bullet(i_Shooter.Game);
                newBullet.SpriteKilled += onBulletDestroyed;
            }

            return newBullet;
        }

        private void onBulletDestroyed(object i_Bullet)
        {
            Bullet bullet = i_Bullet as Bullet;
            bullet.Velocity = Vector2.Zero;
            m_BulletsStack.Push(bullet as Bullet);
        }
    }
}