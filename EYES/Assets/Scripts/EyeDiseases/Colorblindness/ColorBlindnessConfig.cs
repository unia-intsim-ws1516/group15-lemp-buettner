using UnityEngine;
using System.Collections;

namespace eyediseases
{

    /**
     * This class is used to inform the color blindness object of value changes.
     * Probably this could be solved more elegantly by using delegates.
     */
    public class ColorBlindnessConfig : MonoBehaviour {

        public GameObject ConfigDialog;
        [HideInInspector] public eyediseases.ColorBlindnessSimulator cvdSim;

        public void Start () {
            ConfigDialog.SetActive (false);
        }


        public void OnProtanopiaClicked (bool enabled) {
            if (enabled) {
                Debug.Log ("Type is Protanopia");
                cvdSim.BlindMode = ColorBlindnessSimulator.ColorBlindMode.Protanope;
            }
        }

        public void OnDeuteranopiaClicked (bool enabled) {
            if (enabled) {
                Debug.Log ("Type is Deuteranopia");
                cvdSim.BlindMode = ColorBlindnessSimulator.ColorBlindMode.Deuteranope;
            }
        }

        public void OnSeverityChange (float severity) {
            Debug.Log ("Severity changed to " + severity.ToString());
            cvdSim.BlindIntensity = severity;
        }
    }

} /* namespace eyediseases */
