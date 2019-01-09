using System;
using System.Linq;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    [DontPremultiplyAlpha]
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
            float intersectionHeight = getPreciseHeightOfIntersectionWithBullet(i_Bullet);
            float currentBulletIntersectionPercent = intersectionHeight / i_Bullet.Height;
            float percentLeft = i_RequiredIntersectionPrecent - currentBulletIntersectionPercent;
            i_Bullet.Position += new Vector2(0, percentLeft * i_Bullet.Height * i_Bullet.DirectionVector.Y);
        }

        private float getPreciseHeightOfIntersectionWithBullet(Bullet i_Bullet)
        {
            Rectangle intersection = Rectangle.Intersect(this.Bounds, i_Bullet.Bounds);

            // Traverse the pixels top-to-bottom or bottom-to-top depending on the direction of the bullet
            IEnumerable<int> yTraversalOrder = Enumerable.Range(intersection.Top, intersection.Height);
            if (i_Bullet.DirectionVector.Y == -1)
            {
                yTraversalOrder = yTraversalOrder.OrderByDescending(i => i);
            }

            // Look for the highest and lowest pixels that collide within the intersection rectangle
            float? highestCollidedPixelY = null;
            float? lowestCollidedPixelY = null;
            bool collisionWasDetectedInRow;
            foreach (int y in yTraversalOrder)
            {
                collisionWasDetectedInRow = false;

                for (int x = intersection.Left; x < intersection.Right; x++)
                {
                    int barrierPixelIndex = ((y - this.Bounds.Top) * this.Bounds.Width) + (x - this.Bounds.Left);
                    int bulletPixelIndex = ((y - i_Bullet.Bounds.Top) * i_Bullet.Bounds.Width) + (x - i_Bullet.Bounds.Left);

                    if (this.TextureData[barrierPixelIndex].A != 0 && i_Bullet.TextureData[bulletPixelIndex].A != 0)
                    {
                        collisionWasDetectedInRow = true;

                        if (y < highestCollidedPixelY || highestCollidedPixelY == null)
                        {
                            highestCollidedPixelY = y;
                        }

                        if (y > lowestCollidedPixelY || lowestCollidedPixelY == null)
                        {
                            lowestCollidedPixelY = y;
                        }

                        // Skip to the next Y if a new high or low was found
                        if(highestCollidedPixelY == y || lowestCollidedPixelY == y)
                        {
                            break;
                        }
                    }
                }

                // Stop the scan when Y has passed the intersection (this works due to the shape of the bullet)                 
                if (highestCollidedPixelY.HasValue && lowestCollidedPixelY.HasValue && !collisionWasDetectedInRow)
                {
                    break;
                }
            }

            return Math.Abs(highestCollidedPixelY.Value - lowestCollidedPixelY.Value);
        }

        public void ErasePixelsThatIntersectWith(Sprite i_CollidedSprite)
        {
            const bool v_StopAfterFirstDetection = true;
            Color collidedPixelsModificationFunc(Color p) => new Color(p, 0);

            LookForCollidingPixels(
                i_CollidedSprite as ICollidable2D,
                !v_StopAfterFirstDetection,
                collidedPixelsModificationFunc);
        }
    }
}
