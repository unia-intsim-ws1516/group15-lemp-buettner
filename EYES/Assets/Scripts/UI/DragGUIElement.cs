using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent (typeof(CanvasRenderer))]
public class DragGUIElement : MonoBehaviour, IDragHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnDrag (PointerEventData eventData) {
        gameObject.transform.Translate (eventData.delta.x, eventData.delta.y, 0.0f);
    }
}
