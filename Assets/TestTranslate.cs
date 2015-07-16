using UnityEngine;
using System.Collections;

public class TestTranslate : MonoBehaviour {

	float speed = 10f;
	
	// Update is called once per frame
	void Update () {
	
		transform.Translate(Vector3.forward * speed * Time.deltaTime);

	}
}
