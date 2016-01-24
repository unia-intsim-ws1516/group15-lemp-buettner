#pragma strict

function OnTriggerEnter (Coll : Collider)
{
	if(Coll.tag == "Player")

	{
		Application.LoadLevel(3);

	}
}