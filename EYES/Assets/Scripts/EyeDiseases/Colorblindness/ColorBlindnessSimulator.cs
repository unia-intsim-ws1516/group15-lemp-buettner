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

        public GameObject ConfigDialog;

        private DiscreteFunction L;
        private DiscreteFunction M;
        private DiscreteFunction S;

        public ColorBlindnessSimulator ()
            : base("Protanopia")
        {
            Debug.Log ("ColorBlindnessSimulator::ctor");
        }

        void Awake () {
            Debug.Log ("ColorBlindnessSimulator::Awake");
        }

        public void Start () {
            Debug.Log ("ColorBlindnessSimulator::Start");
            ConfigDialog.SetActive (false);
            ConfigDialog.GetComponent<ColorBlindnessConfig>().cvdSim = this;
        }

        #region Overrides


        public override void showConfig () {
            ConfigDialog.SetActive (true);
        }
        
        protected override bool CheckResources ()
        {
            CheckSupport (false);
            BrettelShader = Shader.Find ("Hidden/CVDBrettel");
            MachadoShader = Shader.Find ("Hidden/CVDMachado");
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