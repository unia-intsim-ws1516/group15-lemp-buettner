using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class endlevel : MonoBehaviour {

		void OnCollisionEnter(Collision col){

			if (col.gameObject.tag == "Player"){

				menuScript.score = 500 + (int)lefttime.timeRemaining;
				SceneManager.LoadScene(3);
			}
		}
	}
