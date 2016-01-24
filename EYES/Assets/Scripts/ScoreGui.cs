using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreGui : MonoBehaviour {

	public Text scoreText;


	void Start () {
		
		scoreText.text = "Score: " + menuScript.score;
	}


}

