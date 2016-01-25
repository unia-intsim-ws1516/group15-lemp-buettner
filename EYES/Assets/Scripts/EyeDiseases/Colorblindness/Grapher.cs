using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using eyediseases;

public class Grapher : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
{
    private DiscreteFunction L = null;
    private DiscreteFunction M = null;
    private DiscreteFunction S = null;

    private GameObject[] LPoints = null;
    private GameObject[] MPoints = null;
    private GameObject[] SPoints = null;

    /* Null equas not draggable */
    private List<int> draggedPointsL = new List<int> ();
    /* Null equas not draggable */
    private List<int> draggedPointsM = new List<int> ();
    /* Null equas not draggable */
    private List<int> draggedPointsS = new List<int> ();

    private float dragRadius = 15.0f; // in px
    public GameObject dragRadiusGizmo;


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

    public void OnDestroy () {
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

    public void EditL (bool edit) {
        if (edit) {
            draggedPointsL = new List<int> ();
        } else {
            draggedPointsL = null;
        }
    }

    public void EditM (bool edit) {
        if (edit) {
            draggedPointsM = new List<int> ();
        } else {
            draggedPointsM = null;
        }
    }

    public void EditS (bool edit) {
        if (edit) {
            draggedPointsS = new List<int> ();
        } else {
            draggedPointsS = null;
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

    public void OnBeginDrag (PointerEventData eventData) {
        //Debug.Log ("OnBeginDrag: Click(" + eventData.pressPosition + "), dot(" + dot.transform.position + ")");

        if (draggedPointsL != null) {
            
            draggedPointsL.Clear ();
            for (int i = 0; i < LPoints.Length; ++i) {
                GameObject dot = LPoints[i];
                Vector2 dotPos = new Vector2 (dot.transform.position.x, dot.transform.position.y);
                if ((dotPos - eventData.pressPosition).magnitude < dragRadius) {
                    draggedPointsL.Add (i);
                }
            }
        }


        if (draggedPointsM != null) {
            draggedPointsM.Clear ();
            for (int i = 0; i < MPoints.Length; ++i) {
                GameObject dot = MPoints[i];
                Vector2 dotPos = new Vector2 (dot.transform.position.x, dot.transform.position.y);
                if ((dotPos - eventData.pressPosition).magnitude < dragRadius) {
                    draggedPointsM.Add (i);
                }
            }
        }


        if (draggedPointsS != null) {
            draggedPointsS.Clear ();
            for (int i = 0; i < SPoints.Length; ++i) {
                GameObject dot = SPoints[i];
                Vector2 dotPos = new Vector2 (dot.transform.position.x, dot.transform.position.y);
                if ((dotPos - eventData.pressPosition).magnitude < dragRadius) {
                    draggedPointsS.Add (i);
                }
            }
        }
    }

    public void OnDrag (PointerEventData eventData) {
        if (draggedPointsL != null) {
            DragPoints (LPoints, draggedPointsL, L, eventData.delta);
        }
        if (draggedPointsM != null) {
            DragPoints (MPoints, draggedPointsM, M, eventData.delta);
        }
        if (draggedPointsS != null) {
            DragPoints (SPoints, draggedPointsS, S, eventData.delta);
        }
    }

    /**
     * Updates the points in points specified by indices in y-direction and
     * updates the appropriate function value of f as well.
     */
    void DragPoints (GameObject[] points, List<int> indices, DiscreteFunction f, Vector2 delta) {
        foreach (int idx in indices) {
            RectTransform TParent = gameObject.GetComponent<RectTransform> ();
            float height = TParent.rect.height;
            RectTransform T = points[idx].GetComponent<RectTransform> ();
            float newF = T.anchorMax.y + (delta.y / height);
            newF = Mathf.Max (newF, 0.0f);
            newF = Mathf.Min (newF, 1.0f);
            f.values[idx] = newF;
            T.anchorMin = T.anchorMax = new Vector2 (T.anchorMax.x, newF);
        }
    }

    public void OnPointerExit (PointerEventData eventData) {
        Debug.Log ("P out");
        dragRadiusGizmo.SetActive (false);
    }

    public void OnPointerEnter (PointerEventData eventData) {
        Debug.Log ("P in");
        dragRadiusGizmo.SetActive (true);

        // Set the size of the gizmo
        RectTransform tf = dragRadiusGizmo.GetComponent<RectTransform> ();
        tf.offsetMin = -Vector2.one * dragRadius;
        tf.offsetMax =  Vector2.one * dragRadius;
    }

    // Update is called once per frame
    void Update () {

        Debug.Log ("MousePos: " + UnityEngine.Input.mousePosition);
        dragRadiusGizmo.transform.localPosition = UnityEngine.Input.mousePosition - transform.position;
    }

    public void OnScroll (PointerEventData eventData) {
        Debug.Log ("Scroll " + eventData.scrollDelta);
        RectTransform tf = dragRadiusGizmo.GetComponent<RectTransform> ();
        dragRadius -= eventData.scrollDelta.y * 3.0f;
        tf.offsetMin = -Vector2.one * dragRadius;
        tf.offsetMax =  Vector2.one * dragRadius;
    }

//    IEnumerator TrackPointer () {
//        GraphicRaycaster ray = GetComponentInParent<GraphicRaycaster> ();
//        StandaloneInputModule Input = FindObjectOfType<StandaloneInputModule> ();
//
//        if (ray != null && Input != null) {
//            while ( Application.isPlaying ) {
//                Vector2 localPos;
//                RectTransformUtility.ScreenPointToLocalPointInRectangle (transform as RectTransform, UnityEngine.Input.mousePosition, ray.eventCamera, out localPos);
//
//                dragRadiusGizmo.transform.localPosition = localPos;
//                yield return 0;
//            }
//        }
//        else
//            Debug.LogWarning ("Could not find GraphicRaycaster and/or StandaloneInputModule");
//    }
}
