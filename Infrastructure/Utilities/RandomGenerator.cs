using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    public class RandomGenerator : Random
    {
        private static readonly object sr_CreationLock = new object();
        private static RandomGenerator s_Instance;

        private RandomGenerator() : base((int)DateTime.Now.Ticks)
        {
        }

        public static RandomGenerator Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (sr_CreationLock)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new RandomGenerator();
                        }
                    }
                }

                return s_Instance;
            }
        }

        public float NextFloat(int i_MinValue, int i_MaxValue)
        {
            return Next(i_MinValue, i_MaxValue - 1) + Next(1, 100) / 100f;
        }
    }
}
