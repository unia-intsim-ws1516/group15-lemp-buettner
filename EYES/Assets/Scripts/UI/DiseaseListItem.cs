using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DiseaseListItem : MonoBehaviour {

    public Toggle diseaseSwitch;
    public Text diseaseSwitchLabel;
    public eyediseases.EyeDisease disease;
    private bool isConfigDisplayed = false;

    public void OnShowConfig () {
        Debug.Log ("Show configuration");
        if (!isConfigDisplayed)
            disease.showConfig ();
        else
            disease.hideConfig ();
        isConfigDisplayed = !isConfigDisplayed;
    }

    public void OnToggle () {
        string onOff = " off";
        if (diseaseSwitch.isOn)
            onOff = " on";
        Debug.Log ("Turn " + diseaseSwitchLabel.text + onOff);

        disease.enabled = diseaseSwitch.isOn;
    }
}
