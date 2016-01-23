using UnityEngine;
using System.Collections;

namespace eyediseases {

    public class DiscreteFunction {

        public double[] values;
        public double minX = 0.0;
        public double maxX = 1.0;

        public double Stepsize () {
            return (maxX - minX) / (values.Length - 1);
        }

        public DiscreteFunction (DiscreteFunction f) {
            values = (double[])f.values.Clone ();
            minX = f.minX;
            maxX = f.maxX;
        }

        public DiscreteFunction (double[] values, double minX, double maxX) {
            this.values = (double[])values.Clone ();
            this.minX = minX;
            this.maxX = maxX;
        }

        public static DiscreteFunction operator * (double c, DiscreteFunction f) {
            DiscreteFunction g = new DiscreteFunction (f);
            for (int i = 0; i < g.values.Length; ++i) {
                g.values[i] = c * f.values[i];
            }
            return g;
        }
    }

}