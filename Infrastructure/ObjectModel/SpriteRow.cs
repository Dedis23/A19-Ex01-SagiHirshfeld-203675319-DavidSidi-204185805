using System;
using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class SpriteRow : SpriteRow<Sprite>
    {
        public SpriteRow(Game i_Game, int i_SpritesNum, Func<Game, Sprite> i_tCreationFunc) : base(i_Game, i_SpritesNum, i_tCreationFunc)
        {
        }
    }

    public class SpriteRow<T> where T : Sprite
    {
        public enum Order
        {
            LeftToRight,
            RightToLeft
        }

        private readonly LinkedList<T> r_SpritesList;
        private readonly Func<Game, T> r_TCreationFunc;
        private readonly Game r_Game;

        public Order InsertionOrder { get; set; } = Order.LeftToRight;

        public SpriteRow(Game i_Game, int i_SpritesNum, Func<Game, T> i_TCreationFunc)
        {
            r_SpritesList = new LinkedList<T>();
            r_Game = i_Game;
            r_TCreationFunc = i_TCreationFunc;

            if (i_SpritesNum <= 0)
            {
                i_SpritesNum = 1;
            }

            for (int i = 0; i < i_SpritesNum; i++)
            {
                AddNewSprite();
            }
        }
        
        public void AddNewSprite()
        {
            T newSprite = r_TCreationFunc(r_Game);

            if (r_SpritesList.Count == 0)
            {
                r_SpritesList.AddFirst(newSprite);
            }
            else
            {
                newSprite.DeepCopyFrom(First);

                if (InsertionOrder == Order.LeftToRight)
                {
                    newSprite.Position = new Vector2(r_SpritesList.Last.Value.Bounds.Right + Gap, this.Position.Y);
                    r_SpritesList.AddLast(newSprite);
                }
                else
                {
                    newSprite.Position = new Vector2(r_SpritesList.Last.Value.Bounds.Left - Gap, this.Position.Y);
                    r_SpritesList.AddFirst(newSprite);
                }
            }
        }

        public void RemoveSprite()
        {
            T spriteToRemove;
            if (r_SpritesList.Count != 0)
            {
                spriteToRemove = r_SpritesList.Last.Value;
                r_SpritesList.RemoveLast();
                spriteToRemove.Kill();
            }
        }

        public T First
        {
            get
            {
                return r_SpritesList.First.Value;
            }
        }

        public T Last
        {
            get
            {
                return r_SpritesList.Last.Value;
            }
        }

        public LinkedList<T> SpritesLinkedList
        {
            get
            {
                return r_SpritesList;
            }
        }

        public float Width
        {
            get
            {
                float gapsSum = Gap * (r_SpritesList.Count - 1);
                float barrierWidthSum = r_SpritesList.First.Value.Width * r_SpritesList.Count;
                return gapsSum + barrierWidthSum;
            }
        }

        public float Height
        {
            get
            {
                return r_SpritesList.First.Value.Height;
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

        public float Gap
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
            LinkedListNode<T> currentSprite = r_SpritesList.First.Next;
            for (int i = 1; i < r_SpritesList.Count; i++)
            {
                if(InsertionOrder == Order.LeftToRight)
                {
                    currentSprite.Value.Position = new Vector2(currentSprite.Previous.Value.Bounds.Right + Gap, First.Position.Y);
                }
                else
                {
                    currentSprite.Value.Position = new Vector2(currentSprite.Previous.Value.Bounds.Left - Gap, First.Position.Y);
                }
                
                currentSprite = currentSprite.Next;
            }
        }
    }
}
