////*** Guy Ronen © 2008-2011 ***////
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public abstract class GameService : RegisteredComponent
    {
        public GameService(Game i_Game, int i_UpdateOrder)
            : base(i_Game, i_UpdateOrder)
        {
            RegisterAsService(); // self-regsiter as a service
        }

        public GameService(Game i_Game)
            : base(i_Game)
        {
            RegisterAsService(); // self-regsiter as a service
        }

        /// <summary>
        /// This method register this component as a service in the game.
        /// It should be overriden in derived classes
        ///     if they want to register it with an interface 
        /// </summary>
        protected virtual void RegisterAsService()
        {
            AddServiceToGame(this.GetType());
        }

        protected void AddServiceToGame(Type i_Type)
        {
            GameServiceContainer gameServices = this.Game.Services;

            if (gameServices.GetService(i_Type) != null)
            {
                gameServices.RemoveService(i_Type);
            }

            gameServices.AddService(i_Type, this);
        }
    }
}
