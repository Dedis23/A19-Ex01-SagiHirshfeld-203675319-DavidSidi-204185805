using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens;

namespace Infrastructure.ObjectModel.Menus
{
    public abstract class MenuScreen : GameScreen
    {
        private string m_Title;
        private int m_ActiveItemIndex;
        public MenuScreen(Game i_Game) : base(i_Game)
        {

        }
    }
}
