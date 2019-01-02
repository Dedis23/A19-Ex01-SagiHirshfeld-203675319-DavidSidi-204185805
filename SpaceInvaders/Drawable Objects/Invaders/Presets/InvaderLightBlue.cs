using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderLightBlue : Invader
    {
        private const int k_InvaderLightBluePointsValue = 140;
        public InvaderLightBlue(Game i_Game, int i_StartingCellAnimationIndexInSpriteSheet)
            : base(i_Game, Color.LightBlue, k_InvaderLightBluePointsValue, i_StartingCellAnimationIndexInSpriteSheet)
        {
        }
    }
}
