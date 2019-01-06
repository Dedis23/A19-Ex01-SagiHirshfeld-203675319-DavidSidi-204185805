using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    public class DancingBarriersRow : DrawableGameComponent
    {
        private const int k_DancingSpeed = 45;
        private const int k_DefaultBarrierNum = 4;
        private readonly SpriteRow<Barrier> r_SpritesRow;

        public DancingBarriersRow(Game i_Game, int i_BarrierNum) : base(i_Game)
        {
            r_SpritesRow = new SpriteRow<Barrier>(i_Game, i_BarrierNum, Game => new Barrier(i_Game));
            this.Game.Components.Add(this);
        }

        public DancingBarriersRow(Game i_Game) : this(i_Game, k_DefaultBarrierNum)
        { }

        protected override void LoadContent()
        {
            base.LoadContent();
            r_SpritesRow.Gap = r_SpritesRow.First.Width;
            dance();
        }

        public Vector2 Position
        {
            get
            {
                return r_SpritesRow.Position;
            }

            set
            {
                r_SpritesRow.Position = value;

                // Restart the dance when moved
                dance();
            }
        }

        private void dance()
        {
            bool v_Loop = true;
            foreach (Barrier sprite in r_SpritesRow.SpritesLinkedList)
            {
                SpriteAnimator danceAnimation = new WaypointsAnymator(
                        k_DancingSpeed,
                        TimeSpan.Zero,
                        v_Loop,
                        sprite.Position + new Vector2(sprite.Width, 0),
                        sprite.Position - new Vector2(sprite.Width, 0));

                sprite.Animations.Remove(danceAnimation.Name);
                sprite.Animations.Add(danceAnimation);
                sprite.Animations.Resume();
            }
        }

        public float Width
        {
            get
            {
                return r_SpritesRow.Width;
            }
        }

        public float Height
        {
            get
            {
                return r_SpritesRow.Height;
            }
        }
    }
}
