using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderLightYellow : Invader
    {
        private const string k_InvaderLightYellowURL = @"Sprites\Enemy0301_32x32";
        private const int k_InvaderLightYellowPointsValue = 110;

        public InvaderLightYellow(Game i_Game) 
            : base(i_Game, k_InvaderLightYellowURL, Color.LightYellow, k_InvaderLightYellowPointsValue)
        {
        }
    }
}
