using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class SpriteRow : SpriteRow<Sprite>
    {
        public SpriteRow(Game i_Game, int i_SpritesNum, Func<Game, Sprite> i_tCreationFunc) : base(i_Game, i_SpritesNum, i_tCreationFunc)
        {
        }
    }    
}
