using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace eyediseases {

    public class MyopiaHyperopiaConfig : MonoBehaviour {

        [HideInInspector] public MyopiaHyperopia mhSim;

        public Text CurrentValueLable;

        void Awake () {
            Debug.Log ("MyopiaHyperopiaConfig::Awake");
        }

        public void Start () {
            Debug.Log ("MyopiaHyperopiaConfig::Start");
        }

        public void SetActive (bool active) {
            gameObject.SetActive (active);
        }

        public void OnFocalLengthChange (float diopters) {
            mhSim.diopters = 2.0f * (diopters - 0.5f) * 8.0f;
            CurrentValueLable.text = mhSim.diopters.ToString ();
        }

    }

}
