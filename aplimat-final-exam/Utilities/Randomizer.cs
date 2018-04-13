using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplimat_core.utilities
{
    public class Randomizer
    {
        private double min, max;
        private Random random;

        public Randomizer(double min, double max)
        {
            this.min = min;
            this.max = max + 1;
            random = new Random();
        }

        public double GenerateDouble()
        {
            return random.NextDouble() * (max - min) + min;
        }

        public int GenerateInt()
        {
            return (int)random.Next((int)min, (int)max);
        }
    }
}
