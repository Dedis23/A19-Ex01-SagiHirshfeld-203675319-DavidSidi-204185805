using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{    
    public class SpaceBG : BackgroundSprite
    {
        private const string k_AssetName = @"Backgrounds\BG_Space01_1024x768";

        public SpaceBG(Game i_Game) : base(k_AssetName, i_Game)
        {
        }
    }
}
