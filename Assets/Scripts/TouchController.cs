using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {


	public float minMovement = 20.0f;
	public GameObject touchTarget = null;
	public MainManager mainManagerScript;
	
	private Vector2 StartPos;

	public LayerMask touchLayer;

	void Start()
	{
		Active ();
	}

	void Active()
	{
		//StartCoroutine (GetInput());
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0)||Input.GetMouseButton(0)||Input.GetMouseButtonUp(0))
		{
			
			if (Input.GetMouseButtonDown(0))
			{
				StartPos = new Vector2( Input.mousePosition.x,Input.mousePosition.y);
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				
				if (Physics.Raycast (ray, out hit, 100f, touchLayer)) 
				{
					touchTarget = hit.transform.gameObject;		
					if(touchTarget.tag == "Boss")
						mainManagerScript.BossGetHit(hit.point);
					else
						touchTarget.SendMessage ("GetHit",5, SendMessageOptions.DontRequireReceiver);
				}
			} 
			else if (Input.GetMouseButton(0)) 
			{
				Vector2 deltaPosition = StartPos - new Vector2( Input.mousePosition.x,Input.mousePosition.y);
				if (deltaPosition.magnitude > minMovement) {
					if (Mathf.Abs (deltaPosition.x) > Mathf.Abs (deltaPosition.y)) {
						if (deltaPosition.x > 0)
							mainManagerScript.MoveLeft ();
						else if (deltaPosition.x < 0)
							mainManagerScript.MoveRight ();
					} else {
						if (deltaPosition.y > 0)
							mainManagerScript.Slide ();
						else if (deltaPosition.y < 0)
							mainManagerScript.Jump ();
					}
				}
			}
			else if (Input.GetMouseButtonUp(0)) 
			{
				//touchTarget.SendMessage ("OnTap", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	IEnumerator GetInput ()
	{
		while (true) 
		{
			//if (touchTarget == null)
			//	touchTarget = gameObject;
#if UNITY_EDITOR 
			if(Input.GetMouseButtonDown(0)||Input.GetMouseButton(0)||Input.GetMouseButtonUp(0))
			{

				if (Input.GetMouseButtonDown(0))
				{
					StartPos = new Vector2( Input.mousePosition.x,Input.mousePosition.y);
					
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					
					if (Physics.Raycast (ray, out hit, 100f, touchLayer)) 
					{
						touchTarget = hit.transform.gameObject;		
						touchTarget.SendMessage ("GetHit",5, SendMessageOptions.DontRequireReceiver);
					}
				} 
				else if (Input.GetMouseButton(0)) 
				{
					Vector2 deltaPosition = StartPos - new Vector2( Input.mousePosition.x,Input.mousePosition.y);
					if (deltaPosition.magnitude > minMovement) {
						if (Mathf.Abs (deltaPosition.x) > Mathf.Abs (deltaPosition.y)) {
							if (deltaPosition.x > 0)
								mainManagerScript.MoveLeft ();
							else if (deltaPosition.x < 0)
								mainManagerScript.MoveRight ();
						} else {
							if (deltaPosition.y > 0)
								mainManagerScript.Slide ();
							else if (deltaPosition.y < 0)
								mainManagerScript.Jump ();
						}
					}
				}
				else if (Input.GetMouseButtonUp(0)) 
				{
						//touchTarget.SendMessage ("OnTap", SendMessageOptions.DontRequireReceiver);
				}
			}
#endif
#if UNITY_ANDROID

			foreach (Touch touch in Input.touches) 
			{
				if (touch.phase == TouchPhase.Began) 
				{
					StartPos = touch.position;

					Ray ray = Camera.main.ScreenPointToRay (touch.position);
					RaycastHit hit;
				
					if (Physics.Raycast (ray, out hit, 100f, touchLayer)) 
					{
						touchTarget = hit.transform.gameObject;		
						touchTarget.SendMessage ("GetHit",5, SendMessageOptions.DontRequireReceiver);
					}
				} 
				else if (touch.phase == TouchPhase.Moved) 
				{
					print ("KUYYYY");
					Vector2 deltaPosition = StartPos - touch.position;
					if (deltaPosition.magnitude > minMovement) 
					{
						if (Mathf.Abs (deltaPosition.x) > Mathf.Abs (deltaPosition.y)) 
						{
							if (deltaPosition.x > 0)
								mainManagerScript.MoveLeft ();
							else if (deltaPosition.x < 0)
								mainManagerScript.MoveRight ();
						} 
						else 
						{
							if (deltaPosition.y > 0)
								mainManagerScript.Slide ();
							else if (deltaPosition.y < 0)
								mainManagerScript.Jump ();
						}
					} 
					else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) 
					{

					}
				}
			}
#endif
			yield return null;
		}    
	}


}
