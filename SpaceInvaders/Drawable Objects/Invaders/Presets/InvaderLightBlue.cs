using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderLightBlue : Invader
    {
        private const int k_InvaderLightBluePointsValue = 140;
        private const int k_ColIndexInSpriteSheet = 2;
        private const int k_RowIndexInSpriteSheet = 1;

        public InvaderLightBlue(Game i_Game, int i_StartingCell)
            : base(
                  i_Game,
                  Color.LightBlue,
                  k_InvaderLightBluePointsValue,
                  (k_ColIndexInSpriteSheet + i_StartingCell) % Invader.k_NumOfCells,
                  k_RowIndexInSpriteSheet)
        {
        }
    }
}
