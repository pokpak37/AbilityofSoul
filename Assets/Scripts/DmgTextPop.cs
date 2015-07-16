using UnityEngine;
using System.Collections;

public class DmgTextPop : MonoBehaviour {

	float speed = 5f;
	float slerpSpeed = -5f;

	float lifeTime = 1f;

	TextMesh textMesh;

	public void Active (int dmg) 
	{	
		textMesh = this.GetComponent<TextMesh> ();
		textMesh.text = dmg.ToString ();
		StartCoroutine (Move ());
		StartCoroutine (FadeOut ());
		StartCoroutine (TimeCount ());
	}

	IEnumerator TimeCount()
	{
		while(lifeTime>0)
		{
			lifeTime-=Time.deltaTime;
			yield return null;
		}
		StopAllCoroutines ();
		Destroy (gameObject);
		yield return null;
	}

	IEnumerator Move()
	{
		while(lifeTime>0)
		{
			transform.Translate(Vector3.up*speed*Time.deltaTime);
			speed += slerpSpeed*Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

	IEnumerator FadeOut()
	{
		Color fadeColor = textMesh.color;
		float alpha = 1f;
		while(lifeTime>0)
		{
			alpha += Time.deltaTime * -1f/1.5f;
			fadeColor.a = alpha;
			textMesh.color = fadeColor;
			yield return null;
		}
		yield return null;
	}

}
