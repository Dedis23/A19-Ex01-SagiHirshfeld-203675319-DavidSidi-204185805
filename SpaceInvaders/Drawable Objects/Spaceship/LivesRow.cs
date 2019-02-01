using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceInvaders
{
    class LivesRow : SpriteRow<LifeIconSprite>
    {
        public LivesRow(CompositeDrawableComponent<IGameComponent> i_ContainingComponent, int i_SpritesNum, string i_AssetName) 
            : base(i_ContainingComponent, i_SpritesNum, (ContainingComponenet) => new LifeIconSprite(i_AssetName, ContainingComponenet))
        {
            this.InsertionOrder = Order.RightToLeft;
            this.RemovalOrder = Order.LeftToRight;
        }
    }
}
