using UnityEngine;
using System.Collections;

namespace eyediseases
{
    [RequireComponent (typeof(ImageBasedBlur))]
    public class Glaucoma : EyeDisease {

        public Texture2D BlurIntensity = null;
        private ImageBasedBlur blur = null;

        Glaucoma ()
            : base("Glaucoma")
        {}

        void Awake () {
            blur = GetComponent<ImageBasedBlur> ();
            blur.BlurIntensity = BlurIntensity;
        }


        public override void showConfig () {
        }

        public override void hideConfig () {
        }

        protected override bool CheckResources () {
            return true;
        }

        void OnEnable () {
            blur.enabled = true;
        }

        void OnDisable () {
            blur.enabled = false;
        }
    }
}
