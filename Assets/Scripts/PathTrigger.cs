using UnityEngine;
using System.Collections;

public class PathTrigger : MonoBehaviour {

	public enum triggerType
	{
		Split,
		Join,
		None
	};


	public string targetPathName;

	public triggerType type;

	public int fromOrToLaneNo;

	public bool activeBoss;
}
