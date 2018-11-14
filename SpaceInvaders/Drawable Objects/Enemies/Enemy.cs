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
    public class Enemy : Drawable2DGameComponent, ICollideable
    {
        public int PointsValue { get; set; }
        public Enemy(Game i_Game, string i_SourceFileURL, Color i_Tint, int i_PointsValue) : base(i_Game, i_SourceFileURL)
        {
            Tint = i_Tint;
            PointsValue = i_PointsValue;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
        }
    }
}
