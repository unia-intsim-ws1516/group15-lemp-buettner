
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace eyediseases
{
    public class GlaucomaSimulator : EyeDisease
    {
        [SerializeField]
        public float MaxRadius = 0.01f; // in uv-coordinates units
        [SerializeField]
        public Shader ColorBlindShader;
        protected Material ColorBlindMat;

        public GameObject ConfigDialog;

        public Texture2D SeverityTex;

        public GlaucomaSimulator ()
            : base("Glaucoma")
        {
        }

        public void Start () {
            //ConfigDialog.SetActive (false);
            //ConfigDialog.GetComponent<GlaucomaConfig>().cvdSim = this;
        }

        #region Overrides


        public override void showConfig () {
            //ConfigDialog.SetActive (true);
        }

        protected override bool CheckResources ()
        {
            CheckSupport (false);
            ColorBlindShader = Shader.Find ("Hidden/GlaucomaSimulator");
            ColorBlindMat = CreateMaterial (ColorBlindShader, ColorBlindMat);

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

//            switch (BlindMode) {
//            case ColorBlindMode.Protanope:
//                ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_ONE" };
//                break;
//            case ColorBlindMode.Deuteranope:
//                ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_TWO" };
//                break;
//            }
//
//            switch (BlindAlgorithm) {
//            case ColorBlindAlgorithm.GULTI:
//                ColorBlindShader = Shader.Find ("Hidden/GULTI/ColorBlindSimulator");
//                break;
//            case ColorBlindAlgorithm.IntSim:
//                ColorBlindShader = Shader.Find ("Hidden/GULTI/CVDSimulator");
//                break;
//            }
//            ColorBlindMat.shader = ColorBlindShader;
//
//            // Intensity Set
//            ColorBlindMat.SetFloat ("_BlindIntensity", BlindIntensity);
            ColorBlindMat.SetFloat("_Radius", MaxRadius);
            if (SeverityTex)
            {
                ColorBlindMat.SetTexture("_BlurIntensity", SeverityTex);
            }

            Graphics.Blit (_src, _dst, ColorBlindMat);
        }

        #endregion
    }
}