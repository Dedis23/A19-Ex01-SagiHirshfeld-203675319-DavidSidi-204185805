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
        private readonly Stack<Bullet> r_AvailableBulletsForDeploymentsStack;
        private readonly Dictionary<Bullet, GameScreen> r_FlyingBulletsToContainingScreensDictionary;


        public BulletsFactory(Game i_Game) : base(i_Game)
        {
            r_AvailableBulletsForDeploymentsStack = new Stack<Bullet>();
            r_FlyingBulletsToContainingScreensDictionary = new Dictionary<Bullet, GameScreen>();
            r_GameScreensManager = Game.Services.GetService(typeof(IScreensMananger)) as IScreensMananger;
        }

        private GameScreen CurrentlyActiveScreen
        {
            get
            {
                return r_GameScreensManager.ActiveScreen;
            }
        }

        public Bullet GetBullet()
        {
            Bullet newBullet;

            if (r_AvailableBulletsForDeploymentsStack.Count != 0)
            {
                newBullet = r_AvailableBulletsForDeploymentsStack.Pop();
            }

            else
            {
                newBullet = new Bullet(this.Game);
                newBullet.Died += onBulletDestroyed;
            }

            r_FlyingBulletsToContainingScreensDictionary.Add(newBullet, CurrentlyActiveScreen);
            CurrentlyActiveScreen.Add(newBullet); 

            return newBullet;
        }

        private void onBulletDestroyed(object i_Bullet)
        {
            Bullet bullet = i_Bullet as Bullet;
            if (r_FlyingBulletsToContainingScreensDictionary.ContainsKey(bullet))
            {
                GameScreen ScreenBulletIsIn = r_FlyingBulletsToContainingScreensDictionary[bullet];
                ScreenBulletIsIn.Remove(bullet);
                r_FlyingBulletsToContainingScreensDictionary.Remove(bullet);
                r_AvailableBulletsForDeploymentsStack.Push(bullet);
            }
        }
    }
}