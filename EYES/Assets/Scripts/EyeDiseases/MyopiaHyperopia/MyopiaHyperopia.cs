using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

namespace eyediseases
{

    [RequireComponent (typeof(Camera))]
    [RequireComponent (typeof(DepthOfField))]
    public class MyopiaHyperopia : EyeDisease
    {
        public float diopters = -3.2f;

        private Camera playerCam = null;
        private DepthOfField dof = null;
        private Vector3 screenpoint = Vector3.zero;

        public MyopiaHyperopiaConfig ConfigDialog;

        public MyopiaHyperopia ()
            : base("Myopia")
        {
        }

    	// Use this for initialization
    	void Start () {
            Debug.Log ("MyopiaHyperopia::Start");

            playerCam = GetComponent<Camera> ();
            dof = GetComponent<DepthOfField> ();
            screenpoint = new Vector3(playerCam.pixelWidth / 2,
                playerCam.pixelHeight / 2,
                0.0f);

            dof.highResolution = true;
            dof.focalSize = 0.0f;
            dof.aperture = 3.36f;
            dof.blurType = DepthOfField.BlurType.DX11;
            dof.maxBlurSize = 12.83f;
            dof.nearBlur = true;
            dof.foregroundOverlap = 10.0f;

            ConfigDialog.mhSim = this;
            ConfigDialog.SetActive (false);
    	}
    	
    	// Update is called once per frame
    	void Update () {
            Ray viewray = playerCam.ScreenPointToRay(screenpoint);

            RaycastHit hitInfo = new RaycastHit ();
            const float maxDist = 10000.0f;
            float dist = maxDist;
            if (Physics.Raycast(viewray, out hitInfo)) {
                dist = hitInfo.distance;
                //Debug.Log ("Hit distance = " + dist);
            }

            // the cornea-retina distance
            float crd = 0.024f;
            float crd2 = crd / (1.0f - diopters * crd);

            //Debug.Log ("crd2 = " + crd2);

            float minF = 0.02068966f;
            float maxF = 0.024f;

            // compute the required focal length for the diseased eye and the given distance
            float f = dist * crd2 / (dist + crd2);

            //Debug.Log ("f = " + f);

            // if the lense can provide the focal length, approve.
            if ( minF <= f && f <= maxF) {
                dof.focalLength = dist;
            } else if (f < minF){
                // comput the distance capable with minF
                if (crd2 > minF) {
                    dof.focalLength = crd2 * minF / (crd2 - minF);
                } else {
                    dof.focalLength = maxDist;
                }
            } else if (maxF < f) {
                dof.focalLength = crd2 * maxF / (crd2 - maxF);
            }
    	}

        public void OnDisable () {
            dof.enabled = false;
        }

        public void OnEnable () {
            dof.enabled = true;
        }

        public override void showConfig () {
            ConfigDialog.SetActive (true);
        }

        protected override bool CheckResources () {
            return true;
        }
    }
}
