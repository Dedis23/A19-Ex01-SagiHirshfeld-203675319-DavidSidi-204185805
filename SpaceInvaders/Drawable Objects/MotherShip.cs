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
    class MotherShip : Drawable2DGameComponent
    {
        private static readonly String sr_SourceFileURL = @"Sprites\MotherShip_32x120";
        private const int k_ChanceToSpawn = 10;
        private const float k_DelayBetweenRolls = 1;
        private float m_RemainingDelay;
        private Random m_RandomGenerator;

        public MotherShip(Game i_Game) : base(i_Game, sr_SourceFileURL)
        {
            this.Tint = Color.Red;
            this.Velocity = 110;
            m_RemainingDelay = k_DelayBetweenRolls;
            m_RandomGenerator = new Random();
            hideMotherShip();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Spawn the MotherShip at a fixed spawning chance, every second we try to spawn it in "k_ChanceToSpawn" chance
            if (tryToSpawnTheMothership(gameTime) == true)
            {
                spawnMotherShip();
            }
            moveMotherShip(gameTime);
        }

        private void moveMotherShip(GameTime i_GameTime)
        {
            if (this.Visible == true)
            {
                if (this.Position.X >= this.GraphicsDevice.Viewport.Width)
                {
                    hideMotherShip();
                    this.Position = GetDefaultPosition();
                }
                else
                {
                    this.Position = new Vector2(this.Position.X + (float)i_GameTime.ElapsedGameTime.TotalSeconds * Velocity,
                        this.Position.Y);
                }
            }
        }

        private void spawnMotherShip()
        {
            this.Visible = true;
        }

        private void hideMotherShip()
        {
            this.Visible = false;
        }

        private bool tryToSpawnTheMothership(GameTime i_GameTime)
        {
            bool didMotherShipSpawned = false;
            // To make the roll question tied to time and to the framerate we use m_RemainingDelay and k_DelayBetweenRolls
            // this way, we make sure we try to spawn the MotherShip at a fixed delay time
            // Currently it is set to 10% chance and we roll each second
            m_RemainingDelay -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_RemainingDelay < 0)
            {
                if (this.Visible == false && (m_RandomGenerator.Next(100) + 1) <= k_ChanceToSpawn)
                {
                    didMotherShipSpawned = true;
                }
                m_RemainingDelay = k_DelayBetweenRolls;
            }

            return didMotherShipSpawned;
        }

        protected override Vector2 GetDefaultPosition()
        {
            // Default MotherShip position (coming from the left of the screen)
            float x = -(float)this.Texture.Width;
            float y = (float)this.Texture.Height;

            return new Vector2(x, y);
        }
    }
}