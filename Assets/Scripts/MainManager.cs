using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour {

	public static MainManager instance;

	public GameObject Text3D;

	public RectTransform bossHpBar;

	#region Editable Variables
	float playerCurrentSpeed = 10f;

	float playerLandSpeed = 10f;
	float playerWaterSpeed = 6f;
	float playerAirSpeed = 0f;

	float playerStamina = 50f;
	int playerDmg = 3; 

	float bossStamina = 100;
	float maxBossStamina ;


	float jumpAndSlideDuration = 0.3f;
	float jumpAndSlideSpeed = 0.1f; // more = faster

	float changeLaneSpeed = 0.2f; // more = faster

	int theLeftestLane = 0;
	int theRightestLane = 2;

	#endregion

	#region  SaveVariables

	//LaneManager laneManagerScript;
	public GameObject laneManager;
	public Transform playerLane;
	public Transform player;

	int playerCurrentLane; // left = 0, mid = 1, right = 2

	bool onSplitLane;
	bool onChangeLane;
	bool onJump;
	bool onSlide;

	string currentPathName;
	float pathLength;
	float step;
	float pathPercent;

	public Transform[] lanePositions = new Transform[3];
	public Transform[] jumpLines = new Transform[2];

	BoxCollider playerCollider;

	#endregion

	#region SetUpAndControl

	void Start()
	{
		SetUpAndActive();
	}

	IEnumerator ButtonInput()
	{
		while(true)
		{
			if(Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
				MoveLeft();
			if(Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
				MoveRight();
			if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
				Jump();
			if(Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
				Slide();
			yield return null;
		}
	}

	public void Restart()
	{
		theLeftestLane = 0;
		theRightestLane = 2;
		onSplitLane = onChangeLane = onJump = onSlide = false;
		
		playerCurrentLane = 1;
		playerLane.position = lanePositions [playerCurrentLane].position;
		StartNewPath("Path1");
	}

	
	public void SetUpAndActive()
	{
		//laneManagerScript = LaneManager.instance;
		//playerPos = PlayerController.instance.transform.position;

		maxBossStamina = bossStamina;
		bossHpBar.localScale = Vector3.one;
		playerCurrentLane = 1;
		playerCollider = player.GetComponent<BoxCollider> ();

		StartNewPath("Path1");
		StartCoroutine (MoveAlongPath (laneManager));
		StartCoroutine (ButtonInput ());
		
		/*
		for(int i = 0;i < 3 ;i++)
		{
			lanePositions[1] = LaneManager.instance.lanePos[1];
		}
		*/
	}

	#endregion

	#region PathFunction

	public void ChangePath(string pathName,PathTrigger.triggerType type, int fromOrToPathId)
	{
		if (type == PathTrigger.triggerType.Split)
		{
			if(playerCurrentLane == fromOrToPathId)
			{
				//if(onChangeLane)
				//{
				//	StopCoroutine(OnChangeLane(0));
				//	onChangeLane = false;
				//}
				playerCurrentLane = 1;
				onSplitLane = true;
				theLeftestLane = 1;
				theRightestLane = 1;
				StartNewPath(pathName);
			}
			else
			{
				switch(fromOrToPathId)
				{
				case 0 : theLeftestLane = 1;
					break;
				case 2 : theRightestLane = 1;
					break;
				}
			}
		}
		else if(type == PathTrigger.triggerType.Join)
		{
			if (onSplitLane)
				playerCurrentLane = fromOrToPathId;			
			else
			{
				if (fromOrToPathId == 2)
					playerCurrentLane--;
				else
					playerCurrentLane++;
			}
			theLeftestLane = 0;
			theRightestLane = 2;
			StartNewPath(pathName);
		}
		else
		{
			StartNewPath(pathName);
		}
		playerLane.position = lanePositions [playerCurrentLane].position;
		ShowCurrentLane ();
	}

	IEnumerator MoveAlongPath(GameObject targetObject)
	{
		while (pathPercent<1) 
		{
			pathPercent += step/100f * Time.fixedDeltaTime ;
			iTween.PutOnPath(targetObject,iTweenPath.GetPath(currentPathName), pathPercent);
			targetObject.transform.LookAt(iTween.PointOnPath(iTweenPath.GetPath(currentPathName),pathPercent+.01f));
			yield return null;
		}
		//finish
		
		yield return null;
	}


	IEnumerator MoveAlongPath(GameObject targetObject, float offsetPathPercent)
	{
		while (pathPercent<1) 
		{
			pathPercent += step/100f * Time.fixedDeltaTime ;
			iTween.PutOnPath(targetObject,iTweenPath.GetPath(currentPathName), pathPercent + offsetPathPercent);
			targetObject.transform.LookAt(iTween.PointOnPath(iTweenPath.GetPath(currentPathName),pathPercent+.01f));
			yield return null;
		}
		//finish

		yield return null;
	}

	void StartNewPath(string pathName)
	{
		currentPathName = pathName;
		pathLength = iTween.PathLength (iTweenPath.GetPath (currentPathName));
		step =  playerCurrentSpeed/(pathLength/100f) ;
		pathPercent = 0f;
	}
	void StartNewPath(string pathName, float startAtPercent)
	{
		currentPathName = pathName;
		pathLength = iTween.PathLength (iTweenPath.GetPath (currentPathName));
		step =  playerCurrentSpeed/(pathLength/100f) ;
		pathPercent = startAtPercent;
	}

	#endregion

	#region Main Function
	
	public void MoveLeft()
	{
		if (isCanMove ('L', playerCurrentLane))
			StartCoroutine (OnChangeLane (playerCurrentLane - 1));

	}
	
	public void MoveRight()
	{
		if (isCanMove ('R', playerCurrentLane))
			StartCoroutine (OnChangeLane (playerCurrentLane + 1));
	}

	public void Jump()
	{
		if (isCanJump())
			StartCoroutine (OnJump ());
	}

	public void Slide()
	{
		if (isCanSlide())
			StartCoroutine (OnSlide ());
	}

	#endregion

	#region ChangeLaneSubFunction

	bool isCanMove(char moveTo,int fromLane)
	{
		if (onChangeLane)
			return false;
		else if(fromLane == theLeftestLane && moveTo == 'L')
			return false;
		else if(fromLane == theRightestLane && moveTo == 'R')
			return false;
		else 
			return true;
	}

	IEnumerator OnChangeLane(int toLane)
	{
		onChangeLane = true;

		ShowCurrentLane ("start changeLane");
		//playerLane.position = lanePositions [toLane].position;
		while(Vector3.Distance(playerLane.position,lanePositions[toLane].position)>0.1f)
		{
			playerLane.position = Vector3.Slerp (playerLane.position, lanePositions[toLane].position, changeLaneSpeed);
			yield return null;
		}
		playerLane.position = lanePositions [toLane].position;
		playerCurrentLane = toLane;
		onChangeLane = false;
		ShowCurrentLane ("finish changeLane");
		yield return null;
	}

	#endregion

	#region JumpSubFunction

	bool isCanJump()
	{
		if (onJump)
			return false;
		else 
			return true;
	}

	IEnumerator OnJump()
	{
		onJump = true;
		
		while(Mathf.Abs(player.position.y-jumpLines[0].position.y)>0.1f)
		{
			player.position = Vector3.Slerp (player.position, jumpLines [0].position, jumpAndSlideSpeed);
			yield return null;
		}
		
		player.position = jumpLines [0].position;
		
		yield return new WaitForSeconds (jumpAndSlideDuration);
		
		while(Mathf.Abs(player.position.y-jumpLines[1].position.y)>0.1f)
		{
			player.position = Vector3.Slerp (player.position, jumpLines [1].position, jumpAndSlideSpeed);
			yield return null;
		}
		
		player.position = jumpLines [1].position;

		onJump = false;
		print ("JumpFinish");
		yield return null;
	}

	#endregion

	#region SlideSubFunction
	
	bool isCanSlide()
	{
		if (onSlide)
			return false;
		else 
			return true;
	}
	
	IEnumerator OnSlide()
	{
		onSlide = true;
		
		Vector3 targetColliderCenter = Vector3.zero;
		while(playerCollider.center.y>0.05f)
		{
			playerCollider.center = Vector3.Slerp(playerCollider.center,targetColliderCenter,jumpAndSlideSpeed);
			yield return null;
		}
		playerCollider.center = Vector3.zero;
		
		yield return new WaitForSeconds (jumpAndSlideDuration);

		targetColliderCenter = new Vector3 (0, 0.5f, 0);
		while(playerCollider.center.y<0.45f)
		{
			playerCollider.center = Vector3.Slerp(playerCollider.center,targetColliderCenter,jumpAndSlideSpeed);
			yield return null;
		}
		playerCollider.center = targetColliderCenter;

		onSlide = false;
		yield return null;
	}
	
	#endregion

	#region Boss

	public void ActiveBoss()
	{
		//BossController bossScript = GameObject.Find ("Boss").GetComponent<BossController> ();
		//bossScript.Active();

		GameObject bossGameObject = GameObject.Find ("Boss");
		StartCoroutine (MoveAlongPath (bossGameObject,0.1f));
	}

	public void BossGetHit(Vector3 point)
	{
		bossStamina -= playerDmg;
		ShowDmgText(playerDmg, point);
		ChangeHPBar (bossStamina);
		if (bossStamina <= 0)
			BossDead ();
	}

	public void ShowDmgText(int dmg, Vector3 point)
	{
		GameObject textDmg = Instantiate (Text3D, point, Quaternion.identity) as GameObject;
		DmgTextPop textDmgScript = textDmg.GetComponent<DmgTextPop> ();
		textDmgScript.Active (3);
	}

	void BossDead()
	{
		GameObject bossGameObject = GameObject.Find ("Boss");
		bossGameObject.SetActive (false);
	}

	void ChangeHPBar(float bossHP)
	{

		float hpBarPercent = bossStamina / maxBossStamina;
		Mathf.Clamp (hpBarPercent, 0, 1);
		bossHpBar.localScale = new Vector3 (hpBarPercent, 1, 1);

	}

	#endregion

	#region helperFunction

	void ShowCurrentLane()
	{
		print ("Player in lane : " + playerCurrentLane);
	}

	void ShowCurrentLane(string detail)
	{
		print ("Player in lane : " + playerCurrentLane + " . . . " + detail);
	}

	#endregion

	#region OldFunction (not use)

	/*
		##jump##
		
		Vector3 targetPos = new Vector3 (0, jumpLines [0].position.y, 0);

		while(Mathf.Abs(player.position.y-jumpLines[0].position.y)>0.1f)
		{
			player.position = Vector3.Slerp (player.position, lanePositions[playerCurrentLane].position + targetPos, 0.3f);
			yield return null;
		}

		player.position = lanePositions[playerCurrentLane].position + targetPos;

		yield return new WaitForSeconds (jumpAndSlideDuration);

		while(Mathf.Abs(player.transform.position.y-jumpLines[1].position.y)>0.1f)
		{
			player.position = Vector3.Slerp (player.position, lanePositions[playerCurrentLane].position, 0.3f);
			yield return null;
		}

		player.position = lanePositions[playerCurrentLane].position;


		##path##

		iTween.PutOnPath (laneManager, iTweenPath.GetPath ("Path1"),0f);
		iTween.MoveTo (laneManager, iTween.Hash ("path", iTweenPath.GetPath ("Path1"),
		                                        "easetype", iTween.EaseType.linear,
		                                        "looptype", iTween.LoopType.loop,
		                                        "orienttopath", true,
		                                         "speed", speed,"movetopath", false));


	*/

	#endregion
}