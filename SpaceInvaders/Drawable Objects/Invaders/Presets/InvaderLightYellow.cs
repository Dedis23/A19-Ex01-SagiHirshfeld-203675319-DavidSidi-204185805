using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderLightYellow : Invader
    {
        private const int k_InvaderLightYellowPointsValue = 110;

        public InvaderLightYellow(Game i_Game, int i_StartingCellAnimationIndexInSpriteSheet) 
            : base(i_Game, Color.LightYellow, k_InvaderLightYellowPointsValue, i_StartingCellAnimationIndexInSpriteSheet)
        {
        }
    }
}
