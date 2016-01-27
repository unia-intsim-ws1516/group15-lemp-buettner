using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace eyediseases
{
    public class ColorBlindnessSimulator : EyeDisease
    {
        public enum ColorBlindMode
        {
            Protanope,
            Deuteranope,
            Tritanope,
        }
        [SerializeField]
        public ColorBlindMode BlindMode_ = ColorBlindMode.Protanope;

        public ColorBlindMode BlindMode {
            get { return BlindMode_; }
            set {
                BlindMode_ = value;
                switch (BlindMode_) {
                case ColorBlindMode.Protanope:
                    name = "Protanopia";
                    break;
                case ColorBlindMode.Deuteranope:
                    name = "Deuteranopia";
                    break;
                }
            }
        }

        [SerializeField]
        public float BlindIntensity = 1.0f;
        [SerializeField]
        public Shader BrettelShader;
        public Shader MachadoShader;
        private Material ColorBlindMat;

        public enum ColorBlindAlgorithm
        {
            Brettel,
            Machado,
        }

        public ColorBlindAlgorithm BlindAlgorithm = ColorBlindAlgorithm.Brettel;

        public ColorBlindnessConfig ConfigDialog;

        private DiscreteFunction Loriginal = new DiscreteFunction ();
        private DiscreteFunction Moriginal = new DiscreteFunction ();
        private DiscreteFunction Soriginal = new DiscreteFunction ();

        private DiscreteFunction L = new DiscreteFunction ();
        private DiscreteFunction M = new DiscreteFunction ();
        private DiscreteFunction S = new DiscreteFunction ();

        private DiscreteFunction X = new DiscreteFunction ();
        private DiscreteFunction Y = new DiscreteFunction ();
        private DiscreteFunction Z = new DiscreteFunction ();

        private Matrix4x4 XYZ2RGB = new Matrix4x4 ();
        private Matrix4x4 GammaNormal = new Matrix4x4 ();

        public ColorBlindnessSimulator ()
            : base("Color vision deficiency")
        {
            XYZ2RGB.SetRow (0, new Vector4  (0.4124f,  0.3576f,  0.1805f, 0.0f));
            XYZ2RGB.SetRow (1, new Vector4 ( 0.2126f,  0.7152f,  0.0722f, 0.0f));
            XYZ2RGB.SetRow (2, new Vector4 ( 0.0193f,  0.1192f,  0.9505f, 0.0f));
            XYZ2RGB.SetRow (3, new Vector4 (    0.0f,     0.0f,     0.0f, 1.0f));
        }

        void Awake () {

            L.LoadFromCSV ("responsivityFunctions/linss10e_5.csv", 0, 1);
            M.LoadFromCSV ("responsivityFunctions/linss10e_5.csv", 0, 2);
            S.LoadFromCSV ("responsivityFunctions/linss10e_5.csv", 0, 3);

            Loriginal.LoadFromCSV ("responsivityFunctions/linss10e_5.csv", 0, 1);
            Moriginal.LoadFromCSV ("responsivityFunctions/linss10e_5.csv", 0, 2);
            Soriginal.LoadFromCSV ("responsivityFunctions/linss10e_5.csv", 0, 3);

            X.LoadFromCSV ("responsivityFunctions/ciexyz31.csv", 0, 1);
            Y.LoadFromCSV ("responsivityFunctions/ciexyz31.csv", 0, 2);
            Z.LoadFromCSV ("responsivityFunctions/ciexyz31.csv", 0, 3);

            GammaNormal[0,0] = (L * X).integral();
            GammaNormal[0,1] = (L * Y).integral();
            GammaNormal[0,2] = (L * Z).integral();
            GammaNormal[0,3] = 0.0f;
            GammaNormal[1,0] = (M * X).integral();
            GammaNormal[1,1] = (M * Y).integral();
            GammaNormal[1,2] = (M * Z).integral();
            GammaNormal[1,3] = 0.0f;
            GammaNormal[2,0] = (S * X).integral();
            GammaNormal[2,1] = (S * Y).integral();
            GammaNormal[2,2] = (S * Z).integral();
            GammaNormal[2,3] = 0.0f;
            GammaNormal.SetRow (3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

            GammaNormal = GammaNormal * XYZ2RGB;

            if (ConfigDialog != null) {
                ConfigDialog.cvdSim = this;
                ConfigDialog.SetActive (false);
            }
        }

        public void Start () {
        }

        #region Overrides


        public override void showConfig (bool show) {
            if (ConfigDialog == null)
                return;
            
            if (show) {
                // Important to setActive(true) first in order to get the scripts executed
				ConfigDialog.SetActive (true);
				ConfigDialog.SetLCurve (L);
				ConfigDialog.SetMCurve (M);
				ConfigDialog.SetSCurve (S); 

            } else {
                ConfigDialog.SetActive (false);
            }
        }

        public override bool isConfigDisplayed () {
            if (ConfigDialog == null)
                return false;
            
            return ConfigDialog.isActiveAndEnabled;
        }

        public void ResetL () {
            for (int i = 0; i < L.values.Count; ++i) {
                L.values[i] = Loriginal.values[i];
            }
			if (ConfigDialog != null) {
				ConfigDialog.SetLCurve (L);
			}
        }

        public void ResetM () {
            for (int i = 0; i < M.values.Count; ++i) {
                M.values[i] = Moriginal.values[i];
            }
			if (ConfigDialog != null) {
				ConfigDialog.SetMCurve (M);
			}
        }

        public void ResetS () {
            for (int i = 0; i < S.values.Count; ++i) {
                S.values[i] = Soriginal.values[i];
            }
			if (ConfigDialog != null) {
				ConfigDialog.SetSCurve (S);
			}
        }
        
        protected override bool CheckResources ()
        {
            CheckSupport (false);
            ColorBlindMat = CreateMaterial (MachadoShader, ColorBlindMat);

            return ColorBlindMat != null;
        }
        
        #endregion

        #region Monobehavior

        void OnDisable ()
        {
            if (ColorBlindMat != null) {
#if UNITY_EDITOR
                if(!UnityEditor.EditorApplication.isPlaying)
                    DestroyImmediate(ColorBlindMat, true);
                else
#endif
                Destroy (ColorBlindMat);
            }
        }

        void OnRenderImage (RenderTexture _src, RenderTexture _dst)
        {
            if (ColorBlindMat == null) {
                if (!CheckResources ()) {
                    NotSupported ();
                    return;
                }
            }

            if (ColorBlindAlgorithm.Brettel == BlindAlgorithm) {

                ColorBlindMat.shader = BrettelShader;

                switch (BlindMode) {
                case ColorBlindMode.Protanope:
                    ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_ONE" };
                    break;
                case ColorBlindMode.Deuteranope:
                    ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_TWO" };
                    break;
                }

                // Intensity Set
                ColorBlindMat.SetFloat ("_BlindIntensity", BlindIntensity);

            } else if (ColorBlindAlgorithm.Machado == BlindAlgorithm) {
                
                ColorBlindMat.shader = MachadoShader;

                Matrix4x4 Gamma = new Matrix4x4 ();
                Gamma[0,0] = (L * X).integral();
                Gamma[0,1] = (L * Y).integral();
                Gamma[0,2] = (L * Z).integral();
                Gamma[0,3] = 0.0f;
                Gamma[1,0] = (M * X).integral();
                Gamma[1,1] = (M * Y).integral();
                Gamma[1,2] = (M * Z).integral();
                Gamma[1,3] = 0.0f;
                Gamma[2,0] = (S * X).integral();
                Gamma[2,1] = (S * Y).integral();
                Gamma[2,2] = (S * Z).integral();
                Gamma[2,3] = 0.0f;
                Gamma.SetRow (3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

                Matrix4x4 CVD = GammaNormal.inverse * Gamma * XYZ2RGB;

                Vector4 tmp = CVD.GetRow (0);
                float sum = (tmp.x + tmp.y + tmp.z);
                CVD.SetRow (0, tmp / sum);

                tmp = CVD.GetRow (1);
                sum = (tmp.x + tmp.y + tmp.z);
                CVD.SetRow (1, tmp / sum);

                tmp = CVD.GetRow (2);
                sum = (tmp.x + tmp.y + tmp.z);
                CVD.SetRow (2, tmp / sum);

                ColorBlindMat.SetMatrix ("_CVD", CVD);
            }

            Graphics.Blit (_src, _dst, ColorBlindMat);
        }

        #endregion
    }
}