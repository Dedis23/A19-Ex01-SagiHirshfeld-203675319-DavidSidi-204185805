using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class CollisionHandler : GameComponent
    {
        private readonly Queue<IKillable> r_KillQueue;
        private int k_ScorePenaltyForBulletHit = 1100;
        public event Action EnemyCollidedWithSpaceship;
        public CollisionHandler(Game i_Game) : base(i_Game)
        {
            r_KillQueue = new Queue<IKillable>();
        }

        public void HandleCollision(ICollideable i_CollideableA, ICollideable i_CollideableB)
        {
            handleCollisionForPermutation(i_CollideableA, i_CollideableB);
            handleCollisionForPermutation(i_CollideableB, i_CollideableA);
        }

        private void handleCollisionForPermutation(ICollideable i_CollideableA, ICollideable i_CollideableB)
        {
            if (i_CollideableA is Bullet && i_CollideableB is IKillable)
            {
                handleBulletHitsKillable(i_CollideableA as Bullet, i_CollideableB as IKillable);
            }

            else if (i_CollideableA is Invader && i_CollideableB is Spaceship)
            {
                handleEnemyHitsSpaceship(i_CollideableA as Invader, i_CollideableB as Spaceship);
            }
        }

        private void handleBulletHitsKillable(Bullet i_Bullet, IKillable i_Killable)
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
            if(i_BulletA.Shooter is IEnemy && i_BulletB.Shooter is Spaceship)
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
                r_KillQueue.Enqueue(i_Enemy);
                (i_Bullet.Shooter as Spaceship).Score += i_Enemy.PointsValue;
            }
        }

        private void handleBulletHitsSpaceship(Bullet i_Bullet, Spaceship i_Spaceship)
        {
            r_KillQueue.Enqueue(i_Bullet);

            i_Spaceship.Lives--;
            i_Spaceship.Score -= k_ScorePenaltyForBulletHit;

            if (i_Spaceship.Lives == 0)
            {
                r_KillQueue.Enqueue(i_Spaceship);
            }

            else
            {
                i_Spaceship.SetDefaultPosition();
            }
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
            foreach (IKillable killableComponent in r_KillQueue)
            {
                killableComponent.Kill();
            }

            r_KillQueue.Clear();
        }
    }
}
