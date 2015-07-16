using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {
	/*
	float speed = 10f;

	string currentPathName;
	float pathLength;
	float step;
	float pathPercent;

	public void Active()
	{
		StartNewPath ("Path6",0.1f);
		StartCoroutine (MoveAlongPath ());
	}

	IEnumerator MoveAlongPath()
	{
		while (pathPercent<1) 
		{
			pathPercent += step/100f * Time.fixedDeltaTime ;
			iTween.PutOnPath(this.gameObject,iTweenPath.GetPath(currentPathName), pathPercent);
			this.transform.LookAt(iTween.PointOnPath(iTweenPath.GetPath(currentPathName),pathPercent+.01f));
			yield return null;
		}
		//finish
		
		yield return null;
	}

	void StartNewPath(string pathName)
	{
		currentPathName = pathName;
		pathLength = iTween.PathLength (iTweenPath.GetPath (currentPathName));
		step =  speed/(pathLength/100f) ;
		pathPercent = 0f;
	}

	void StartNewPath(string pathName, float startAtPercent)
	{
		currentPathName = pathName;
		pathLength = iTween.PathLength (iTweenPath.GetPath (currentPathName));
		step =  speed/(pathLength/100f) ;
		pathPercent = startAtPercent;
	}

	public void GetHit(int dmg)
	{
		print ("Fuckkkkk " + dmg);
	}
	*/
}
