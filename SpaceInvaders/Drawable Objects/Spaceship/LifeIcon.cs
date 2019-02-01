using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    [DontPremultiplyAlpha]
    public class LifeIcon : Sprite
    {
        public LifeIcon(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        {
            this.Opacity /= 2;
            this.Scales /= 2;
        }
    }
}
