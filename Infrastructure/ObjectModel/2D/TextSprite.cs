using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel
{
    public class TextSprite : Sprite
    {
        private SpriteFont m_Font;
        private string m_Text = string.Empty;

        public TextSprite(Game i_Game, string i_AssetName) : base(i_AssetName, i_Game)
        {
        }

        protected override void LoadTexture()
        {
            m_Font = Game.Content.Load<SpriteFont>(AssetName);
        }

        protected override void SpecificSpriteBatchDraw()
        {
            m_SpriteBatch.DrawString(m_Font, Text, Position, TintColor, Rotation, PositionOrigin, Scales, SpriteEffects.None, LayerDepth);
        }

        protected override void InitBounds()
        {
            measureSize();
        }

        public string Text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;
                measureSize();
            }
        }

        public Rectangle GetTextRectangle()
        {
            return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)GetTextSize().X,
                    (int)GetTextSize().Y);
        }

        public Vector2 GetTextSize()
        {
            if (m_Font != null)
            {
                return m_Font.MeasureString(m_Text);
            }
            else
            {
                return Vector2.Zero;
            }
        }

        private void measureSize()
        {
            if (m_Font != null)
            {
                Vector2 newSpriteSize = m_Font.MeasureString(m_Text);
                m_WidthBeforeScale = newSpriteSize.X;
                m_HeightBeforeScale = newSpriteSize.Y;
            }
        }
    }
}
