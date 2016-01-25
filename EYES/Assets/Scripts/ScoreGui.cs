using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreGui : MonoBehaviour {

	public Text scoreText;

	void Start () {
        Score.score += (int)lefttime.timeRemaining;
        scoreText.text = "Score: " + Score.score;
	}
}

