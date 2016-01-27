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
        private string diseaseName_ = "Unnamed disease";
        public string diseaseName {
            get { return diseaseName_; }
            set { diseaseName_ = value; }
        }

        /** Subclasses have to give the disease a name. */
        public EyeDisease (string name) {
            diseaseName = name;
        }

        public abstract void showConfig(bool show);
        public abstract bool isConfigDisplayed();

        /* Use this for initialization */
        void Start () {
        }
        
        /* Update is called once per frame */
        void Update () {
        }
    }
}