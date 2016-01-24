using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class Points : MonoBehaviour 

{
	public GUIText scoreText;
	private int score;
	public Canvas EndMenu;

	// Use this for initialization
	void Start () {

		EndMenu = EndMenu.GetComponent<Canvas>();
		EndMenu.enabled = false;
		score = 100;
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}
}
