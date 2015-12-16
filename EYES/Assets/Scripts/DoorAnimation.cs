using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour {

    private Animator[] anims;
    private GameObject player;
    private int count = 0;

    void Awake () {
        anims = GetComponentsInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter (Collider other) {
        Debug.Log(this.gameObject.name + "::OnTriggerEnter");
        if (other.gameObject == player) {
            ++count;
        }
    }

    void OnTriggerExit (Collider other) {
        Debug.Log(this.gameObject.name + "::OnTriggerExit");
        if (other.gameObject == player) {
            --count;
        }
    }

    void Update () {
        foreach (Animator anim in anims) {
            anim.SetBool("Open", count > 0);
        }
    }
}
