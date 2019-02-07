using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
        private int m_LastSelected;
        private int m_Selected;
        protected Keys m_MenuUpKey;
        protected Keys m_MenuDownKey;

        public MenuScreen(Game i_Game,
            Color i_NonSelectedRowColor,
            Color i_SelectedRowColor,
            Keys i_MenuUpKey = Keys.Up,
            Keys i_MenuDownKey = Keys.Down) : base(i_Game)
        {
            r_MenuRows = new List<MenuItemsRow>();
            m_Selected = m_LastSelected = 0;
            m_InputManager = i_Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            m_NonSelectedRowColor = i_NonSelectedRowColor;
            m_SelectedRowColor = i_SelectedRowColor;
            m_MenuUpKey = i_MenuUpKey;
            m_MenuDownKey = i_MenuDownKey;
        }

        protected abstract void BuildMenuItems();

        protected void AddMenuItem(MenuItemsRow i_MenuRow)
        {
            if (r_MenuRows.Count == 0)
            {
                i_MenuRow.MenuText.TintColor = m_SelectedRowColor;
                i_MenuRow.MenuText.StartAnimation();
                i_MenuRow.Active = true;
            }
            r_MenuRows.Add(i_MenuRow);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (r_MenuRows.Count != 0)
            {
                if (m_InputManager.KeyPressed(m_MenuUpKey))
                {
                    m_LastSelected = m_Selected--;
                    if (m_Selected < 0)
                    {
                        m_Selected = r_MenuRows.Count - 1;
                    }
                }

                if (m_InputManager.KeyPressed(m_MenuDownKey))
                {
                    m_LastSelected = m_Selected++;
                    if (m_Selected >= r_MenuRows.Count)
                    {
                        m_Selected = 0;
                    }
                }

                if (!r_MenuRows[m_Selected].IsEmpty)
                {
                    if (r_MenuRows[m_Selected].IsLoopedItems)
                    {
                        foreach (MenuItem menuItem in r_MenuRows[m_Selected].Items)
                        {
                            if (m_InputManager.KeyPressed(menuItem.Key))
                            {
                                r_MenuRows[m_Selected].InvokeCurrentSelected();
                            }
                        }
                    }
                    else
                    {
                        if (r_MenuRows[m_Selected].Items.Count == 1 &&
                            m_InputManager.KeyPressed(r_MenuRows[m_Selected].GetSelectedKey()))
                        {
                            r_MenuRows[m_Selected].InvokeCurrentSelected();
                        }
                    }
                }

                if (m_LastSelected != m_Selected)
                {
                    updateSelected();
                }
            }
        }

        private void updateSelected()
        {
            r_MenuRows[m_LastSelected].Active = false;
            r_MenuRows[m_Selected].Active = true;
            r_MenuRows[m_LastSelected].MenuText.TintColor = m_NonSelectedRowColor;
            r_MenuRows[m_LastSelected].StopTitleAnimation();
            r_MenuRows[m_Selected].MenuText.TintColor = m_SelectedRowColor;
            r_MenuRows[m_Selected].StartTitleAnimation();
        }
    }
}