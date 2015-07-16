using UnityEngine;
using System.Collections;

public class CameraDamping : MonoBehaviour {

	public Transform target;

	public Vector3 offset;

	public float slerpSpeed;

	void Start()
	{
		Active ();
	}

	public void Active()
	{
		//offset = transform.position - target.position;
		StartCoroutine (FollowTarget ());
	}

	IEnumerator FollowTarget()
	{
		Vector3 xxx = target.position;
		while(true)
		{
			xxx = Vector3.Slerp (xxx, target.position, slerpSpeed);
			transform.position = xxx + offset;

			//transform.eulerAngles = target.rotation.eulerAngles;
			transform.rotation = Quaternion.RotateTowards(transform.rotation,target.rotation,1.5f);
			yield return null;
		}
	}

}
