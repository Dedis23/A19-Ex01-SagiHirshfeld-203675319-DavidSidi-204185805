using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class MothershipSpawner
    {
        private const int k_ChanceToSpawn = 10;
        private const float k_TimeBetweenRolls = 1;
        private readonly Spawner r_Spawner;
        private Mothership m_MotherShip;

        public MothershipSpawner(Game i_Game)
        {
            r_Spawner = new Spawner(i_Game, k_ChanceToSpawn, k_TimeBetweenRolls);            
            m_MotherShip = new Mothership(i_Game);

            r_Spawner.Spawned += checkAnimationStatusAndSpawn;
            m_MotherShip.SpriteKilled += OnMothershipKilled;
            r_Spawner.Activate();
        }

        private void checkAnimationStatusAndSpawn()
        {
            SpriteAnimator deathanimation = m_MotherShip.Animations["DeathAnimation"];
            if (deathanimation == null)
            {
                spawnMotherShip();
            }
            else if (deathanimation.Enabled == false)
            {
                spawnMotherShip();
            }
        }

        private void spawnMotherShip()
        {
            m_MotherShip.SpawnAndFly();
            r_Spawner.DeActivate();
        }

        public void OnMothershipKilled(object i_Mothership)
        {
            r_Spawner.Activate();
        }
    }
}