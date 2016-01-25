using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

namespace eyediseases
{
    [RequireComponent (typeof(ScreenOverlay))]
    [RequireComponent (typeof(BlurOptimized))]
    public class Cateract : EyeDisease {

        public Texture2D TintTexture = null;
        private ScreenOverlay tint = null;
        private BlurOptimized blur = null;

        public Cateract ()
            : base ("Cateract")
        {}

        void Awake () {
            tint = GetComponent<ScreenOverlay> ();
            tint.texture = TintTexture;
            tint.blendMode = ScreenOverlay.OverlayBlendMode.ScreenBlend;
            tint.intensity = 0.3f;

            blur = GetComponent<BlurOptimized> ();
            blur.blurType = BlurOptimized.BlurType.StandardGauss;
            blur.blurIterations = 1;
            blur.downsample = 0;
            blur.blurSize = 2.0f;
        }

        public override void showConfig () {
        }

        protected override bool CheckResources () {
            return true;
        }

        void OnEnable () {
            tint.enabled = true;
            blur.enabled = true;
        }

        void OnDisable () {
            tint.enabled = false;
            blur.enabled = false;
        }
    }
}
