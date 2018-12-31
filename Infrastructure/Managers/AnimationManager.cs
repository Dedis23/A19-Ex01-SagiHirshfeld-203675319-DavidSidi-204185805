using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Infrastructure.ServiceInterfaces;

namespace Infrastructure.Managers
{
    public class AnimationManager : GameService, IAnimationManager
    {
        protected readonly List<IAnimated> r_AnimationsObjects = new List<IAnimated>();

        public AnimationManager(Game i_Game) :
            base(i_Game, int.MaxValue)
        {}

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(IAnimationManager), this);
        }

        public void AddObjectToMonitor(IAnimated i_Animated)
        {
            if (!this.r_AnimationsObjects.Contains(i_Animated))
            {
                this.r_AnimationsObjects.Add(i_Animated);
            }
        }
    }
}
