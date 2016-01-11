using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour 
{
	public Canvas QuitMenu;
	public Button playbutton;
	public Button quitbutton;

	void Start ()

	{
		QuitMenu = QuitMenu.GetComponent<Canvas>();
		playbutton = playbutton.GetComponent<Button> ();
		quitbutton = quitbutton.GetComponent<Button> ();
		QuitMenu.enabled = false;

	}

	 public void ExitPress() //this function will be used on our Exit button

	 {
		QuitMenu.enabled = true; //enable the Quit menu when we click the Exit button
		playbutton.enabled = false; //then disable the Play and Exit buttons so they cannot be clicked
		quitbutton.enabled = false;

	}

	public void NoPress() //this function will be used for our "NO" button in our Quit Menu

	{
		QuitMenu.enabled = false; //we'll disable the quit menu, meaning it won't be visible anymore
		playbutton.enabled = true; //enable the Play and Exit buttons again so they can be clicked
		quitbutton.enabled = true;

	}

	public void StartLevel () //this function will be used on our Play button

	{
		SceneManager.LoadScene(1); //this will load our first level from our build settings. "1" is the second scene in our game

	}

	public void ExitGame () //This function will be used on our "Yes" button in our Quit menu

	{
		Application.Quit(); //this will quit our game. Note this will only work after building the game

	}
	public void goMenu ()
	{
		SceneManager.LoadScene(0); // laod Menu.
	}

}