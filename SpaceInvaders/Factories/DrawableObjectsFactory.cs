using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

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

        private const string k_InvaderPinkURL = @"Sprites\Enemy0101_32x32";
        private const string k_InvaderLightBlueURL = @"Sprites\Enemy0201_32x32";
        private const string k_InvaderLightYellowURL = @"Sprites\Enemy0301_32x32";
        private const int k_InvaderPinkPointsValue = 260;
        private const int k_InvaderLightBluePointsValue = 140;
        private const int k_InvaderLightYellowPointsValue = 110;

        public static Sprite Create(Game i_Game, eSpriteType i_ObjectType)
        {
            Sprite objectToReturn = null;
            switch (i_ObjectType)
            {
                case eSpriteType.SpaceBG:
                    objectToReturn = new SpaceBG(i_Game);
                    break;

                case eSpriteType.Spaceship:
                    objectToReturn = new Spaceship(i_Game);
                    break;

                case eSpriteType.Mothership:
                    objectToReturn = new Mothership(i_Game);
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
                    objectToReturn = new Bullet(i_Game);
                    break;
            }

            return objectToReturn;
        }
    }
}