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
    class Background : Drawable2DGameComponent
    {
        private static readonly String sr_SourceFileURL = @"Backgrounds\BG_Space01_1024x768";

        public Background(Game game) : base(game, sr_SourceFileURL)
        {   
        }

        protected override Vector2 GetDefaultPosition()
        {
            return Vector2.Zero;
        }
    }
}
