using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel
{
    public class NonPremultipliedSpriteBatch : SpriteBatch
    {
        public NonPremultipliedSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
        }

        public void Begin()
        {
            base.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        }
    }
}
