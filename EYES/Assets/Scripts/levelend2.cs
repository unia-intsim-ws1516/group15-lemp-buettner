using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class levelend2 : MonoBehaviour {

	void OnCollisionEnter(Collision col)

	{
		if(col.gameObject.name == "Player")
			{
			SceneManager.LoadScene (0);

		}
	}
}
