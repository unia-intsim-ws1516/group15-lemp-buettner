#pragma strict

var showMobileButton = true;

function Start () {

if(showMobileButton == true){
	#if UNITY_IOS
		transform.guiTexture.enabled = true;
	#endif

	#if UNITY_ANDROID
		transform.guiTexture.enabled = true;
	#endif

	#if UNITY_WP8
		transform.guiTexture.enabled = true;
	#endif

	#if UNITY_BLACKBERRY
		transform.guiTexture.enabled = true;
	#endif
}

}
