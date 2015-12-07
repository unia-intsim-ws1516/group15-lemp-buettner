using UnityEngine;
using System.Collections;

public class DiseaseOverviewList : MonoBehaviour
{

    public Camera TargetCamera;
    public Transform DiseaseListContainer;
    public Transform DiseaseListItemPrefab;

    // Use this for initialization
    void Start ()
    {
        if (TargetCamera == null) {
            Debug.LogError ("No target camera attached! Could not load any diseases...");
            return;
        }

        eyediseases.EyeDisease[] diseaseList = TargetCamera.GetComponents<eyediseases.EyeDisease> ();

        foreach (eyediseases.EyeDisease disease in diseaseList) {
            Debug.Log (disease.diseaseName);
            Transform item = Object.Instantiate (DiseaseListItemPrefab);
            item.SetParent (DiseaseListContainer, false);
            DiseaseListItem diseaseItem = item.GetComponent<DiseaseListItem> ();
            diseaseItem.diseaseSwitchLabel.text = disease.diseaseName;
            diseaseItem.disease = disease;
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
}
