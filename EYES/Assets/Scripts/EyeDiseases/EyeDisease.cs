using UnityEngine;
using System.Collections;

namespace eyediseases
{

    /**
     * Base class of all eye diseases used in this project.
     * 
     * Every disease has to have certain features like a name,
     * a method for generating a settings dialog and the same
     * base class.
     */
    public abstract class EyeDisease : PostEffectsBase
    {

        public string diseaseName = "Unnamed disease";

        public EyeDisease (string name) {
            diseaseName = name;
        }

        // Use this for initialization
        void Start () {
        }
        
        // Update is called once per frame
        void Update () {
        }
    }
}