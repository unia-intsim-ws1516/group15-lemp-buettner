using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour 
{
	public GameObject QuitMenu;
    public GameObject LevelMenu;
	public Button QuitButton;
	public Button LevelButton;
    public GameObject DiseaseMenu;
	public Button InfoButton;


	void Start ()  // disables different supmenus and activates the buttons

	{
		//QuitMenu = QuitMenu.GetComponent<Canvas>();
		//QuitMenu.enabled = false;
        QuitMenu.SetActive (false);
		//Levelmenu = Levelmenu.GetComponent<Canvas>();
		//Levelmenu.enabled = false;
        LevelMenu.SetActive (false);
		//Diseasemenu = Diseasemenu.GetComponent<Canvas>();
		//Diseasemenu.enabled = false;
        DiseaseMenu.SetActive (false);
		QuitButton = QuitButton.GetComponent<Button> ();
		LevelButton = LevelButton.GetComponent<Button> ();
		InfoButton = InfoButton.GetComponent<Button> ();





	}

	 public void ExitPress() //this function will be used on our Exit button

	 {
		//QuitMenu.enabled = true; //enable the Quit menu when we click the Exit button
        QuitMenu.SetActive (true);
		QuitButton.enabled = false;
		LevelButton.enabled = false;
		//Levelmenu.enabled = false;
        LevelMenu.SetActive (false);
		InfoButton.enabled = false;
		//DiseaseMenu.enabled = false;
        DiseaseMenu.SetActive (false);
	}

	public void NoPress() //this function will be used for our "NO" button in our Quit Menu

	{
		//QuitMenu.enabled = false;	//we'll disable the quit menu, meaning it won't be visible anymore
        QuitMenu.SetActive (false);
		//Levelmenu.enabled = false;
        LevelMenu.SetActive (false);
		QuitButton.enabled = true;
		LevelButton.enabled = true;
		InfoButton.enabled = true;
		//DiseaseMenu.enabled = false;
        DiseaseMenu.SetActive (false);

	


	}

	public void StartLevel () //this function will be used on our Play button

	{
		SceneManager.LoadScene(1); //this will load our first level from our build settings. "1" is the second scene in our game

	}

	public void ExitGame () //This function will be used on our "Yes" button in our Quit menu

	{
		Application.Quit(); //this will quit our game. Note this will only work after building the game

	}
	public void goMenu ()  //this will be used for the jump to the menu
	{
		SceneManager.LoadScene(0); // load Menu.
	}

	public void Levelpress ()  // this will be used for the Level menu 
		
	{
		//Levelmenu.enabled = true; //
        LevelMenu.SetActive (true);
		QuitButton.enabled = false;
		LevelButton.enabled = false;
		InfoButton.enabled = false;
		//DiseaseMenu.enabled = false;
        DiseaseMenu.SetActive (false);
	}

	public void Infopress () // this will be used for the info menu
	{
		//DiseaseMenu.enabled = true;
        DiseaseMenu.SetActive (true);
		//Levelmenu.enabled = false; //
        LevelMenu.SetActive (false);
		QuitButton.enabled = false;
		LevelButton.enabled = false;
		InfoButton.enabled = false;

	}


	public void levelColorblindness () // this will be used for loading the level 1 ( Colorblindness)
	{
		SceneManager.LoadScene(1); // load Colorblindness
	}

	public void levelGlaucoma () // this will be used for loading the level 2 ( Glacuoma)
	{
		SceneManager.LoadScene(2); // load Glaucoma
	}

	public void levelCataract () // this will be used for loading the level 3 (Cataract)
	{
		SceneManager.LoadScene(3); // load Cataract
	}

	public void levelMyopia ()  // this will be used for loading the level 4 (Myopia)
	{
		SceneManager.LoadScene(4); // load Myopia
	}

	public void levelHyperopia ()  // this will be used for loading the level 5 (Hyperopia)
	{
		SceneManager.LoadScene(5); // load Hyperopia
	}

	public void levelexploration ()  // this will be used for loading the Explorationlevel
	{
		SceneManager.LoadScene(6); // load Exploration level
	}

	public void infocolorblindness ()
	{
		Application.OpenURL ("https://nei.nih.gov/health/color_blindness/facts_about");
	}

	public void infoglaucoma ()
	{
		Application.OpenURL ("https://nei.nih.gov/health/glaucoma/glaucoma_facts");
	}


	public void infocataract ()
	{
		Application.OpenURL ("https://nei.nih.gov/health/cataract/cataract_facts");
	}

	public void infomyopia ()
	{
		Application.OpenURL ("https://nei.nih.gov/health/errors/myopia");
	}

	public void infohyperopia ()
	{
		Application.OpenURL ("https://nei.nih.gov/health/errors/hyperopia");
	}

}