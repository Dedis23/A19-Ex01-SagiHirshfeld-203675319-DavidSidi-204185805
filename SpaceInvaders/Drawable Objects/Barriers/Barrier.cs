using System;
using System.Linq;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Infrastructure;

namespace SpaceInvaders
{
    public class Barrier : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\Barrier_44x32";
        private const string k_BarrierHitSoundEffectAssetName = @"Audio/BarrierHit";
        private const float k_BulletDamagePercent = 0.7f;
        private Color[] m_OriginalTextureData;
        private SoundEffectInstance m_BarrierHitSoundEffectInstance;

        public Barrier(Game i_Game) : base(k_AssetName, i_Game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_BarrierHitSoundEffectInstance = Game.Content.Load<SoundEffect>(k_BarrierHitSoundEffectAssetName).CreateInstance();
        }

        protected override void LoadTexture()
        {
            Texture2D texturePrototype = ContentManager.Load<Texture2D>(m_AssetName);
            m_OriginalTextureData = new Color[texturePrototype.Width * texturePrototype.Height];
            texturePrototype.GetData(m_OriginalTextureData);
            this.Texture = new Texture2D(GraphicsDevice, texturePrototype.Width, texturePrototype.Height);
            this.Texture.SetData(m_OriginalTextureData);
        }

        public void ReceiveBulletDamage(Bullet i_Bullet)
        {
            i_Bullet.Visible = false;
            moveBulletToMatchRequiredIntersectionPrecent(i_Bullet, k_BulletDamagePercent);
            ErasePixelsThatIntersectWith(i_Bullet);
            m_BarrierHitSoundEffectInstance.PauseAndThenPlay();
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
            float preciseHeightOfIntersection = 0;
            Rectangle rectanglesIntersection = Rectangle.Intersect(this.Bounds, i_Bullet.Bounds);
            Color[] barrierTextureData = new Color[this.Texture.Width * this.Texture.Height];
            Color[] bulletTextureData = new Color[i_Bullet.Texture.Width * i_Bullet.Texture.Height];

            this.Texture.GetData(barrierTextureData);
            i_Bullet.Texture.GetData(bulletTextureData);

            // Traverse the pixels top-to-bottom or bottom-to-top depending on the direction of the bullet
            IEnumerable<int> yTraversalOrder = Enumerable.Range(rectanglesIntersection.Top, rectanglesIntersection.Height);
            if (i_Bullet.DirectionVector.Y == 1)
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

                for (int x = rectanglesIntersection.Left; x < rectanglesIntersection.Right; x++)
                {
                    int barrierPixelIndex = ((y - this.Bounds.Top) * this.Bounds.Width) + (x - this.Bounds.Left);
                    int bulletPixelIndex = ((y - i_Bullet.Bounds.Top) * i_Bullet.Bounds.Width) + (x - i_Bullet.Bounds.Left);

                    if (barrierTextureData[barrierPixelIndex].A != 0 && bulletTextureData[bulletPixelIndex].A != 0)
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
                    preciseHeightOfIntersection = Math.Abs(highestCollidedPixelY.Value - lowestCollidedPixelY.Value);
                    break;
                }
            }

            return preciseHeightOfIntersection;
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

        public void Reset()
        {
            this.Texture.SetData(m_OriginalTextureData);
        }
    }
}
