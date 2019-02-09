using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Screens;

namespace Infrastructure.Menus
{
    public abstract class MenuScreen : GameScreen
    {
        protected IInputManager m_InputManager;
        private readonly List<MenuItemsRow> r_MenuRows;
        private Color m_NonSelectedRowColor;
        private Color m_SelectedRowColor;
        private int m_PrevSelected;
        private int m_CurrSelected;
        private MouseState m_PrevMouseState;
        protected Keys m_MenuUpKey;
        protected Keys m_MenuDownKey;

        public MenuScreen(
            Game i_Game,
            Color i_NonSelectedRowColor,
            Color i_SelectedRowColor,
            Keys i_MenuUpKey = Keys.Up,
            Keys i_MenuDownKey = Keys.Down) : base(i_Game)
        {
            r_MenuRows = new List<MenuItemsRow>();
            m_InputManager = Game.Services.GetService<IInputManager>();
            m_PrevSelected = m_CurrSelected = -1;
            m_PrevMouseState = m_InputManager.MouseState;
            m_NonSelectedRowColor = i_NonSelectedRowColor;
            m_SelectedRowColor = i_SelectedRowColor;
            m_MenuUpKey = i_MenuUpKey;
            m_MenuDownKey = i_MenuDownKey;
        }

        protected abstract void BuildMenuItems();

        protected void AddMenuItem(MenuItemsRow i_MenuRow)
        {
            r_MenuRows.Add(i_MenuRow);
        }

        public void MarkASpecificItemInTheRow(int i_Row, int i_ItemInTheRowToMark)
        {
            r_MenuRows[i_Row].MarkASpecificItem(i_ItemInTheRowToMark);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_PrevSelected = m_CurrSelected;
            if (r_MenuRows.Count != 0)
            {
                if (m_InputManager.MouseState != m_PrevMouseState)
                {
                    handleMouseInput();
                }
                else
                {
                    handleKeyboardInput();
                }

                handleMenuSoundEffects();
            }
        }

        private void handleMenuSoundEffects()
        {
            if (m_CurrSelected != -1)
            {
                if(r_MenuRows[m_CurrSelected].IsThereAChangeInTheRow())
                {
                    PlayMenuMoveSoundEffect();
                }

                if (m_CurrSelected != m_PrevSelected)
                {
                    PlayMenuMoveSoundEffect();
                }
            }
        }

        private void handleKeyboardInput()
        {
            if (m_InputManager.KeyPressed(m_MenuUpKey))
            {
                m_CurrSelected--;
                if (m_CurrSelected < 0)
                {
                    m_CurrSelected = r_MenuRows.Count - 1;
                }
            }

            if (m_InputManager.KeyPressed(m_MenuDownKey))
            {
                m_CurrSelected++;
                if (m_CurrSelected >= r_MenuRows.Count)
                {
                    m_CurrSelected = 0;
                }
            }

            if (m_CurrSelected != -1)
            {
                UpdateSelected(m_CurrSelected);
                checkForInputOperationsInvokes(m_CurrSelected);
            }
        }

        private void handleMouseInput()
        {
            int i;
            m_CurrSelected = -1;
            bool isMouseOnARow = false;
            Point mousePosition = new Point(m_InputManager.MouseState.Position.X, m_InputManager.MouseState.Position.Y);
            for (i = 0; i < r_MenuRows.Count; i++)
            {
                if (r_MenuRows[i].MenuText.GetTextRectangle().Contains(mousePosition))
                {
                    isMouseOnARow = true;
                    m_CurrSelected = i;
                    UpdateSelected(m_CurrSelected);
                    checkForInputOperationsInvokes(i);
                    break;
                }
            }

            checkScrollWheel();
            if (!isMouseOnARow)
            {
                UpdateSelected(m_CurrSelected);
            }

            m_PrevMouseState = m_InputManager.MouseState;
        }

        private void checkForInputOperationsInvokes(int i_IndexToInvoke)
        {
            if (r_MenuRows[i_IndexToInvoke].IsLoopedItems)
            {
                bool isThereAnInvokationInput = false;
                foreach (MenuItem menuItem in r_MenuRows[i_IndexToInvoke].Items)
                {
                    if (m_InputManager.KeyPressed(menuItem.Key))
                    {
                        isThereAnInvokationInput = true;
                        break;
                    }
                    else if (m_InputManager.ButtonPressed(eInputButtons.Right))
                    {
                        isThereAnInvokationInput = true;
                        break;
                    }
                }

                if (isThereAnInvokationInput)
                {
                    r_MenuRows[i_IndexToInvoke].IncreaseCurrentItem();
                    r_MenuRows[i_IndexToInvoke].UpdateSelectedColor();
                    r_MenuRows[i_IndexToInvoke].InvokeCurrentSelected();
                }
            }
            else
            {
                if (r_MenuRows[i_IndexToInvoke].Items.Count == 1 &&
                    (m_InputManager.KeyPressed(r_MenuRows[i_IndexToInvoke].GetSelectedKey())
                    || m_InputManager.ButtonPressed(eInputButtons.Left)))
                {
                    r_MenuRows[i_IndexToInvoke].InvokeCurrentSelected();
                }
            }
        }

        private void checkScrollWheel()
        {
            bool scrollWheelActivity = false;
            if (m_CurrSelected != -1 &&
                r_MenuRows[m_CurrSelected].Active &&
                r_MenuRows[m_CurrSelected].IsLoopedItems)
            {
                if (m_InputManager.ScrollWheelDelta > 0)
                {
                    scrollWheelActivity = true;
                    r_MenuRows[m_CurrSelected].IncreaseCurrentItem();
                }
                else if (m_InputManager.ScrollWheelDelta < 0)
                {
                    scrollWheelActivity = true;
                    r_MenuRows[m_CurrSelected].DecreaseCurrentItem();
                }
            }

            if (scrollWheelActivity)
            {
                r_MenuRows[m_CurrSelected].UpdateSelectedColor();
                r_MenuRows[m_CurrSelected].InvokeCurrentSelected();
            }
        }

        protected virtual void UpdateSelected(int i_IndexToMark)
        {
            for (int i = 0; i < r_MenuRows.Count; i++)
            {
                if (i == i_IndexToMark)
                {
                    r_MenuRows[i].Active = true;
                    r_MenuRows[i].MenuText.TintColor = m_SelectedRowColor;
                    r_MenuRows[i].StartTitleAnimation();
                }
                else
                {
                    r_MenuRows[i].Active = false;
                    r_MenuRows[i].MenuText.TintColor = m_NonSelectedRowColor;
                    r_MenuRows[i].StopTitleAnimation();
                }
            }
        }

        protected abstract void PlayMenuMoveSoundEffect();
    }
}