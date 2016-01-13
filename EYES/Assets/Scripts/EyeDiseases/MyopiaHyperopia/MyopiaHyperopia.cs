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

        private Camera camera = null;
        private DepthOfField dof = null;
        private Vector3 screenpoint = Vector3.zero;
        private Vector2 focalDistanceRange = new Vector2 (0.15f, float.MaxValue);

        public MyopiaHyperopia ()
            : base("Myopia")
        {
        }

    	// Use this for initialization
    	void Start () {
            camera = GetComponent<Camera> ();
            dof = GetComponent<DepthOfField> ();
            screenpoint = new Vector3(camera.pixelWidth / 2,
                camera.pixelHeight / 2,
                0.0f);
    	}
    	
    	// Update is called once per frame
    	void Update () {
            Ray viewray = camera.ScreenPointToRay(screenpoint);

            RaycastHit hitInfo = new RaycastHit ();
            float dist = 10000.0f;
            if (Physics.Raycast(viewray, out hitInfo)) {
                dist = hitInfo.distance;
                Debug.Log ("Hit distance = " + dist);
            }

            // the cornea-retina distance
            float crd = 0.024f;
            float crd2 = crd / (1.0f - diopters * crd);

            float minF = 0.02068966f;
            float maxF = 0.024f;

            // compute the required focal length for the diseased eye and the given distance
            float f = dist * crd2 / (dist + crd2);

            Debug.Log ("f = " + f);

            // if the lense can provide the focal length, approve.
            if ( minF <= f && f <= maxF) {
                dof.focalLength = dist;
            } else if (f < minF){
                // comput the distance capable with minF
                dof.focalLength = crd2 * minF / (crd2 - minF);
            } else if (maxF < f) {
                dof.focalLength = crd2 * maxF / (crd2 - maxF);
            }
    	}

        public override void showConfig () {
        }

        protected override bool CheckResources () {
            return true;
        }
    }

}
