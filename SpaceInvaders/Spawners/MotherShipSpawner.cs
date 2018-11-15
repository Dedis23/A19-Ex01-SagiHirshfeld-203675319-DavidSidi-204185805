using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class MotherShipSpawner : GameComponent
    {
        private const int k_ChanceToSpawn = 10;
        private const float k_TimeBetweenRolls = 1;
        private Spawner m_RollingMachine;
        private MotherShip m_CurrentMotherShip;
        public event Action<MotherShip> MotherShipSpawned;
        public event Action<MotherShip> MotherShipDeSpawned;

        public MotherShipSpawner(Game i_Game) : base(i_Game)
        {
            m_RollingMachine = new Spawner(i_Game, k_ChanceToSpawn, k_TimeBetweenRolls);
            m_RollingMachine.Spawned += spawnMotherShip;
            m_RollingMachine.Activate();
        }

        public void spawnMotherShip()
        {
            m_CurrentMotherShip = DrawableObjectsFactory.Create(Game, DrawableObjectsFactory.eSpriteType.MotherShip) as MotherShip;
            m_CurrentMotherShip.MotherShipLeftTheScreen += OnMotherShipLeftTheScreen;
            m_CurrentMotherShip.MotherShipDestroyed += OnMotherShipDestroyed;
            MotherShipSpawned?.Invoke(m_CurrentMotherShip);
            m_RollingMachine.DeActivate();
        }

        public void OnMotherShipLeftTheScreen()
        {
            killMotherShipAndNotify();
        }

        public void OnMotherShipDestroyed()
        {
            killMotherShipAndNotify();
            // TO DO MORE (for example, add points to player)
        }

        private void killMotherShipAndNotify()
        {
            // Calling listeners to let them know that the MotherShip is gone
            MotherShipDeSpawned?.Invoke(m_CurrentMotherShip);
            // Reactivating the RandomSpawner
            m_CurrentMotherShip = null;
            m_RollingMachine.Activate();
        }
    }
}