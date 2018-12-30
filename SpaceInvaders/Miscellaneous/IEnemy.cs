using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ObjectModel;

namespace SpaceInvaders
{
    public interface IEnemy : IKillable
    {
        int PointsValue { get; }
    }
}
