using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplimat_final_exam.Models
{
    public class Movable
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public float Mass = 1;

        public void ApplyForce(Vector3 force)
        {
            // F = MA
            // A = F/M
            this.Acceleration += (force / Mass); //force accumulation
        }

        public void ApplyGravity(float scalar = 0.1f)
        {
            this.Acceleration += (new Vector3(0, -scalar * Mass, 0) / Mass);
        } 
    }
}
