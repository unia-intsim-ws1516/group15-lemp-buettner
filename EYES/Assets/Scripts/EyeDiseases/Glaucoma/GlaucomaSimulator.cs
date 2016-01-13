
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace eyediseases
{
    public class GlaucomaSimulator : EyeDisease
    {
        [SerializeField]
        [Range(0.0f, 16.0f)]
        public float BlurSize = 1.0f;
        [SerializeField]
        [Range(0, 8)]
        public int BlurIterations = 3;

        public GameObject ConfigDialog;
        public Texture2D Intensity;
        public Shader GlaucomaShader;
        private Material GlaucomaMaterial = null;

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

            GlaucomaMaterial = CreateMaterial (GlaucomaShader, GlaucomaMaterial);
            if (GlaucomaMaterial == null)
            {
                Debug.LogError("Failed to create BlurMaterial");
                return false;
            }

            return true;
        }

        #endregion

        #region Monobehavior

        void OnDisable ()
        {
            if (GlaucomaMaterial != null) {
                #if UNITY_EDITOR
                if(!UnityEditor.EditorApplication.isPlaying)
                    DestroyImmediate(GlaucomaMaterial, true);
                else
                #endif
                    Destroy (GlaucomaMaterial);
            }
        }

        void OnRenderImage (RenderTexture _src, RenderTexture _dst)
        {
            if (GlaucomaMaterial == null) {
                if (!CheckResources ()) {
                    NotSupported ();
                    return;
                }
            }

            GlaucomaMaterial.SetVector ("_Parameter", new Vector4 (BlurSize, -BlurSize, 0.0f, 0.0f));
            GlaucomaMaterial.SetTexture ("_IntensityMask", Intensity);
            _src.filterMode = FilterMode.Bilinear;

            // downsample
            RenderTexture rt = RenderTexture.GetTemporary (_src.width, _src.height, 0, _src.format);

            rt.filterMode = FilterMode.Bilinear;
            Graphics.Blit (_src, rt, GlaucomaMaterial, 0);

            const int gaussianPass = 0;

            for(int i = 0; i < BlurIterations; i++) {
                float iterationOffs = (i*1.0f);
                GlaucomaMaterial.SetVector ("_Parameter", new Vector4 (BlurSize + iterationOffs, -BlurSize - iterationOffs, 0.0f, 0.0f));

                // vertical blur
                RenderTexture rt2 = RenderTexture.GetTemporary (_src.width, _src.height, 0, _src.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit (rt, rt2, GlaucomaMaterial, 1 + gaussianPass);
                RenderTexture.ReleaseTemporary (rt);
                rt = rt2;

                // horizontal blur
                rt2 = RenderTexture.GetTemporary (_src.width, _src.height, 0, _src.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit (rt, rt2, GlaucomaMaterial, 2 + gaussianPass);
                RenderTexture.ReleaseTemporary (rt);
                rt = rt2;
            }

            Graphics.Blit (rt, _dst);

            RenderTexture.ReleaseTemporary (rt);
        }

        #endregion
    }
}