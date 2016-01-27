using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DiseaseListItem : MonoBehaviour {

    public Toggle diseaseSwitch;
    public Text diseaseSwitchLabel;
    public eyediseases.EyeDisease disease;

    public void OnShowConfig () {
        Debug.Log ("Show configuration");
        disease.showConfig (!disease.isConfigDisplayed ());
    }

    public void OnToggle () {
        string onOff = " off";
        if (diseaseSwitch.isOn)
            onOff = " on";
        Debug.Log ("Turn " + diseaseSwitchLabel.text + onOff);

        disease.enabled = diseaseSwitch.isOn;
    }
}
