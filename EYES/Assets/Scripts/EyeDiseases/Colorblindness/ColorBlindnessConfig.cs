using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace eyediseases
{

    /**
     * This class is used to inform the color blindness object of value changes.
     * Probably this could be solved more elegantly by using delegates.
     */
    public class ColorBlindnessConfig : MonoBehaviour {

        public Toggle MachadoChk;
        public Toggle BrettelChk;
        public Toggle ProtanopiaChk;
        public Toggle DeuteranopiaChk;
        public Toggle TritanopiaChk;
        public Scrollbar SeverityScroll;
        public Toggle LChk;
        public Toggle MChk;
        public Toggle SChk;
        public GameObject Graph;

        [HideInInspector] public ColorBlindnessSimulator cvdSim;
        [HideInInspector] public Grapher grapher;

        void Awake () {
            Debug.Log ("ColorBlindnessConfig::Awake");
            MachadoChk.isOn = false;
            MachadoChk.interactable = false;
            BrettelChk.isOn = false;
            BrettelChk.interactable = false;
            ProtanopiaChk.isOn = false;
            ProtanopiaChk.interactable = false;
            DeuteranopiaChk.isOn = false;
            DeuteranopiaChk.interactable = false;
            TritanopiaChk.isOn = false;
            TritanopiaChk.interactable = false;
            SeverityScroll.interactable = false;
            LChk.isOn = false;
            LChk.interactable = false;
            MChk.isOn = false;
            MChk.interactable = false;
            SChk.isOn = false;
            SChk.interactable = false;
            Graph.SetActive (false);
            grapher = Graph.GetComponent<Grapher> ();
        }

        public void Start () {
            Debug.Log ("ColorBlindnessConfig::Start");
        }

        public void SetLCurve (DiscreteFunction L) {
            grapher.SetLCurve(L);
        }

        public void SetMCurve (DiscreteFunction M) {
            grapher.SetMCurve(M);
        }

        public void SetSCurve (DiscreteFunction S) {
            grapher.SetSCurve(S);
        }

        public void SetActive (bool active) {
            gameObject.SetActive (active);
        }

        public void OnProtanopiaClicked (bool enabled) {
            if (enabled) {
                Debug.Log ("Type is Protanopia.");
                cvdSim.BlindMode = ColorBlindnessSimulator.ColorBlindMode.Protanope;
            }
        }

        public void OnDeuteranopiaClicked (bool enabled) {
            if (enabled) {
                Debug.Log ("Type is Deuteranopia.");
                cvdSim.BlindMode = ColorBlindnessSimulator.ColorBlindMode.Deuteranope;
            }
        }

        public void OnSeverityChange (float severity) {
            Debug.Log ("Severity changed to " + severity.ToString());
            cvdSim.BlindIntensity = severity;
        }

        public void OnBrettelClicked (bool enabled) {
            if (enabled) {
                Debug.Log ("Algorithm is Brettel.");
                cvdSim.BlindAlgorithm = ColorBlindnessSimulator.ColorBlindAlgorithm.Brettel;
            }
        }

        public void OnMachadoClicked (bool enabled) {
            if (enabled) {
                Debug.Log ("Algorithm is Machado.");
                cvdSim.BlindAlgorithm = ColorBlindnessSimulator.ColorBlindAlgorithm.Machado;

            }
        }

    }

} /* namespace eyediseases */
