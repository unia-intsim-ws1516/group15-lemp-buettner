using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DunnoWhatThisShouldDo : MonoBehaviour {

    public Toggle diseaseSwitch;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnShowConfig () {
        Debug.Log ("Show configuration");
    }

    public void OnToggle () {
        Debug.Log ("Use disease: " + diseaseSwitch.isOn);
    }
}
