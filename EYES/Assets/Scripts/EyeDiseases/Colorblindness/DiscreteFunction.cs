using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace eyediseases {

    public class DiscreteFunction {

        public List<float> values = new List<float> ();
        public float minX = 0.0f;
        public float maxX = 1.0f;

        public float Stepsize () {
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

        /**
         * csvXColumn and csvYColumn are 0-based.
         */
        public void LoadFromCSV (string path, int csvXColumn, int csvYColumn) {

            Debug.Assert (csvXColumn >= 0);
            Debug.Assert (csvYColumn >= 0);

            // Load the responsivity functions
            string text = System.IO.File.ReadAllText(path);
            string[] lines = text.Split("\n"[0]);

            values.Clear ();
            values.Capacity = lines.Length;

            for (int i = 0; i < lines.Length; ++i) {
                string[] dataText = lines[i].Split(","[0]);
                Debug.Assert (dataText.Length > Mathf.Max(csvXColumn, csvYColumn));

                if (0 == i) {
                    float.TryParse (dataText[csvXColumn], out minX);
                } else if ((lines.Length - 1) == i) {
                    float.TryParse (dataText[csvXColumn], out maxX);
                }

                float tmp = 0.0f;
                float.TryParse (dataText[csvYColumn], out tmp);
                values.Add (tmp);
            }

            // normalize the curve
            float maxF = float.MinValue;
            foreach (float v in values) { maxF = Mathf.Max (maxF, v);  }
            for (int i = 0; i < values.Count; ++i) { values[i] /= maxF; }
        }

        public static DiscreteFunction operator * (float c, DiscreteFunction f) {
            DiscreteFunction g = new DiscreteFunction (f);
            for (int i = 0; i < g.values.Count; ++i) {
                g.values[i] = c * f.values[i];
            }
            return g;
        }

        public static DiscreteFunction operator * (DiscreteFunction f, DiscreteFunction g) {
            Debug.Assert (f.values.Count == g.values.Count);
            /* This could be dropped, if the function was assumed to be
             * extended by 0 where no values are given. */
            Debug.Assert (f.minX == g.minX);
            Debug.Assert (f.maxX == g.maxX);

            DiscreteFunction h = new DiscreteFunction ();
            for (int i = 0; i < f.values.Count; ++i) {
                h.values.Add (f.values[i] * g.values[i]);
            }

            return h;
        }

        /** Integrates tho function from minX to maxX. */
        public float integral () {
            float I = 0.0f;
            for (int i = 0; i < values.Count - 2; i += 2) {
                I += values[i] + 4.0f * values[i+1] + values[i+2];
            }

            return I * Stepsize() / 3.0f;
        }
    }

}