using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using eyediseases;

public class Grapher : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private DiscreteFunction L = null;
    private DiscreteFunction M = null;
    private DiscreteFunction S = null;

    private GameObject[] LPoints = null;
    private GameObject[] MPoints = null;
    private GameObject[] SPoints = null;

    private List<int> draggedPoints = new List<int> ();
    private const float dragRadius = 15.0f; // in px


    public void SetLCurve (DiscreteFunction L) {
        this.L = L;
        if (LPoints != null) {
            foreach (GameObject go in LPoints) {
                Destroy (go);
            }
        }
        if (L != null) {
            LPoints = new GameObject[L.values.Count];
            CreatePoints(L, LPoints, Color.red);
        }
    }

    public void SetMCurve (DiscreteFunction M) {
        this.M = M;
        if (MPoints != null) {
            foreach (GameObject go in MPoints) {
                Destroy (go);
            }
        }
        if (M != null) {
            MPoints = new GameObject[M.values.Count];
            CreatePoints(M, MPoints, Color.green);
        }
    }

    public void SetSCurve (DiscreteFunction S) {
        this.S = S;
        if (SPoints != null) {
            foreach (GameObject go in SPoints) {
                Destroy (go);
            }
        }
        if (S != null) {
            SPoints = new GameObject[S.values.Count];
            CreatePoints(S, SPoints, Color.blue);
        }
    }

    void Awake () {
        Debug.Log ("Grapher::Awake");
    }


    public void OnDisable () {
        if (LPoints != null) {
            foreach (GameObject go in LPoints) {
                Destroy (go);
            }
        }
        if (MPoints != null) {
            foreach (GameObject go in MPoints) {
                Destroy (go);
            }
        }
        if (SPoints != null) {
            foreach (GameObject go in SPoints) {
                Destroy (go);
            }
        }
    }

	// Use this for initialization
	void Start () {
        Debug.Log ("Grapher::Start");
    }

    private void CreatePoints (DiscreteFunction f, GameObject[] points, Color color) {
        Debug.Assert (f != null);
        Debug.Assert (points != null);
        Debug.Assert (f.values.Count == points.Length);

        for (int i = 0; i < points.Length; ++i) {
            GameObject dot = points[i] = new GameObject ();

            dot.AddComponent<CanvasRenderer> ();
            RectTransform T = dot.AddComponent<RectTransform> ();
            T.SetParent (this.gameObject.GetComponent<RectTransform> (), false);
            T.anchorMin = T.anchorMax = new Vector2 ((float)i/ (float)points.Length, (float)f.values[i]);
            T.pivot = new Vector2 (0.5f, 0.0f);
            T.offsetMin = new Vector2 (-1.0f, -1.0f);
            T.offsetMax = new Vector2 (1.0f, 1.0f);
            T.anchoredPosition = new Vector2 (0.0f, 0.0f);

            UnityEngine.UI.RawImage image = dot.AddComponent<UnityEngine.UI.RawImage> ();
            image.color = color;

            dot.name = "Graphdot-" + i;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBeginDrag (PointerEventData eventData) {
        //Debug.Log ("OnBeginDrag: Click(" + eventData.pressPosition + "), dot(" + dot.transform.position + ")");

        draggedPoints.Clear ();

        for (int i = 0; i < LPoints.Length; ++i) {
            GameObject dot = LPoints[i];
            Vector2 dotPos = new Vector2 (dot.transform.position.x, dot.transform.position.y);
            if ((dotPos - eventData.pressPosition).magnitude < dragRadius) {
                draggedPoints.Add (i);
            }
        }
    }

    public void OnDrag (PointerEventData eventData) {
        foreach (int idx in draggedPoints) {
            Debug.Log ("I'm being dragged!!");
            RectTransform TParent = gameObject.GetComponent<RectTransform> ();
            float height = TParent.rect.height;
            RectTransform T = LPoints[idx].GetComponent<RectTransform> ();
            float newF = T.anchorMax.y + (eventData.delta.y / height);
            newF = Mathf.Max (newF, 0.0f);
            newF = Mathf.Min (newF, 1.0f);
            L.values[idx] = newF;
            T.anchorMin = T.anchorMax = new Vector2 (T.anchorMax.x, newF);
        }
    }

    public void OnPointerExit (PointerEventData eventData) {
        Debug.Log ("P in");
    }

    public void OnPointerEnter (PointerEventData eventData) {
        Debug.Log ("P out");
    }
}
