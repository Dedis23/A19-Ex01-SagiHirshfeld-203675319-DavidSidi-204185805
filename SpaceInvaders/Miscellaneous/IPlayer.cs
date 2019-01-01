using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    public interface IPlayer
    {
        int Score { get; }
        Color ScoreColor { get; }
        String Name { get; }
    }
}
