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

        public CollisionDetector(Game i_Game) : base(i_Game)
        {
        }

        public override void Update(GameTime i_GameTime)
        {
            checkAndNotifyForCollisions();
            base.Update(i_GameTime);
        }

        private void checkAndNotifyForCollisions()
        {
            IEnumerable<ICollideable> collideableGameContent = from gameComponent in this.Game.Components
                                                               where gameComponent is ICollideable
                                                               select gameComponent as ICollideable;

            HashSet <ICollideable> checkedContent = new HashSet<ICollideable>();
            foreach (ICollideable collideableA in collideableGameContent)
            {
                checkedContent.Add(collideableA);
                foreach (ICollideable collideableB in collideableGameContent)
                {
                    if (!checkedContent.Contains(collideableB))
                    {
                        checkAndNotifySingleCollision(collideableA, collideableB);
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
            return i_CollideableA.Bounds.Intersects(i_CollideableB.Bounds);
        }

        // The following method was mostly taken from this tutorial: 
        // https://www.youtube.com/watch?v=5vKF0zb0PsA - "C# Xna Made Easy Tutorial 27 - Pixel Perfect Collision"
        private bool pixelCollisionDetected(ICollideable i_CollideableA, ICollideable i_CollideableB)
        {
            bool collisionDetected = false;

            Texture2D spriteA = i_CollideableA.Texture;
            Texture2D spriteB = i_CollideableB.Texture;

            // Store the pixel data
            Color[] colorDataA = new Color[spriteA.Bounds.Width * spriteA.Height];
            Color[] colorDataB = new Color[spriteB.Bounds.Width * spriteB.Height];
            spriteA.GetData(colorDataA);
            spriteB.GetData(colorDataB);

            // Calculate the boundaries of the rectangle which is the overlap between i_CollideableA and i_CollideableB
            // float is used instead of int for numerical capacity
            int top, bottom, left, right;
            top = Math.Max(i_CollideableA.Bounds.Top, i_CollideableB.Bounds.Top);
            bottom = Math.Min(i_CollideableA.Bounds.Bottom, i_CollideableB.Bounds.Bottom);
            left = Math.Max(i_CollideableA.Bounds.Left, i_CollideableB.Bounds.Left);
            right = Math.Min(i_CollideableA.Bounds.Right, i_CollideableB.Bounds.Right);

            // Scan the pixels of the rectangle which defines the overlap
            // and look for a pixel in which both textures are not transparent
            for (int y = top; y < bottom && !collisionDetected; y++)
            {
                for (int x = left; x < right && !collisionDetected; x++)
                {
                    int pixelIndexA = (y - i_CollideableA.Bounds.Top) * (i_CollideableA.Bounds.Width) + (x - i_CollideableA.Bounds.Left);
                    int pixelIndexB = (y - i_CollideableB.Bounds.Top) * (i_CollideableB.Bounds.Width) + (x - i_CollideableB.Bounds.Left);

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
