using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public static class DrawableObjectsFactory
    {
        public enum eSpriteType
        {
            SpaceBG,
            Spaceship,
            Mothership,
            InvaderPink,
            InvaderLightBlue,
            InvaderLightYellow,
            Bullet,
        }

        private const string k_MothershipURL = @"Sprites\MotherShip_32x120";
        private const string k_SpaceshipURL = @"Sprites\Ship01_32x32";
        private const string k_SpaceBGURL = @"Backgrounds\BG_Space01_1024x768";
        private const string k_InvaderPinkURL = @"Sprites\Enemy0101_32x32";
        private const string k_InvaderLightBlueURL = @"Sprites\Enemy0201_32x32";
        private const string k_InvaderLightYellowURL = @"Sprites\Enemy0301_32x32";
        private const string k_BulletURL = @"Sprites\Bullet";
        private const int k_InvaderPinkPointsValue = 260;
        private const int k_InvaderLightBluePointsValue = 140;
        private const int k_InvaderLightYellowPointsValue = 110;

        public static Sprite Create(Game i_Game, eSpriteType i_ObjectType)
        {
            Sprite objectToReturn = null;
            switch (i_ObjectType)
            {
                case eSpriteType.SpaceBG:
                    objectToReturn = new SpaceBG(i_Game, k_SpaceBGURL);
                    break;

                case eSpriteType.Spaceship:
                    objectToReturn = new Spaceship(i_Game, k_SpaceshipURL);
                    break;

                case eSpriteType.Mothership:
                    objectToReturn = new Mothership(i_Game, k_MothershipURL);
                    break;

                case eSpriteType.InvaderPink:
                    objectToReturn = new Invader(i_Game, k_InvaderPinkURL, Color.Pink, k_InvaderPinkPointsValue);
                    break;

                case eSpriteType.InvaderLightBlue:
                    objectToReturn = new Invader(i_Game, k_InvaderLightBlueURL, Color.LightBlue, k_InvaderLightBluePointsValue);
                    break;

                case eSpriteType.InvaderLightYellow:
                    objectToReturn = new Invader(i_Game, k_InvaderLightYellowURL, Color.LightYellow, k_InvaderLightYellowPointsValue);
                    break;

                case eSpriteType.Bullet:
                    objectToReturn = new Bullet(i_Game, k_BulletURL);
                    break;
            }

            return objectToReturn;
        }
    }
}