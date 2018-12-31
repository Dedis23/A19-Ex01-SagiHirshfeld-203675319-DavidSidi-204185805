using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderLightBlue : Invader
    {
        private const string k_InvaderLightBlueURL = @"Sprites\Enemy0201_32x32";
        private const int k_InvaderLightBluePointsValue = 140;
        public InvaderLightBlue(Game i_Game) 
            : base(i_Game, k_InvaderLightBlueURL, Color.LightBlue, k_InvaderLightBluePointsValue)
        {
        }
    }
}
