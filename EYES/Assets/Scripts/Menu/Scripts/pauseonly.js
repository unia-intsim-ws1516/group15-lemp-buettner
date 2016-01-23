#pragma strict

var pausedFont:Font;
var darkenScreen:GUITexture;
var pausedText:GUIText;

private var isPaused = false;

function Start () {

if(pausedFont != null){
	pausedText.font = pausedFont;
}

}

function Update () {

var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

if(Input.GetKeyDown("escape")){
	doPause();
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
}else{
	isPaused = false;
	Time.timeScale = 1;
	darkenScreen.GetComponent.<GUITexture>().enabled = false;
	pausedText.GetComponent.<GUIText>().enabled = false;
}

}