using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderLightYellow : Invader
    {
        private const int k_InvaderLightYellowPointsValue = 110;
        private const int k_ColIndexInSpriteSheet = 4;
        private const int k_RowIndexInSpriteSheet = 2;

        public InvaderLightYellow(Game i_Game, int i_StartingCell) 
            : base(i_Game,
                  Color.LightYellow,
                  k_InvaderLightYellowPointsValue,
                  (k_ColIndexInSpriteSheet + i_StartingCell) % Invader.k_NumOfCells,
                  k_RowIndexInSpriteSheet)
        {}
    }
}
