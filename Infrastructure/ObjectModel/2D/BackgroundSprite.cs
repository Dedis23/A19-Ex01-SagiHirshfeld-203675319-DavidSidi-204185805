using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public class BackgroundSprite : Sprite, IBackground
    {
        public BackgroundSprite(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game, int.MinValue)
        {
        }
    }
}
