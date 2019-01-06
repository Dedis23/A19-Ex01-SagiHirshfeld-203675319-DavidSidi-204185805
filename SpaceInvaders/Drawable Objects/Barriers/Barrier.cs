using System;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class Barrier : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\Barrier_44x32";
        private const float k_BulletDamagePercent = 0.7f;

        public Barrier(Game i_Game) : base(k_AssetName, i_Game)
        {
        }

        protected override void LoadTexture()
        {
            Texture2D texturePrototype = ContentManager.Load<Texture2D>(m_AssetName);
            Color[] copiedPixels = new Color[texturePrototype.Width * texturePrototype.Height];
            texturePrototype.GetData(copiedPixels);
            this.Texture = new Texture2D(GraphicsDevice, texturePrototype.Width, texturePrototype.Height);
            this.Texture.SetData(copiedPixels);
        }

        public void ReceiveBulletDamage(Bullet i_Bullet)
        {
            i_Bullet.Visible = false;
            moveBulletToMatchRequiredIntersectionPrecent(i_Bullet, k_BulletDamagePercent);
            ErasePixelsThatIntersectWith(i_Bullet);
        }

        private void moveBulletToMatchRequiredIntersectionPrecent(Bullet i_Bullet, float i_RequiredIntersectionPrecent)
        {
            float intersectionHeight = getPreciseIntersectionHeight(i_Bullet);
            float currentBulletIntersectionPercent = intersectionHeight / i_Bullet.Height;
            float percentLeft = i_RequiredIntersectionPrecent - currentBulletIntersectionPercent;
            Vector2 bulletDirection = Vector2.Normalize(i_Bullet.Velocity);
            i_Bullet.Position += new Vector2(0, percentLeft * i_Bullet.Height * bulletDirection.Y);
        }

        private float getPreciseIntersectionHeight(Bullet i_Bullet)
        {
            Texture2D bulletTexture = i_Bullet.Texture;

            // Store the pixel data
            Color[] barrierPixels = new Color[Texture.Width * Texture.Height];
            Color[] bulletPixels = new Color[bulletTexture.Width * bulletTexture.Height];
            Texture.GetData(barrierPixels);
            bulletTexture.GetData(bulletPixels);

            Rectangle intersection = Rectangle.Intersect(this.Bounds, i_Bullet.Bounds);

            float? highestCollidedPixelY = null;
            float? lowestCollidedPixelY = null;

            // Scan the pixels of both sprites within their intersection
            // and look for a pixel in which both textures are not transparent
            for (int y = intersection.Top; y < intersection.Bottom; y++)
            {
                for (int x = intersection.Left; x < intersection.Right; x++)
                {
                    int barrierPixelInvdex = ((y - this.Bounds.Top) * this.Bounds.Width) + (x - this.Bounds.Left);
                    int bulletPixelIndex = ((y - i_Bullet.Bounds.Top) * i_Bullet.Bounds.Width) + (x - i_Bullet.Bounds.Left);

                    if (barrierPixels[barrierPixelInvdex].A != 0 && bulletPixels[bulletPixelIndex].A != 0)
                    {
                        if (y < highestCollidedPixelY || highestCollidedPixelY == null)
                        {
                            highestCollidedPixelY = y;
                        }

                        if (y > lowestCollidedPixelY || lowestCollidedPixelY == null)
                        {
                            lowestCollidedPixelY = y;
                        }
                    }
                }
            }

            return Math.Abs(highestCollidedPixelY.Value - lowestCollidedPixelY.Value);
        }

        public void ErasePixelsThatIntersectWith(Sprite i_CollidedSprite)
        {
            Texture2D collidedTexture = i_CollidedSprite.Texture;

            // Store the pixel data
            Color[] barrierPixels = new Color[Texture.Width * Texture.Height];
            Color[] collidedPixels = new Color[collidedTexture.Width * collidedTexture.Height];
            Texture.GetData(barrierPixels);
            collidedTexture.GetData(collidedPixels);

            Rectangle intersection = Rectangle.Intersect(this.Bounds, i_CollidedSprite.Bounds);

            // Scan the pixels of both sprites within their intersection
            // and look for a pixel in which both textures are not transparent
            for (int y = intersection.Top; y < intersection.Bottom; y++)
            {
                for (int x = intersection.Left; x < intersection.Right; x++)
                {
                    int barrierPixelInvdex = ((y - this.Bounds.Top) * this.Bounds.Width) + (x - this.Bounds.Left);
                    int collidedPixelIndex = ((y - i_CollidedSprite.Bounds.Top) * i_CollidedSprite.Bounds.Width) + (x - i_CollidedSprite.Bounds.Left);

                    if (barrierPixels[barrierPixelInvdex].A != 0 && collidedPixels[collidedPixelIndex].A != 0)
                    {
                        barrierPixels[barrierPixelInvdex].A = 0;
                    }
                }
            }

            Texture.SetData(barrierPixels);
        }
    }
}
