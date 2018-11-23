using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class SpaceBG : Drawable2DGameComponent
    {
        public SpaceBG(Game game, string i_SourceFileURL) : base(game, i_SourceFileURL)
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
