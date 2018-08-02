using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GUIType
{
	NGUI,
	uGUIJelly,
	uGUIDragon,
	worldSpace
}
public class LocationFollower : MonoBehaviour {

	public float TARGET_DIFF = 0.1f;
	public float SPEED = 3.0f;
	//To track on which GUI current gameobject and target gameobject resides
	GUIType currentGUIType;
	GUIType targetGUIType;

	//When current gameobject wants to reach another gameobject
	bool isReachingGameobject;
	GameObject isReachingGameobjectTarget;
	EventDelegate hasReachedGameobject;
	bool hasReachedGameobjectTriggered;

	//When current gameobject wants to reach given location
	bool isReachingLocation;
	Vector3 isReachingLocationTarget;
	EventDelegate hasReachedLocation;

	//When current gameobject wants to follow another gameobject
	bool followAfterReachingGameobject;

	public void initiateFields(GUIType _gUIType, float _targetDiff, float _speed){
		currentGUIType = _gUIType;
		TARGET_DIFF = _targetDiff;
		SPEED = _speed;
	}

	public void initiateFields(GUIType _gUIType){
		currentGUIType = _gUIType;
	}

	//--------------------------------------------------- Reaching Methods ------------------------------------
	/// <summary>
	/// Sets up fields so that current gameobjct starts reaching target gameobject.
	/// </summary>
	public void startReachingGameobject(){
		isReachingGameobject = true;
	}

	/// <summary>
	/// Sets up target Gameobjects and their GUI level/type
	/// </summary>
	/// <param name="_targetGo">Target Gameobject</param>
	/// <param name="_targetGUIType">Target GUI type.</param>
	public void setReachingGameobject(GameObject _targetGo, GUIType _targetGUIType){
		isReachingGameobjectTarget = _targetGo;
		targetGUIType = _targetGUIType;
		hasReachedGameobjectTriggered = false;
	}

    public void setReachingGameobject(GameObject _targetGo, GUIType _targetGUIType, bool _followAfterReaching)
    {
        isReachingGameobjectTarget = _targetGo;
        targetGUIType = _targetGUIType;
        followAfterReachingGameobject = _followAfterReaching;
        hasReachedGameobjectTriggered = false;
    }

    public void setReachingGameobject(GameObject _targetGo, GUIType _targetGUIType, bool _followAfterReaching, EventDelegate _nextEvent)
    {
        isReachingGameobjectTarget = _targetGo;
        targetGUIType = _targetGUIType;
        followAfterReachingGameobject = _followAfterReaching;
        hasReachedGameobjectTriggered = false;
        hasReachedGameobject = _nextEvent;
    }
	/// <summary>
	/// Updates the field so that current gameobject stops reaching target gameobject
	/// </summary>
	public void stopReachingGameobject(){
		isReachingGameobject = false;
	}

	public void sethasReachedGameobjectTrigger(){
		hasReachedGameobjectTriggered = !hasReachedGameobjectTriggered;
	}


	//--------------------------------------------------- Lifecycle Methods ------------------------------------
	// Use this for initialization
	void Start () {
		hasReachedGameobjectTriggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isReachingGameobject) {
			Vector3 targetPos = calculateTargetPos (isReachingGameobjectTarget.transform.position);
			if (Vector3.Distance (transform.position, targetPos) > TARGET_DIFF) {
				transform.position = Vector3.MoveTowards (transform.position, targetPos, SPEED * Time.deltaTime);
			} else {
                if ((hasReachedGameobject != null) && (!hasReachedGameobjectTriggered))
                {
                    Debug.Log("LocationFollower REached2");
                    hasReachedGameobject.Execute();
                    hasReachedGameobjectTriggered = true;
                }
				if (!followAfterReachingGameobject) {
					isReachingGameobject = false;
				}
			}
		}

		if (isReachingLocation) {
			Vector3 targetPos = calculateTargetPos (isReachingLocationTarget);
			if (Vector3.Distance (transform.position, targetPos) > TARGET_DIFF) {
				transform.position = Vector3.MoveTowards (transform.position, targetPos, SPEED * Time.deltaTime);
			} else {
				isReachingLocation = false;
				if (hasReachedLocation != null) {
					hasReachedLocation.Execute ();
				}
			}
		}
	}
	//--------------------------------------------------- Common Libraries Methods ------------------------------------
	Vector3 calculateTargetPos(Vector3 _targetPosition){
		Vector3 targetPosition = _targetPosition;
		if (targetGUIType != currentGUIType) {
			if ((targetGUIType == GUIType.NGUI) && (currentGUIType == GUIType.uGUIDragon)) {
				return MathTrigger.Instance.convertNGUIToArrow(targetPosition);
			}
		}
		return targetPosition;
	}
}
