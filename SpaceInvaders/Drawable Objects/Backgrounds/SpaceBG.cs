using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public class SpaceBG : Sprite
    {
        private const string k_AssetName = @"Backgrounds\BG_Space01_1024x768";
        public SpaceBG(Game i_Game) : base(k_AssetName, i_Game)
        {            
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            this.DrawOrder = int.MinValue;
        }
    }
}
