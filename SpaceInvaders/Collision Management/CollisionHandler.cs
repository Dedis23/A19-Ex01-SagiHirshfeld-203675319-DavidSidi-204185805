using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class CollisionHandler : GameService, ICollisionHandler
    {
        private readonly Queue<Sprite> r_KillQueue;        
        public event Action EnemyCollidedWithSpaceship;

        public CollisionHandler(Game i_Game) : base(i_Game)
        {
            r_KillQueue = new Queue<Sprite>();
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ICollisionHandler), this);
        }

        public void HandleCollision(ICollidable i_CollideableA, ICollidable i_CollideableB)
        {
            if(i_CollideableA is ICollidable2D && i_CollideableB is ICollidable2D)
            {
                ICollidable2D collidableA = i_CollideableA as ICollidable2D;
                ICollidable2D collidableB = i_CollideableB as ICollidable2D;

                handleCollisionForPermutation(collidableA, collidableB);
                handleCollisionForPermutation(collidableB, collidableA);
            }
        }

        private void handleCollisionForPermutation(ICollidable2D i_CollideableA, ICollidable2D i_CollideableB)
        {
            if (i_CollideableA is Bullet && i_CollideableB is Sprite)
            {
                handleBulletHitsKillable(i_CollideableA as Bullet, i_CollideableB as Sprite);
            }

            else if (i_CollideableA is Invader && i_CollideableB is Spaceship)
            {
                handleEnemyHitsSpaceship(i_CollideableA as Invader, i_CollideableB as Spaceship);
            }
        }

        private void handleBulletHitsKillable(Bullet i_Bullet, Sprite i_Killable)
        {
            if (!r_KillQueue.Contains(i_Bullet) && !r_KillQueue.Contains(i_Killable))
            {
                if (i_Killable is Bullet)
                {
                    handleBulletHitsBullet(i_Bullet, i_Killable as Bullet);
                }

                else if (i_Killable is IEnemy)
                {
                    handleBulletHitsEnemy(i_Bullet, i_Killable as IEnemy);
                }

                else if (i_Killable is Spaceship)
                {
                    handleBulletHitsSpaceship(i_Bullet, i_Killable as Spaceship);
                }
            }
        }

        private void handleBulletHitsBullet(Bullet i_BulletA, Bullet i_BulletB)
        {
            if (i_BulletA.Shooter is IEnemy && i_BulletB.Shooter is Spaceship)
            {
                r_KillQueue.Enqueue(i_BulletA);
                r_KillQueue.Enqueue(i_BulletB);
            }
        }

        private void handleBulletHitsEnemy(Bullet i_Bullet, IEnemy i_Enemy)
        {
            if (i_Bullet.Shooter is Spaceship)
            {
                r_KillQueue.Enqueue(i_Bullet);
                if(i_Enemy is Sprite)
                {
                    r_KillQueue.Enqueue(i_Enemy as Sprite);
                }
                
                (i_Bullet.Shooter as Spaceship).Score += i_Enemy.PointsValue;
            }
        }

        private void handleBulletHitsSpaceship(Bullet i_Bullet, Spaceship i_Spaceship)
        {
            r_KillQueue.Enqueue(i_Bullet);
            i_Spaceship.TakeBulletHit();
        }

        private void handleEnemyHitsSpaceship(Invader i_Enemy, Spaceship i_Spaceship)
        {
            EnemyCollidedWithSpaceship.Invoke();
        }

        public override void Update(GameTime gameTime)
        {
            killComponentsInQueue();
            base.Update(gameTime);
        }

        private void killComponentsInQueue()
        {
            foreach (Sprite killableComponent in r_KillQueue)
            {
                killableComponent.Kill();
            }

            r_KillQueue.Clear();
        }
    }
}
