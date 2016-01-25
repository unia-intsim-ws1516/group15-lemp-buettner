using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DiseaseOverviewList : MonoBehaviour
{

    public Camera TargetCamera;
    [HideInInspector] public Transform DiseaseListContainer;
    [HideInInspector] public Transform DiseaseListItemPrefab;

    private eyediseases.EyeDisease[] diseaseList;
    private IList<DiseaseListItem> diseaseListItems;

    // Use this for initialization
    void Start ()
    {
        if (TargetCamera == null) {
            Debug.LogError ("No target camera attached! Could not load any diseases...");
            return;
        }

        if (diseaseListItems == null) {
            diseaseListItems = new List<DiseaseListItem>();
        } else {
            diseaseListItems.Clear ();
        }

        diseaseList = TargetCamera.GetComponents<eyediseases.EyeDisease> ();
        foreach (eyediseases.EyeDisease disease in diseaseList) {

            Transform item = Instantiate (DiseaseListItemPrefab);
            item.SetParent (DiseaseListContainer, false);

            DiseaseListItem diseaseItem = item.GetComponent<DiseaseListItem> ();
            diseaseItem.diseaseSwitchLabel.text = disease.diseaseName;
            diseaseItem.disease = disease;
            Toggle tg = diseaseItem.GetComponentInChildren<Toggle> ();
            tg.isOn = disease.isActiveAndEnabled;
            diseaseListItems.Add(diseaseItem);
        }
    }
}
