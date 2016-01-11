#pragma strict

var menuSceneName:String = "menu";
var menuFont:Font;

var darkenScreen:GUITexture;
var pausedText:GUIText;
var menuText:GUIText;
var resumeText:GUIText;
var resumeGraphics:GameObject;
var menuGraphics:GameObject;

private var isPaused = false;

function Start () {

if(menuFont != null){
	pausedText.font = menuFont;
	menuText.font = menuFont;
	resumeText.font = menuFont;		
}

}

function Update () {

var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

if(Input.GetKeyDown("escape")){
	doPause();
}

if(isPaused == true){
	if(Input.GetMouseButtonDown(0)){
		if(mousePos.y > 0.53 && mousePos.y < 0.8){
			doPause();
		}
		if(mousePos.y < 0.53 && mousePos.y > 0.2){
			Application.LoadLevel(menuSceneName);
		}
	}
}

#if UNITY_IOS
if(Input.GetMouseButtonDown(0)){
	if(mousePos.x > 0.8 && mousePos.y > 0.8){
		doPause();
	}
}
#endif

#if UNITY_ANDROID
if(Input.GetMouseButtonDown(0)){
	if(mousePos.x > 0.8 && mousePos.y > 0.8){
		doPause();
	}
}
#endif

#if UNITY_WP8
if(Input.GetMouseButtonDown(0)){
	if(mousePos.x > 0.8 && mousePos.y > 0.8){
		doPause();
	}
}
#endif

#if UNITY_BLACKBERRY
if(Input.GetMouseButtonDown(0)){
	if(mousePos.x > 0.8 && mousePos.y > 0.8){
		doPause();
	}
}
#endif

}

function doPause () {

if(isPaused == false){
	isPaused = true;
	Time.timeScale = 0;
	darkenScreen.GetComponent.<GUITexture>().enabled = true;
	pausedText.GetComponent.<GUIText>().enabled = true;
	resumeGraphics.SetActive(true);
	menuGraphics.SetActive(true);
}else{
	isPaused = false;
	Time.timeScale = 1;
	darkenScreen.GetComponent.<GUITexture>().enabled = false;
	pausedText.GetComponent.<GUIText>().enabled = false;
	resumeGraphics.SetActive(false);
	menuGraphics.SetActive(false);
}

}