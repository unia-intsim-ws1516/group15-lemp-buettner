﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class levelend2 : MonoBehaviour {

	void OnCollisionEnter(Collision col)

	{
		if(col.gameObject.name == "Player")
			{
			
			menuScript.score = 500 + (int)lefttime.timeRemaining;
			SceneManager.LoadScene(3);


		}
	}
}
