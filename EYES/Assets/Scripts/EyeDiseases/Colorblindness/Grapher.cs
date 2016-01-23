using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using eyediseases;

public class Grapher : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private DiscreteFunction L = new DiscreteFunction ();
    private GameObject[] LPoints = null;
    private List<GameObject> draggedPoints = new List<GameObject> ();
    private const float dragRadius = 15.0f; // in px


    void Awake () {
        Debug.Log ("Grapher::Awake");
    }

	// Use this for initialization
	void Start () {
        Debug.Log ("Grapher::Start");

        // Load the responsivity functions
        string text = System.IO.File.ReadAllText("responsivityFunctions/ciexyz31.csv");
        string[] lines = text.Split("\n"[0]);

        L.values.Clear ();
        L.values.Capacity = lines.Length;

        for (int i = 0; i < lines.Length; ++i) {
            string[] dataText = lines[i].Split(","[0]);
            Debug.Assert (dataText.Length >= 4);

            if (0 == i) {
                double.TryParse (dataText[0], out L.minX);
            } else if ((lines.Length - 1) == i) {
                double.TryParse (dataText[0], out L.maxX);
            }

            double tmp = 0.0f;
            double.TryParse (dataText[1], out tmp);
            L.values.Add (tmp);
        }

        LPoints = new GameObject[L.values.Count];
        for (int i = 0; i < LPoints.Length; ++i) {
            GameObject dot = LPoints[i] = new GameObject ();

            dot.AddComponent<CanvasRenderer> ();
            RectTransform T = dot.AddComponent<RectTransform> ();
            T.SetParent (this.gameObject.GetComponent<RectTransform> (), false);
            T.anchorMin = T.anchorMax = new Vector2 ((float)i/ (float)LPoints.Length, (float)L.values[i]);
            T.pivot = new Vector2 (0.5f, 0.0f);
            T.offsetMin = new Vector2 (-1.0f, -1.0f);
            T.offsetMax = new Vector2 (1.0f, 1.0f);
            T.anchoredPosition = new Vector2 (0.0f, 0.0f);

            UnityEngine.UI.RawImage image = dot.AddComponent<UnityEngine.UI.RawImage> ();
            image.color = Color.red;

            dot.name = "Graphdot-L-" + i;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBeginDrag (PointerEventData eventData) {
        //Debug.Log ("OnBeginDrag: Click(" + eventData.pressPosition + "), dot(" + dot.transform.position + ")");

        draggedPoints.Clear ();

        foreach (GameObject dot in LPoints) {
            Vector2 dotPos = new Vector2 (dot.transform.position.x, dot.transform.position.y);
            if ((dotPos - eventData.pressPosition).magnitude < dragRadius) {
                draggedPoints.Add (dot);
            }
        }
    }

    public void OnDrag (PointerEventData eventData) {
        foreach (GameObject point in draggedPoints) {
            Debug.Log ("I'm being dragged!!");
            RectTransform TParent = gameObject.GetComponent<RectTransform> ();
            float width = TParent.rect.width;
            float height = TParent.rect.height;
            RectTransform T = point.GetComponent<RectTransform> ();
            Vector2 newPos = T.anchorMax + new Vector2 (0.0f, eventData.delta.y / height);
            newPos.y = Mathf.Max (newPos.y, 0.0f);
            newPos.y = Mathf.Min (newPos.y, 1.0f);
            T.anchorMin = T.anchorMax = newPos;
        }
    }

    public void OnPointerExit (PointerEventData eventData) {
        Debug.Log ("P in");
    }

    public void OnPointerEnter (PointerEventData eventData) {
        Debug.Log ("P out");
    }
}
