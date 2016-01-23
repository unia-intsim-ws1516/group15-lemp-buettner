using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace eyediseases {

    public class DiscreteFunction {

        public List<double> values = new List<double> ();
        public double minX = 0.0;
        public double maxX = 1.0;

        public double Stepsize () {
            if (values.Count > 0) {
                return (maxX - minX) / (values.Count - 1);
            } else {
                return 0.0f;
            }
        }

        public DiscreteFunction () {
            
        }

        public DiscreteFunction (DiscreteFunction f) {
            values.Clear ();
            values.AddRange (f.values);
            //values = (List<double>)f.values.Clone ();
            minX = f.minX;
            maxX = f.maxX;
        }

//        public DiscreteFunction (double[] values, double minX, double maxX) {
//            //this.values = (List<double>)values.Clone ();
//            this.values.Clear ();
//            this.values.AddRange (values);
//            this.minX = minX;
//            this.maxX = maxX;
//        }

        public static DiscreteFunction operator * (double c, DiscreteFunction f) {
            DiscreteFunction g = new DiscreteFunction (f);
            for (int i = 0; i < g.values.Count; ++i) {
                g.values[i] = c * f.values[i];
            }
            return g;
        }
    }

}