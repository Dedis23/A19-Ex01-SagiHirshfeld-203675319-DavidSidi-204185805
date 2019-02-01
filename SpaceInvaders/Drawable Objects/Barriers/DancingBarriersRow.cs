﻿using System;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace SpaceInvaders
{
    public class DancingBarriersRow : SpriteRow<Barrier>
    {
        private const int k_DancingSpeed = 45;
        private const int k_DefaultBarrierNum = 4;

        public DancingBarriersRow(Game i_Game, int i_BarrierNum) : base(i_Game, i_BarrierNum, Game => new Barrier(i_Game))
        {
            this.InsertionOrder = Order.LeftToRight;
            this.BlendState = BlendState.NonPremultiplied;
        }

        public DancingBarriersRow(Game i_Game) : this(i_Game, k_DefaultBarrierNum)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.Gap = this.First.Width;
            dance();
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }

            set
            {
                base.Position = value;

                // Restart the dance when moved
                dance();
            }
        }

        private void dance()
        {
            bool v_Loop = true;
            foreach (Barrier sprite in this.SpritesLinkedList)
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
    }
}
