using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplimat_core.utilities
{
    public static class Gaussian
    {
        public static double Generate(double mean = 0, double stdDev = 1)
        {
            Random r = new Random();

            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var randomStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + stdDev * randomStdNormal;
        }
    }
}
