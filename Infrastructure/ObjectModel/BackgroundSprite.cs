using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public class BackgroundSprite : Sprite
    {
        public BackgroundSprite(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        {
        }

        protected override void LoadContent()
        {
            // This promises the background will be printed before any other sprite
            CreateAndUsePrivateSpriteBatch();
            this.DrawOrder = int.MinValue;
            base.LoadContent();
        }
    }
}
