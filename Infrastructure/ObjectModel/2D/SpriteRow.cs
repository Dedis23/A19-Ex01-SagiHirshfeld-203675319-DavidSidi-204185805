using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public class SpriteRow<T> : CompositeDrawableComponent<T>
        where T : Sprite
    {
        public enum Order
        {
            LeftToRight,
            RightToLeft
        }

        protected readonly LinkedList<T> r_SpritesLinkedList;
        private readonly Func<Game, T> r_TCreationFunc;
        private readonly Game r_Game;

        public Order InsertionOrder { get; set; } = Order.LeftToRight;

        public Order RemovalOrder { get; set; } = Order.RightToLeft;

        public SpriteRow(Game i_Game, int i_SpritesNum, Func<Game, T> i_TCreationFunc) : base(i_Game)
        {
            r_SpritesLinkedList = new LinkedList<T>();
            r_Game = i_Game;
            r_TCreationFunc = i_TCreationFunc;

            if (i_SpritesNum <= 0)
            {
                i_SpritesNum = 1;
            }

            for (int i = 0; i < i_SpritesNum; i++)
            {
                AddSprite();
            }
        }

        public void AddSprite()
        {
            T newSprite = r_TCreationFunc(r_Game);

            if (r_SpritesLinkedList.Count == 0)
            {
                r_SpritesLinkedList.AddFirst(newSprite);
            }

            else
            {
                newSprite.Opacity = this.Opacity;
                newSprite.Scales = this.Scales;
                newSprite.TintColor = this.TintColor;
                newSprite.Rotation = this.Rotation;
                newSprite.Velocity = this.Velocity;


                if (InsertionOrder == Order.LeftToRight)
                {
                    newSprite.Position = new Vector2(r_SpritesLinkedList.Last.Value.Bounds.Right + GapBetweenSprites, this.Position.Y);
                }
                else
                {
                    newSprite.Position = new Vector2(r_SpritesLinkedList.Last.Value.Bounds.Left - GapBetweenSprites, this.Position.Y);
                }

                r_SpritesLinkedList.AddLast(newSprite);
            }

            this.Add(newSprite);
        }

        public void RemoveSprite()
        {
            T spriteToRemove;

            if (r_SpritesLinkedList.Count != 0)
            {
                if (InsertionOrder != RemovalOrder)
                {
                    spriteToRemove = r_SpritesLinkedList.Last.Value;
                    r_SpritesLinkedList.RemoveLast();
                }
                else
                {
                    spriteToRemove = r_SpritesLinkedList.First.Value;
                    r_SpritesLinkedList.RemoveFirst();
                }

                spriteToRemove.Kill();
                this.Remove(spriteToRemove);
            }            
        }

        public T First
        {
            get
            {
                return r_SpritesLinkedList.First.Value;
            }
        }

        public T Last
        {
            get
            {
                return r_SpritesLinkedList.Last.Value;
            }
        }

        public LinkedList<T> SpritesLinkedList
        {
            get
            {
                return r_SpritesLinkedList;
            }
        }

        public float Width
        {
            get
            {
                float gapsSum = GapBetweenSprites * (r_SpritesLinkedList.Count - 1);
                float barrierWidthSum = r_SpritesLinkedList.First.Value.Width * r_SpritesLinkedList.Count;
                return gapsSum + barrierWidthSum;
            }
        }

        public float Height
        {
            get
            {
                return r_SpritesLinkedList.First.Value.Height;
            }
        }

        public virtual Vector2 Position
        {
            get
            {
                return this.First.Position;
            }

            set
            {
                this.First.Position = value;
                placeSpritesInARowAccordingToTheFirstSpritePosition();
            }
        }

        private float m_Gap;

        public float GapBetweenSprites
        {
            get { return m_Gap; }
            set
            {
                m_Gap = value;

                // Replace with the new Gap
                placeSpritesInARowAccordingToTheFirstSpritePosition();
            }
        }

        public Vector2 Velocity
        {
            get { return First.Velocity; }
            set
            {
                foreach (Sprite sprite in SpritesLinkedList)
                {
                    sprite.Velocity = value;
                }
            }
        }

        public float Opacity
        {
            get { return First.Opacity; }
            set
            {
                foreach (Sprite sprite in SpritesLinkedList)
                {
                    sprite.Opacity = value;
                }
            }
        }

        public Vector2 Scales
        {
            get { return First.Scales; }
            set
            {
                foreach (Sprite sprite in SpritesLinkedList)
                {
                    sprite.Scales = value;
                }
            }
        }

        public Color TintColor
        {
            get { return First.TintColor; }
            set
            {
                foreach (Sprite sprite in SpritesLinkedList)
                {
                    sprite.TintColor = value;
                }
            }
        }

        public float Rotation
        {
            get { return First.Rotation; }
            set
            {
                foreach (Sprite sprite in SpritesLinkedList)
                {
                    sprite.Rotation = value;
                }
            }
        }

        private void placeSpritesInARowAccordingToTheFirstSpritePosition()
        {
            LinkedListNode<T> currentSprite = r_SpritesLinkedList.First.Next;
            for (int i = 1; i < r_SpritesLinkedList.Count; i++)
            {
                if (InsertionOrder == Order.LeftToRight)
                {
                    currentSprite.Value.Position = new Vector2(currentSprite.Previous.Value.Bounds.Right + GapBetweenSprites, First.Position.Y);
                }
                else
                {
                    currentSprite.Value.Position = new Vector2(currentSprite.Previous.Value.Bounds.Left - GapBetweenSprites, First.Position.Y);
                }

                currentSprite = currentSprite.Next;
            }
        }
    }

    // The default case of SpriteRow<T>
    public class SpriteRow : SpriteRow<Sprite>
    {
        public SpriteRow(Game i_Game, int i_SpritesNum, string i_AssetName)
            : base(i_Game, i_SpritesNum, Game => new Sprite(i_AssetName, i_Game))
        {
        }
    }
}
