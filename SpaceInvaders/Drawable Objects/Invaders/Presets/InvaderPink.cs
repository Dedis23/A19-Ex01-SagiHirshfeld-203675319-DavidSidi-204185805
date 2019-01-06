using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class InvaderPink : Invader
    {
        private const int k_InvaderPinkPointsValue = 260;
        private const int k_ColIndexInSpriteSheet = 0;
        private const int k_RowIndexInSpriteSheet = 0;

        public InvaderPink(Game i_Game, int i_StartingCell) 
            : base(
                  i_Game,
                  Color.Pink,
                  k_InvaderPinkPointsValue,
                  (k_ColIndexInSpriteSheet + i_StartingCell) % Invader.k_NumOfCells,
                  k_RowIndexInSpriteSheet)
        {
        }
    }
}
