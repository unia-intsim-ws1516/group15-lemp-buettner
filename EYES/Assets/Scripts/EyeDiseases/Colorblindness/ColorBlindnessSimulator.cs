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
        }
        [SerializeField]
        public ColorBlindMode
            BlindMode_ = ColorBlindMode.Protanope;

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
        public float
            BlindIntensity = 1.0f;
        [SerializeField]
        public Shader BrettelShader;
        public Shader MachadoShader;
        private Material ColorBlindMat;

        public enum ColorBlindAlgorithm
        {
            Brettel,
            Machado,
        }
        public ColorBlindAlgorithm BlindAlgorithm = ColorBlindAlgorithm.Machado;

        public ColorBlindnessConfig ConfigDialog;

        private DiscreteFunction L = new DiscreteFunction ();
        private DiscreteFunction M = new DiscreteFunction ();
        private DiscreteFunction S = new DiscreteFunction ();

        public ColorBlindnessSimulator ()
            : base("Protanopia")
        {
            Debug.Log ("ColorBlindnessSimulator::ctor");
        }

        void Awake () {
            Debug.Log ("ColorBlindnessSimulator::Awake");

            // Load the responsivity functions
            string text = System.IO.File.ReadAllText("responsivityFunctions/ciexyz31.csv");
            string[] lines = text.Split("\n"[0]);

            L.values.Clear ();
            L.values.Capacity = lines.Length;
            M.values.Clear ();
            M.values.Capacity = lines.Length;
            S.values.Clear ();
            S.values.Capacity = lines.Length;

            for (int i = 0; i < lines.Length; ++i) {
                string[] dataText = lines[i].Split(","[0]);
                Debug.Assert (dataText.Length >= 4);

                if (0 == i) {
                    float.TryParse (dataText[0], out L.minX);
                } else if ((lines.Length - 1) == i) {
                    float.TryParse (dataText[0], out L.maxX);
                }

                float tmp = 0.0f;
                float.TryParse (dataText[1], out tmp);
                L.values.Add (tmp);

                float.TryParse (dataText[2], out tmp);
                M.values.Add (tmp);

                float.TryParse (dataText[3], out tmp);
                S.values.Add (tmp);
            }

            // normalize the curves
            float maxF = float.MinValue;
            foreach (float v in L.values) { maxF = Mathf.Max (maxF, v);  }
            for (int i = 0; i < L.values.Count; ++i) { L.values[i] /= maxF; }

            maxF = float.MinValue;
            foreach (float v in M.values) { maxF = Mathf.Max (maxF, v);  }
            for (int i = 0; i < M.values.Count; ++i) { M.values[i] /= maxF; }

            maxF = float.MinValue;
            foreach (float v in S.values) { maxF = Mathf.Max (maxF, v);  }
            for (int i = 0; i < S.values.Count; ++i) { S.values[i] /= maxF; }
        }

        public void Start () {
            Debug.Log ("ColorBlindnessSimulator::Start");
            ConfigDialog.cvdSim = this;
            ConfigDialog.SetActive (false);
        }

        #region Overrides


        public override void showConfig () {
            ConfigDialog.SetLCurve (L);
            ConfigDialog.SetMCurve (M);
            ConfigDialog.SetSCurve (S);
            ConfigDialog.SetActive (true);
        }
        
        protected override bool CheckResources ()
        {
            CheckSupport (false);
//            BrettelShader = Shader.Find ("Hidden/CVDBrettel");
//            MachadoShader = Shader.Find ("Hidden/CVDMachado");
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

            switch (BlindMode) {
            case ColorBlindMode.Protanope:
                ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_ONE" };
                break;
            case ColorBlindMode.Deuteranope:
                ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_TWO" };
                break;
            }

            switch (BlindAlgorithm) {
            case ColorBlindAlgorithm.Brettel:
                ColorBlindMat.shader = BrettelShader;
                break;
            case ColorBlindAlgorithm.Machado:
                ColorBlindMat.shader = MachadoShader;
                break;
            }

            // Intensity Set
            ColorBlindMat.SetFloat ("_BlindIntensity", BlindIntensity);

            Graphics.Blit (_src, _dst, ColorBlindMat);
        }

        #endregion
    }
}