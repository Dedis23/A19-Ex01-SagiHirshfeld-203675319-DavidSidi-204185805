using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Gun : GameComponent
    {
        public Gun(Game game) : base(game)
        {
        }

        public void Shoot(Vector2 i_From, eShootingDirection i_Direction, Color i_BulletColor)
        {
            Bullet newBullet = DrawableObjectsFactory.Create(this.Game, DrawableObjectsFactory.eSpriteType.Bullet) as Bullet;
            newBullet.Position = getCentralizedShootingPosition(newBullet, i_From, i_Direction);          

            newBullet.Tint = i_BulletColor;
            newBullet.Direction = i_Direction;
            this.Game.Components.Add(newBullet);
        }

        private Vector2 getCentralizedShootingPosition(Bullet i_Bullet, Vector2 i_From, eShootingDirection i_Direction)
        {
            float centralizedX = i_From.X - 0.5f * i_Bullet.Width;
            float centralizedY = i_Direction == eShootingDirection.Up ? i_From.Y - i_Bullet.Height : i_From.Y + i_Bullet.Height;
            return new Vector2(centralizedX, centralizedY);
        }
    }
}
