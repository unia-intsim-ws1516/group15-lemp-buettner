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
        public Button ResetLBtn;
        public Button ResetMBtn;
        public Button ResetSBtn;
        public GameObject Graph;

        [HideInInspector] public ColorBlindnessSimulator cvdSim;
        [HideInInspector] public Grapher grapher;

        void Awake () {
            Debug.Log ("ColorBlindnessConfig::Awake");
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

        public void OnResetLClicked () {
            cvdSim.ResetL ();
        }

        public void OnResetMClicked () {
            cvdSim.ResetM ();
        }

        public void OnResetSClicked () {
            cvdSim.ResetS ();
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

                BrettelChk.interactable = true;
                MachadoChk.interactable = true;
                ProtanopiaChk.interactable = true;
                DeuteranopiaChk.interactable = true;
                TritanopiaChk.interactable = false;
                SeverityScroll.interactable = true;
                LChk.interactable = false;
                MChk.interactable = false;
                SChk.interactable = false;
                Graph.SetActive (false);
            }
        }

        public void OnMachadoClicked (bool enabled) {
            if (enabled) {
                Debug.Log ("Algorithm is Machado.");
                cvdSim.BlindAlgorithm = ColorBlindnessSimulator.ColorBlindAlgorithm.Machado;

                BrettelChk.interactable = true;
                MachadoChk.interactable = true;
                ProtanopiaChk.interactable = false;
                DeuteranopiaChk.interactable = false;
                TritanopiaChk.interactable = false;
                SeverityScroll.interactable = false;
                LChk.interactable = true;
                MChk.interactable = true;
                SChk.interactable = true;
                Graph.SetActive (true);
            }
        }

        public void OnEditLClicked (bool enabled) {
            grapher.EditL (enabled);
        }

        public void OnEditMClicked (bool enabled) {
            grapher.EditM (enabled);
        }

        public void OnEditSClicked (bool enabled) {
            grapher.EditS (enabled);
        }
    }
} /* namespace eyediseases */
