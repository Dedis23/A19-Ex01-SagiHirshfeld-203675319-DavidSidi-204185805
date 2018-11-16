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
            if (i_CollideableA.GetType() != i_CollideableB.GetType())
            {
                handleCollisionForPermutation(i_CollideableA, i_CollideableB);
                handleCollisionForPermutation(i_CollideableB, i_CollideableA);
            }
        }

        private void handleCollisionForPermutation(ICollideable i_CollideableA, ICollideable i_CollideableB)
        {
            if (i_CollideableA is Bullet && i_CollideableB is IKillable)
            {
                handleBulletHitsKillable(i_CollideableA as Bullet, i_CollideableB as IKillable);
            }

            if (i_CollideableA is Invader && i_CollideableB is Spaceship)
            {
                handleEnemyHitsSpaceship(i_CollideableA as Invader, i_CollideableB as Spaceship);
            }
        }

        private void handleBulletHitsKillable(Bullet i_Bullet, IKillable i_Killable)
        {
            // In Space Invaders a bullet shot by a certain creature won't do anything to similar creatures
            if (i_Bullet.Shooter.GetType() != i_Killable.GetType())
            {
                // When multiple bullets hits the same target - this makes sure only one bullet will register
                if (!r_KillQueue.Contains(i_Killable))
                {
                    r_KillQueue.Enqueue(i_Bullet);

                    if(i_Killable is Spaceship)
                    {
                        handleSpaceshipHitByBullet(i_Killable as Spaceship);
                    }

                    else if(i_Killable is Invader)
                    {
                        handleEnemyHitByBullet(i_Killable as Invader, i_Bullet);
                    }

                    else if(i_Killable is Mothership)
                    {
                        handleMothershipHitByBullet(i_Killable as Mothership, i_Bullet);
                    }
                }
            }
        }

        private void handleSpaceshipHitByBullet(Spaceship i_Spaceship)
        {
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

        // TODO: A need for polymorphism detected!
        private void handleEnemyHitByBullet(Invader i_Enemy, Bullet i_Bullet)
        {
            if (i_Bullet.Shooter is Spaceship)
            {
                (i_Bullet.Shooter as Spaceship).Score += i_Enemy.PointsValue;
            }

            r_KillQueue.Enqueue(i_Enemy);
        }

        // TODO: A need for polymorphism detected!
        private void handleMothershipHitByBullet(Mothership i_Mothership, Bullet i_Bullet)
        {
            if (i_Bullet.Shooter is Spaceship)
            {
                (i_Bullet.Shooter as Spaceship).Score += i_Mothership.PointsValue;
            }
            // Need to fix here, my fix makes exception
            // i_Mothership.NotifyDestruction();
            r_KillQueue.Enqueue(i_Mothership);
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
