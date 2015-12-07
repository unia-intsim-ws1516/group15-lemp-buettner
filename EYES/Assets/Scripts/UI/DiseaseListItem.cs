using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DiseaseListItem : MonoBehaviour {

    public Toggle diseaseSwitch;
    public Text diseaseSwitchLabel;

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
        string onOff = " off";
        if (diseaseSwitch.isOn)
            onOff = " on";
        Debug.Log ("Turn " + diseaseSwitchLabel.text + onOff);
    }
}
