using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace model_metro
{
    internal static class RandomHelper
    {
        private static Random random = new Random();
        private static bool hasSpare = false;
        private static double spare;

        public static double NextGaussian(double mean, double stdDev)
        {
            if (hasSpare)
            {
                hasSpare = false;
                return mean + stdDev * spare;
            }
            else
            {
                double u, v, s;
                do
                {
                    u = 2 * random.NextDouble() - 1;
                    v = 2 * random.NextDouble() - 1;
                    s = u * u + v * v;
                } while (s >= 1 || s == 0);

                s = Math.Sqrt(-2 * Math.Log(s) / s);
                spare = v * s;
                hasSpare = true;
                return mean + stdDev * u * s;
            }
        }
    }
}

