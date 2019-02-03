using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public sealed class BulletsFactory : GameService
    {
        private readonly IScreensMananger r_GameScreensManager;
        private readonly Stack<Bullet> r_BulletsStack;

        public BulletsFactory(Game i_Game) : base(i_Game)
        {
            r_BulletsStack = new Stack<Bullet>();
            r_GameScreensManager = Game.Services.GetService(typeof(IScreensMananger)) as IScreensMananger;
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
                newBullet.Died += onBulletDestroyed;
            }

            r_GameScreensManager.ActiveScreen.Remove(newBullet);
            r_GameScreensManager.ActiveScreen.Add(newBullet);
            return newBullet;
        }

        private void onBulletDestroyed(object i_Bullet)
        {
            Bullet bullet = i_Bullet as Bullet;
            r_BulletsStack.Push(bullet);
        }
    }
}