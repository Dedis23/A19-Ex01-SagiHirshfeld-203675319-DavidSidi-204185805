using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.Menus
{
    public class BarMenuItem : MenuItemsRow
    {
        private IInputManager m_InputManager;
        private SpriteBatch m_SpriteBatch;
        private float m_LastValue;
        private float m_CurrentValue;
        protected MenuItem m_Increase;
        protected MenuItem m_Decrease;
        protected Rectangle m_BorderRect;
        protected Rectangle m_InnerRect;
        protected Rectangle m_FillRect;
        protected Texture2D m_BorderTexture;
        protected Texture2D m_InnerTexture;
        protected Texture2D m_FillTexture;
        protected Color m_BorderColor;
        protected Color m_InnerColor;
        protected Color m_FillColor;
        protected int m_BorderThickness;
        protected float m_Min;
        protected float m_Max;
        protected float m_GrowthValue;
        protected float m_CurrentPercent;

        public BarMenuItem(
            GameScreen i_GameScreen,
            AnimatedTextSprite i_RowText,
            Color i_BorderColor,
            Color i_BackgroundColor,
            Color i_FillColor,
            Rectangle i_Bar,
            float i_Min,
            float i_Max,
            float i_GrowthValue,
            float i_InitialPercentValue,
            MenuItem i_Increase,
            MenuItem i_Decrease,
            int i_BorderThickness = 3)
            : base(i_GameScreen, i_RowText, new MenuItem[] { i_Increase, i_Decrease })
        {
            m_InputManager = Game.Services.GetService<IInputManager>();
            m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            m_Increase = i_Increase;
            m_Decrease = i_Decrease;
            m_Min = i_Min;
            m_Max = i_Max;
            m_BorderColor = i_BorderColor;
            m_InnerColor = i_BackgroundColor;
            m_FillColor = i_FillColor;
            m_GrowthValue = i_GrowthValue;
            m_BorderRect = i_Bar;
            m_BorderThickness = i_BorderThickness;
            m_CurrentPercent = i_InitialPercentValue / 100;
            IsLoopedItems = false;
            Initialize();
        }

        public override void Initialize()
        {
            m_BorderTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            m_BorderTexture.SetData(new[] { Color.White });
            m_InnerTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            m_InnerTexture.SetData(new[] { Color.White });
            m_FillTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            m_FillTexture.SetData(new[] { Color.White });
            m_InnerRect = m_BorderRect;
            m_InnerRect.Inflate(m_BorderThickness * -1, m_BorderThickness * -1);
            m_FillRect = m_InnerRect;
            m_FillRect.Width = (int)(m_InnerRect.Width * m_CurrentPercent);
            m_CurrentValue = m_CurrentPercent * m_Max;
        }

        public override void Update(GameTime gameTime)
        {
            m_ChangeInTheRow = false;
            if (Active)
            {
                m_LastValue = m_CurrentValue;
                if (m_InputManager.KeyPressed(m_Increase.Key) ||
                    (this.Game.IsMouseVisible &&
                    (m_InputManager.ScrollWheelDelta > 0 ||
                    m_InputManager.ButtonPressed(eInputButtons.Right))))
                {
                    m_CurrentValue += m_GrowthValue;
                    if (m_LastValue < 100)
                    {
                        m_Increase.Operation?.Invoke();
                        m_ChangeInTheRow = true;
                    }
                }

                if (m_InputManager.KeyPressed(m_Decrease.Key) ||
                    (this.Game.IsMouseVisible &&
                    m_InputManager.ScrollWheelDelta < 0))
                {
                    m_CurrentValue -= m_GrowthValue;
                    if (m_LastValue > 0)
                    {
                        m_Decrease.Operation?.Invoke();
                        m_ChangeInTheRow = true;
                    }
                }

                m_CurrentValue = MathHelper.Clamp(m_CurrentValue, m_Min, m_Max);
                m_FillRect.Width = m_InnerRect.Width * (int)m_CurrentValue / (int)m_Max;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(m_BorderTexture, m_BorderRect, m_BorderColor);
            m_SpriteBatch.Draw(m_InnerTexture, m_InnerRect, m_InnerColor);
            m_SpriteBatch.Draw(m_FillTexture, m_FillRect, m_FillColor);
            m_SpriteBatch.End();
        }
    }
}