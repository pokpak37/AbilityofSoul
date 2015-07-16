using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;

	MainManager mainManagerScript;

	void Start()
	{
		Active ();
	}

	public void Active()
	{
		mainManagerScript = GameObject.Find("MainManager").GetComponent<MainManager>();
	}

	void OnTriggerEnter(Collider other)
	{
		switch(other.tag)
		{
		case "PathTrigger" : 

			print("Hit " + other.name);

			PathTrigger pathTriggerScript = other.GetComponent<PathTrigger>();

			mainManagerScript.ChangePath(pathTriggerScript.targetPathName,
			                             pathTriggerScript.type,
			                             pathTriggerScript.fromOrToLaneNo);

			if(pathTriggerScript.activeBoss)
				mainManagerScript.ActiveBoss();
			//other.gameObject.SetActive(false);

			break;

		case "Item" : 
			print("Get Item " + other.name);
			break;
		}
	}

}
