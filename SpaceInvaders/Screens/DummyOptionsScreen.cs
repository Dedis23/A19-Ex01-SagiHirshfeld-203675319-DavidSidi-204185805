using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using System;

namespace SpaceInvaders
{
    public class DummyOptionsScreen : GameScreen
    {
        private TextSprite m_TextSprite;

        public DummyOptionsScreen(Game i_Game) : base(i_Game)
        {
            m_TextSprite = new TextSprite(@"Fonts\OptionsFont", Game);
            this.Add(m_TextSprite);
        }

        public override void Initialize()
        {
            base.Initialize();

            m_TextSprite.Text = "Dummy Options Menu" + Environment.NewLine +"Press Q to play";
            m_TextSprite.Position = CenterOfViewPort - new Vector2(m_TextSprite.Width / 2, 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                ExitScreen();
            }
        }
    }
}
