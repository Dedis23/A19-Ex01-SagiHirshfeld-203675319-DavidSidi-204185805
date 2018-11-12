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
    public class SpaceBG : Drawable2DGameComponent
    {
        public SpaceBG(Game game, string i_SourceFileURL) : base(game, i_SourceFileURL)
        {   
        }

        protected override Vector2 GetDefaultPosition()
        {
            return Vector2.Zero;
        }
    }
}
