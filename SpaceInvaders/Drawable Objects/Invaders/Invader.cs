using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvaders
{
    public class Invader : Sprite, ICollidable2D, IShooter, IEnemy
    {
        private readonly Gun r_Gun;
        public Color BulletsColor { get; } = Color.Blue;
        public int PointsValue { get; set; }

        public Invader(Game i_Game, string i_AssetName, Color i_Tint, int i_PointsValue) : base(i_AssetName, i_Game)
        {
            TintColor = i_Tint;
            PointsValue = i_PointsValue;
            r_Gun = new Gun(this);
        }

        public void Shoot()
        {
            r_Gun.Shoot();
        }
    }
}