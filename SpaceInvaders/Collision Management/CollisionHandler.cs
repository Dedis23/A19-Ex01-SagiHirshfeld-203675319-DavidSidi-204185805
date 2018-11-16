using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SpaceInvaders
{
    public class CollisionHandler : GameComponent
    {
        private readonly Queue<IKillable> r_KillQueue;
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

            if (i_CollideableA is Enemy && i_CollideableB is Spaceship)
            {
                handleEnemyHitsSpaceship(i_CollideableA as Enemy, i_CollideableB as Spaceship);
            }
        }

        private void handleBulletHitsKillable(Bullet i_Bullet, IKillable i_Killable)
        {
            // In Space Invaders a bullet shot by a certain creature won't do anything to similar creatures
            if (i_Bullet.TypeOfShooter != i_Killable.GetType())
            {
                // When multiple bullets hits the same target - this makes sure only one bullet will register
                if (!r_KillQueue.Contains(i_Killable))
                {
                    r_KillQueue.Enqueue(i_Bullet);
                    r_KillQueue.Enqueue(i_Killable);
                }
            }
        }

        private void handleEnemyHitsSpaceship(Enemy i_Enemy, Spaceship i_Spaceship)
        {
            EnemyCollidedWithSpaceship.Invoke();
        }

        public override void Update(GameTime gameTime)
        {
            // Remove components which were added to the removal queue
            foreach (IKillable killableComponent in r_KillQueue)
            {
                killableComponent.Kill();
            }

            r_KillQueue.Clear();
            base.Update(gameTime);
        }
    }
}

