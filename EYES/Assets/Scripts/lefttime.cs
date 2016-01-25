using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class lefttime : MonoBehaviour {
	public static float timeRemaining = 400.0f;

	void Update () {
		timeRemaining -= Time.deltaTime;
	}

	void OnGUI(){
		if(timeRemaining > 0){
			GUI.Box(new Rect(5, 95, 100, 20), "" + "Time left:  " + (int)timeRemaining);
		}
		else{
			
			SceneManager.LoadScene(7);

		}
	}
}