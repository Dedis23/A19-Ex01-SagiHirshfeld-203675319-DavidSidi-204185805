using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class SpaceBG : Drawable2DGameComponent
    {
        public SpaceBG(Game i_Game, string i_SourceFileURL) : base(i_Game, i_SourceFileURL)
        {
            setDefaultPosition();
        }

        public void setDefaultPosition()
        {
            PositionX = 0.0f;
            PositionY = 0.0f;
        }
    }
}
