using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class CollisionHandler : GameService, ICollisionHandler
    {
        private const int k_ChanceToDestroyEnemyBullet = 50;
        private readonly Queue<Sprite> r_KillQueue;        
        public event Action EnemyCollidedWithSpaceship;
        private readonly Random r_RandomGenerator;

        public CollisionHandler(Game i_Game) : base(i_Game)
        {
            r_KillQueue = new Queue<Sprite>();
            r_RandomGenerator = new Random((int)DateTime.Now.Ticks);
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ICollisionHandler), this);
        }

        public void HandleCollision(ICollidable i_CollidableA, ICollidable i_CollidableB)
        {
            if(i_CollidableA is ICollidable2D && i_CollidableB is ICollidable2D)
            {
                ICollidable2D collidableA = i_CollidableA as ICollidable2D;
                ICollidable2D collidableB = i_CollidableB as ICollidable2D;

                handleCollisionForPermutation(collidableA, collidableB);
                handleCollisionForPermutation(collidableB, collidableA);
            }
        }

        private void handleCollisionForPermutation(ICollidable2D i_CollidableA, ICollidable2D i_CollidableB)
        {
            if (i_CollidableA is Bullet && i_CollidableB is Sprite)
            {
                handleBulletHitsSprite(i_CollidableA as Bullet, i_CollidableB as Sprite);
            }

            else if (i_CollidableA is Invader && i_CollidableB is Spaceship)
            {
                handleInvaderCollidedWithSpaceship(i_CollidableA as Invader, i_CollidableB as Spaceship);
            }

            else if(i_CollidableA is Invader && i_CollidableB is Barrier)
            {
                handleInvaderCollidedWithBarrier(i_CollidableA as Invader, i_CollidableB as Barrier);
            }
        }

        private void handleBulletHitsSprite(Bullet i_Bullet, Sprite i_Sprite)
        {
            if (!r_KillQueue.Contains(i_Bullet) && !r_KillQueue.Contains(i_Sprite))
            {
                if (i_Sprite is Bullet)
                {
                    handleBulletHitsBullet(i_Bullet, i_Sprite as Bullet);
                }

                else if (i_Sprite is IEnemy)
                {
                    handleBulletHitsEnemy(i_Bullet, i_Sprite as IEnemy);
                }

                else if (i_Sprite is Spaceship)
                {
                    handleBulletHitsSpaceship(i_Bullet, i_Sprite as Spaceship);
                }

                else if(i_Sprite is Barrier)
                {
                    handleBulletHitsBarrier(i_Bullet, i_Sprite as Barrier);
                }
            }
        }

        private void handleBulletHitsBarrier(Bullet i_Bullet, Barrier i_Barrier)
        {
            i_Barrier.ReceiveBulletDamage(i_Bullet);
            r_KillQueue.Enqueue(i_Bullet);
        }

        private void handleBulletHitsBullet(Bullet i_BulletA, Bullet i_BulletB)
        {
            if (i_BulletA.Shooter is IEnemy && i_BulletB.Shooter is Spaceship)
            {
                r_KillQueue.Enqueue(i_BulletB);

                if(r_RandomGenerator.Next(1, 100) <= k_ChanceToDestroyEnemyBullet)
                {
                    r_KillQueue.Enqueue(i_BulletA);
                }
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

        private void handleInvaderCollidedWithSpaceship(Invader i_Enemy, Spaceship i_Spaceship)
        {
            EnemyCollidedWithSpaceship.Invoke();
        }

        private void handleInvaderCollidedWithBarrier(Invader i_Invader, Barrier i_Barrier)
        {
            i_Barrier.ErasePixelsThatIntersectWith(i_Invader);
        }

        public override void Update(GameTime gameTime)
        {
            killComponentsInQueue();
            base.Update(gameTime);
        }

        private void killComponentsInQueue()
        {
            foreach (Sprite sprite in r_KillQueue)
            {
                sprite.Kill();
            }
            r_KillQueue.Clear();
        }
    }
}
