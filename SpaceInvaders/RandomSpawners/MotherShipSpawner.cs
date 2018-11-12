using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class MotherShipSpawner : GameComponent
    {
        private const int k_ChanceToSpawn = 10;
        private const float k_TimeBetweenRolls = 1;
        private RandomSpawner m_RandomSpawner;
        private MotherShip m_CurrentMotherShip;
        private Game m_CurrentGame;
        public bool Active { get; private set; }
        public event Action<MotherShip> MotherShipSpawned;
        public event Action<MotherShip> MotherShipDeSpawned;

        public MotherShipSpawner(Game i_Game) : base(i_Game)
        {
            m_RandomSpawner = new RandomSpawner(i_Game, k_ChanceToSpawn, k_TimeBetweenRolls);
            m_RandomSpawner.ObjectSpawned += OnObjectSpawned;
            m_CurrentGame = i_Game;
            Active = true;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.Active == true)
            {
                m_RandomSpawner.RollForSpawn(i_GameTime);
            }
        }

        public void OnObjectSpawned(object i_Source, EventArgs i_EventArgs)
        {
            // Its time to create the mothership
            Active = false;
            m_CurrentMotherShip = DrawableObjectsFactory.Create(m_CurrentGame, DrawableObjectsFactory.eSpriteType.MotherShip) as MotherShip;
            m_CurrentMotherShip.MotherShipLeftTheScreen += OnMotherShipLeftTheScreen;
            m_CurrentMotherShip.MotherShipDestroyed += OnMotherShipDestroyed;
            // Calling listeners to let them know about the new
            MotherShipSpawned?.Invoke(m_CurrentMotherShip);
        }

        public void OnMotherShipLeftTheScreen(object i_Source, EventArgs i_EventArgs)
        {
            killMotherShipAndNotify();
        }

        public void OnMotherShipDestroyed(object i_Source, EventArgs i_EventArgs)
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
            Active = true;
        }
    }
}