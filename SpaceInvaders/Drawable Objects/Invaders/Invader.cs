using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class Invader : Sprite, ICollidable2D, IShooter, IEnemy
    {
        private const string k_InvadersSpriteSheet = @"Sprites\Enemies";
        public const int k_DefaultInvaderWidth = 32;
        public const int k_DefaultInvaderHeight = 32;
        private const int k_MaxBulletsInScreen = 1;

        private readonly Gun r_Gun;
        public Color BulletsColor { get; } = Color.Blue;
        public int PointsValue { get; set; }

        private static Texture2D s_SharedTexture;

        public Invader(Game i_Game, Color i_Tint, int i_PointsValue) : base(k_InvadersSpriteSheet, i_Game)
        {
            TintColor = i_Tint;
            PointsValue = i_PointsValue;
            r_Gun = new Gun(this, k_MaxBulletsInScreen);
        }

        public void Shoot()
        {
            r_Gun.Shoot();
        }

        protected override void LoadTexture()
        { 
            // only once we load the shared texture to the graphics card
            if (s_SharedTexture == null)
            {
                s_SharedTexture = Game.Content.Load<Texture2D>(m_AssetName);
            }

            Texture = s_SharedTexture;
        }

        protected override void SpecificTextureBounds()
        {
            m_Width = k_DefaultInvaderWidth;
            m_Height = k_DefaultInvaderHeight;
        }
    }
}