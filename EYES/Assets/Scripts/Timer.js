#pragma strict

var timer : float = 600.0;

function Update() {

	timer -= Time.deltaTime;

}

function OnGUI() {
	GUI.Box(new Rect(10,50,50,20), "" + timer.ToString("0"));
	}
