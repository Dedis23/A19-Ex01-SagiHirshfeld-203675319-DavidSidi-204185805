using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderPink : Invader
    {
        private const string k_InvaderPinkURL = @"Sprites\Enemy0101_32x32";
        private const int k_InvaderPinkPointsValue = 260;

        public InvaderPink(Game i_Game) 
            : base(i_Game, k_InvaderPinkURL, Color.Pink, k_InvaderPinkPointsValue)
        {
        }
    }
}
