using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderPink : Invader
    {
        private const int k_InvaderPinkPointsValue = 260;

        public InvaderPink(Game i_Game, int i_StartingCellAnimationIndexInSpriteSheet) 
            : base(i_Game, Color.Pink, k_InvaderPinkPointsValue, i_StartingCellAnimationIndexInSpriteSheet)
        {
        }
    }
}
