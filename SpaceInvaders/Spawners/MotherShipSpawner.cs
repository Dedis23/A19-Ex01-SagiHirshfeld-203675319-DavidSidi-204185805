using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class MothershipSpawner : GameComponent
    {
        private const int k_ChanceToSpawn = 10;
        private const float k_TimeBetweenRolls = 1;
        private readonly Spawner r_Spawner;
        private Mothership m_CurrentMotherShip;

        public MothershipSpawner(Game i_Game) : base(i_Game)
        {
            r_Spawner = new Spawner(i_Game, k_ChanceToSpawn, k_TimeBetweenRolls);
            r_Spawner.Spawned += spawnMotherShip;
            r_Spawner.Activate();
        }

        public void spawnMotherShip()
        {
            m_CurrentMotherShip = DrawableObjectsFactory.Create(Game, DrawableObjectsFactory.eSpriteType.Mothership) as Mothership;
            m_CurrentMotherShip.Killed += OnMothershipKilled;
            this.Game.Components.Add(m_CurrentMotherShip);
            r_Spawner.DeActivate();
        }

        public void OnMothershipKilled(object i_Mothership)
        {
            m_CurrentMotherShip = null;
            r_Spawner.Activate();
        }
    }
}