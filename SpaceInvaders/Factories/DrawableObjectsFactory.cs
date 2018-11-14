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
            EnemyLightBlue,
            EnemyLightYellow,
            Bullet,
        }

        private const string k_MotherShipURL = @"Sprites\MotherShip_32x120";
        private const string k_SpaceShipURL = @"Sprites\Ship01_32x32";
        private const string k_SpaceBGURL = @"Backgrounds\BG_Space01_1024x768";
        private const string k_EnemyPinkURL = @"Sprites\Enemy0101_32x32";
        private const string k_EnemyLightBlueURL = @"Sprites\Enemy0201_32x32";
        private const string k_EnemyLightYellowURL = @"Sprites\Enemy0301_32x32";
        private const string k_BulletURL = @"Sprites\Bullet";
        private const int k_EnemyPinkPointsValue = 260;
        private const int k_EnemyLightBluePointsValue = 140;
        private const int k_EnemyLightYellowPointsValue = 110;

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
                case eSpriteType.EnemyPink:
                    objectToReturn = new Enemy(i_Game, k_EnemyPinkURL, Color.Pink, k_EnemyPinkPointsValue);
                    break;
                case eSpriteType.EnemyLightBlue:
                    objectToReturn = new Enemy(i_Game, k_EnemyLightBlueURL, Color.LightBlue, k_EnemyLightBluePointsValue);
                    break;
                case eSpriteType.EnemyLightYellow:
                    objectToReturn = new Enemy(i_Game, k_EnemyLightYellowURL, Color.LightYellow, k_EnemyLightYellowPointsValue);
                    break;
                case eSpriteType.Bullet:
                    objectToReturn = new Bullet(i_Game, k_BulletURL);
                    break;
            }
            return objectToReturn;
        }
    }
}