using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Grapher : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject dot = null;
    private const float dragRadius = 15.0f; // in px
    private GameObject draggedPoints = null;

    void Awake () {
        Debug.Log ("Grapher::Awake");
    }

	// Use this for initialization
	void Start () {
        Debug.Log ("Grapher::Start");
        dot = new GameObject ();
        dot.AddComponent<CanvasRenderer> ();
        RectTransform T = dot.AddComponent<RectTransform> ();
        T.SetParent (this.gameObject.GetComponent<RectTransform> (), false);
        T.anchorMin = new Vector2 (0.0f, 0.0f);
        T.anchorMax = new Vector2 (0.0f, 0.0f);
        T.pivot = new Vector2 (0.0f, 0.0f);
        T.offsetMin = new Vector2 (-1.0f, -1.0f);
        T.offsetMax = new Vector2 (1.0f, 1.0f);
        T.anchoredPosition = new Vector2 (0.0f, 0.0f);

        UnityEngine.UI.RawImage image = dot.AddComponent<UnityEngine.UI.RawImage> ();
        image.color = Color.black;

        dot.name = "Graphdot";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBeginDrag (PointerEventData eventData) {
        Debug.Log ("OnBeginDrag: Click(" + eventData.pressPosition + "), dot(" + dot.transform.position + ")");

        draggedPoints = null;

        Vector2 dotPos = new Vector2 (dot.transform.position.x, dot.transform.position.y);
        if ((dotPos - eventData.pressPosition).magnitude < dragRadius) {
            draggedPoints = dot;
        }
    }

    public void OnDrag (PointerEventData eventData) {
        if (draggedPoints) {
            Debug.Log ("I'm being dragged!!");
            draggedPoints.transform.Translate(0.0f, eventData.delta.y, 0.0f);
        }
    }

    public void OnPointerExit (PointerEventData eventData) {
        Debug.Log ("P in");
    }

    public void OnPointerEnter (PointerEventData eventData) {
        Debug.Log ("P out");
    }
}
