using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class CollisionDetector : GameComponent
    {
        public event Action<ICollideable, ICollideable> CollisionDetected;

        public CollisionDetector(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            checkAndNotifyForCollisions();
            base.Update(gameTime);
        }

        private void checkAndNotifyForCollisions()
        {
            var collideableGameContent = from gameComponent in this.Game.Components
                                         where gameComponent is ICollideable
                                         select gameComponent;            

            HashSet<ICollideable> checkedContent = new HashSet<ICollideable>();
            foreach (ICollideable collideableA in collideableGameContent)
            {
                if (!checkedContent.Contains(collideableA))
                {
                    checkedContent.Add(collideableA);
                    foreach (ICollideable collideableB in collideableGameContent)
                    {
                        if (!checkedContent.Contains(collideableB) && 
                            !(collideableB is IProjectile && collideableA is IProjectile))
                        {
                            checkAndNotifySingleCollision(collideableA, collideableB);
                        }
                    }
                }
            }
        }

        private void checkAndNotifySingleCollision(ICollideable i_CollideableA, ICollideable i_CollideableB)
        {
            // Checking for collision in the resolution of a pixel is expensive (computing-wise)
            // so check for it only if the boxed boundaries are overlapping -
            // which is significantly "cheaper" to check.
            if (boxedBoundriesAreOverlapping(i_CollideableA, i_CollideableB))
            {
                if (pixelCollisionDetected(i_CollideableA, i_CollideableB))
                {
                    CollisionDetected.Invoke(i_CollideableA, i_CollideableB);
                }
            }
        }

        private bool boxedBoundriesAreOverlapping(ICollideable i_CollideableA, ICollideable i_CollideableB)
        {
            Rectangle rectangleA = new Rectangle(i_CollideableA.Left, i_CollideableA.Top, i_CollideableA.Width, i_CollideableA.Height);
            Rectangle RectangleB = new Rectangle(i_CollideableB.Left, i_CollideableB.Top, i_CollideableB.Width, i_CollideableB.Height);
            return rectangleA.Intersects(RectangleB);
        }

        private bool pixelCollisionDetected(ICollideable i_CollideableA, ICollideable i_CollideableB)
        {
            bool collisionDetected = false;

            Texture2D spriteA = i_CollideableA.Texture;
            Texture2D spriteB = i_CollideableB.Texture;

            // Store the pixel data
            Color[] colorDataA = new Color[spriteA.Width * spriteA.Height];
            Color[] colorDataB = new Color[spriteB.Width * spriteB.Height];
            spriteA.GetData<Color>(colorDataA);
            spriteB.GetData<Color>(colorDataB);

            // Calculate the boundaries of the rectangle which is the overlap between i_CollideableA and i_CollideableB
            // float is used instead of int for numerical capacity
            int top, bottom, left, right;
            top = Math.Max(i_CollideableA.Top, i_CollideableB.Top);
            bottom = Math.Min(i_CollideableA.Bottom, i_CollideableB.Bottom);
            left = Math.Max(i_CollideableA.Left, i_CollideableB.Left);
            right = Math.Min(i_CollideableA.Right, i_CollideableB.Right);

            // Scan the pixels of the rectangle which defines the overlap
            // and look for a pixel in which both textures are not transparent
            for (int y = top; y < bottom && !collisionDetected; y++)
            {
                for (int x = left; x < right && !collisionDetected; x++)
                {
                    int pixelIndexA = (y - i_CollideableA.Top) * (i_CollideableA.Width) + (x - i_CollideableA.Left);
                    int pixelIndexB = (y - i_CollideableB.Top) * (i_CollideableB.Width) + (x - i_CollideableB.Left);

                    Color pixelOfSpriteA = colorDataA[pixelIndexA];
                    Color pixelOfSpriteB = colorDataB[pixelIndexB];

                    // Color.A is the color's alpha component which determines opacity
                    // when a pixel's alpha == 0 that pixel is completely transparent 
                    collisionDetected = pixelOfSpriteA.A != 0 && pixelOfSpriteB.A != 0;
                }
            }

            return collisionDetected;
        }
    }
}
