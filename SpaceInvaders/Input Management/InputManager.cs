using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class InputManager : GameComponent
    {
        private const int k_FramesToWaitBeforeInputTaking = 3;
        private static readonly IEnumerable<Keys> sr_EnumerableKeyboardKeys = Enum.GetValues(typeof(Keys)).Cast<Keys>();
        private readonly Dictionary<Action<GameTime>, Keys> r_ActionToKeyboardDownDictionary;
        private readonly Dictionary<Action<GameTime>, Keys> r_ActionToKeyboardSinglePressDictionary;
        private int m_WaitedFramesBeforeInputTaking = 0;
        private KeyboardState m_CurrentKeyboardState;
        private MouseState m_CurrentMouseState;
        private KeyboardState? m_PrevKeyboardState;
        private MouseState? m_PrevMouseState;

        public event Action<GameTime, Vector2> MouseMoved;

        public event Action<GameTime> MouseLeftButtonPressed;

        public event Action<GameTime> MouseRightButtonPressed;

        public event Action<GameTime> MouseLeftButtonPressedOnce;

        public event Action<GameTime> MouseRightButtonPressedOnce;

        public InputManager(Game i_Game) : base(i_Game)
        {
            r_ActionToKeyboardDownDictionary = new Dictionary<Action<GameTime>, Keys>();
            r_ActionToKeyboardSinglePressDictionary = new Dictionary<Action<GameTime>, Keys>(); 
        }

        public void RegisterKeyboardKeyDownBinding(Action<GameTime> i_Action, Keys i_KeyboardKey)
        {
            if(r_ActionToKeyboardDownDictionary.ContainsKey(i_Action))
            {
                r_ActionToKeyboardDownDictionary[i_Action] = i_KeyboardKey;
            }
            else
            {
                r_ActionToKeyboardDownDictionary.Add(i_Action, i_KeyboardKey);
            }
        }

        public void RegisterKeyboardSinglePressBinding(Action<GameTime> i_Action, Keys i_KeyboardKey)
        {
            if (r_ActionToKeyboardSinglePressDictionary.ContainsKey(i_Action))
            {   
                r_ActionToKeyboardSinglePressDictionary[i_Action] = i_KeyboardKey;
            }
            else
            {
                r_ActionToKeyboardSinglePressDictionary.Add(i_Action, i_KeyboardKey);
            }
        }

        public void RemoveKeyboardDownBinding(Action<GameTime> i_Action)
        {
            r_ActionToKeyboardDownDictionary.Remove(i_Action);
        }

        public void RemoveKeyboardSinglePressBinding(Action<GameTime> i_Action)
        {
            r_ActionToKeyboardSinglePressDictionary.Remove(i_Action);
        }

        public override void Update(GameTime i_GameTime)
        {
            m_CurrentKeyboardState = Keyboard.GetState();
            m_CurrentMouseState = Mouse.GetState();

            // We've noticed that accurate input data for the mouse only registers
            // after the second frame (= after the second Update)
            if(m_WaitedFramesBeforeInputTaking >= k_FramesToWaitBeforeInputTaking)
            {
                checkAndNotifyForKeyboardInput(i_GameTime);
                checkAndNotifyForMouseInput(i_GameTime);
            }
            else
            {
                m_WaitedFramesBeforeInputTaking++;
            }

            m_PrevKeyboardState = m_CurrentKeyboardState;
            m_PrevMouseState = m_CurrentMouseState;

            base.Update(i_GameTime);
        }

        private void checkAndNotifyForKeyboardInput(GameTime i_GameTime)
        {
            // Check and notify if any keyboard key of a registered action is down
            foreach (Action<GameTime> action in r_ActionToKeyboardDownDictionary.Keys)
            {
                if (m_CurrentKeyboardState.IsKeyDown(r_ActionToKeyboardDownDictionary[action]))
                {
                    action.Invoke(i_GameTime);
                }
            }

            // Check and notify if a keyboard key was pressed only once
            foreach (Action<GameTime> action in r_ActionToKeyboardSinglePressDictionary.Keys)
            {
                Keys keyBindedToAction = r_ActionToKeyboardSinglePressDictionary[action];
                if (m_CurrentKeyboardState.IsKeyDown(keyBindedToAction) && (m_PrevKeyboardState == null || !m_PrevKeyboardState.Value.IsKeyDown(keyBindedToAction)))
                {
                    action.Invoke(i_GameTime);
                }
            }
        }

        private void checkAndNotifyForMouseInput(GameTime i_GameTime)
        {
            Vector2 mousePositionDelta = getMousePositionDelta();
            if (mousePositionDelta != Vector2.Zero)
            {
                MouseMoved?.Invoke(i_GameTime, mousePositionDelta);
            }

            if (m_CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                MouseLeftButtonPressed?.Invoke(i_GameTime);
            }

            if (m_CurrentMouseState.RightButton == ButtonState.Pressed)
            {
                MouseRightButtonPressed?.Invoke(i_GameTime);
            }

            if (m_CurrentMouseState.LeftButton == ButtonState.Pressed && 
                (m_PrevMouseState == null || m_PrevMouseState.Value.LeftButton == ButtonState.Released))
            {
                MouseLeftButtonPressedOnce?.Invoke(i_GameTime);
            }

            if (m_CurrentMouseState.RightButton == ButtonState.Pressed &&
                (m_PrevMouseState == null || m_PrevMouseState.Value.RightButton == ButtonState.Released))
            {
                MouseRightButtonPressedOnce?.Invoke(i_GameTime);
            }
        }

        private Vector2 getMousePositionDelta()
        {
            Vector2 retVal = Vector2.Zero;

            if (m_PrevMouseState != null)
            {
                retVal.X = m_CurrentMouseState.X - m_PrevMouseState.Value.X;
                retVal.Y = m_CurrentMouseState.Y - m_PrevMouseState.Value.Y;
            }

            return retVal;
        }
    }
}
