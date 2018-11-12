using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public static class DrawableObjectsFactory
    {
        public enum eSpriteType
        {
            SpaceBG,
            MotherShip,
            SpaceShip,
            EnemyPink,
            EnemyYellow,
            EnemyLightBlue,
        }

        private const string k_MotherShipURL = @"Sprites\MotherShip_32x120";
        private const string k_SpaceShipURL = @"Sprites\Ship01_32x32";
        private const string k_SpaceBGURL = @"Backgrounds\BG_Space01_1024x768";

        public static Drawable2DGameComponent Create(Game i_Game, eSpriteType i_ObjectType)
        {
            Drawable2DGameComponent objectToReturn = null;
            switch (i_ObjectType)
            {
                case eSpriteType.SpaceBG:
                    objectToReturn = new SpaceBG(i_Game, k_SpaceBGURL);
                    break;
                case eSpriteType.SpaceShip:
                    objectToReturn = new Spaceship(i_Game, k_SpaceShipURL);
                    break;
                case eSpriteType.MotherShip:
                    objectToReturn = new MotherShip(i_Game, k_MotherShipURL);
                    break;
            }
            return objectToReturn;
        }
    }
}