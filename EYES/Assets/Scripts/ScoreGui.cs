using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreGui : MonoBehaviour {

	public Text scoreText;


	void Start () {
		menuScript.score += 0 + (int)lefttime.timeRemaining;
		scoreText.text = "Score: " + menuScript.score;
	}


}

