using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;

namespace SpaceInvaders
{
    public sealed class BulletsFactory : GameService
    {
        private readonly GameScreen r_GameScreen;
        private readonly Stack<Bullet> r_BulletsStack;

        public BulletsFactory(GameScreen i_GameScreen) : base(i_GameScreen.Game)
        {
            r_GameScreen = i_GameScreen;
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
                r_GameScreen.Add(newBullet);
                newBullet.Died += onBulletDestroyed;
            }

            return newBullet;
        }

        private void onBulletDestroyed(object i_Bullet)
        {
            r_BulletsStack.Push(i_Bullet as Bullet);
        }
    }
}