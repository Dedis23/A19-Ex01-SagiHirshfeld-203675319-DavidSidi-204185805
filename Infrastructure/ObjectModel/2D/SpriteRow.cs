using System;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class SpriteRow : SpriteRow<Sprite>
    {
        public SpriteRow(Game i_Game, int i_SpritesNum, string i_AssetName)
            : base(i_Game, i_SpritesNum, Game => new Sprite(i_AssetName, i_Game))
        {
        }
    }    
}
