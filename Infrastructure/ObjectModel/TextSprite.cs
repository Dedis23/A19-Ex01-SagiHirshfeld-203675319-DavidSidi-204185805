using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel
{
    public class TextSprite : Sprite
    {
        private SpriteFont m_Font;
        private string m_Text = string.Empty;

        public TextSprite(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        {            
        }

        protected override void LoadTexture()
        {
            m_Font = Game.Content.Load<SpriteFont>(AssetName);
        }

        protected override void SpecificSpriteBatchDraw()
        {
            m_SpriteBatch.DrawString(m_Font, Text, Position, TintColor);
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
