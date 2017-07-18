//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2017 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;

/// <summary>
/// Spring-like motion -- the farther away the object is from the target, the stronger the pull.
/// </summary>

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class FastSpringPosition : MonoBehaviour
{
	static public FastSpringPosition current;

	/// <summary>
	/// Target position to tween to.
	/// </summary>

	public Vector3 target = Vector3.zero;

	/// <summary>
	/// Strength of the spring. The higher the value, the faster the movement.
	/// </summary>

	public float strength = 10f;

	/// <summary>
	/// Is the calculation done in world space or local space?
	/// </summary>

	public bool worldSpace = false;

	/// <summary>
	/// Whether the time scale will be ignored. Generally UI components should set it to 'true'.
	/// </summary>

	public bool ignoreTimeScale = false;

	/// <summary>
	/// Whether the parent scroll view will be updated as the object moves.
	/// </summary>

	public bool updateScrollView = false;

	public delegate void OnFinished ();

	/// <summary>
	/// Delegate to trigger when the spring finishes.
	/// </summary>

	public OnFinished onFinished;

	// Deprecated functionality
	[SerializeField][HideInInspector] GameObject eventReceiver = null;
	[SerializeField][HideInInspector] public string callWhenFinished;

	Transform mTrans;
	float mThreshold = 0f;
	UIScrollView mSv;

	/// <summary>
	/// Cache the transform.
	/// </summary>

	void Start ()
	{
		mTrans = transform;
		if (updateScrollView) mSv = NGUITools.FindInParents<UIScrollView>(gameObject);
	}

	void OnEnable () { mThreshold = 0f; }

	/// <summary>
	/// Advance toward the target position.
	/// </summary>

	void Update ()
	{
		float delta = ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
		if (worldSpace)
		{
			if (mThreshold == 0f) mThreshold = (target - mTrans.position).sqrMagnitude * 0.001f;
			mTrans.position = NGUIMath.SpringLerp(mTrans.position, target, strength, delta);

			if (mThreshold >= (target - mTrans.position).sqrMagnitude)
			{
				mTrans.position = target;
				NotifyListeners();
				enabled = false;
			}
		}
		else
		{
			if (mThreshold == 0f) mThreshold = (target - mTrans.localPosition).sqrMagnitude * 0.0001f;
			mTrans.localPosition = NGUIMath.SpringLerp(mTrans.localPosition, target, strength, delta);
			//Change
			Debug.Log(mThreshold);
			Debug.Log((target - mTrans.localPosition).sqrMagnitude);
			Debug.Log ((mThreshold >= (target - mTrans.localPosition).sqrMagnitude));
			if ((mThreshold >= (target - mTrans.localPosition).sqrMagnitude) || ((target - mTrans.localPosition).sqrMagnitude < 0.01f))
			{
				mTrans.localPosition = target;
				NotifyListeners();
				enabled = false;
			}
		}

		// Ensure that the scroll bars remain in sync
		if (mSv != null) mSv.UpdateScrollbars(true);
	}

	/// <summary>
	/// Notify all finished event listeners.
	/// </summary>

	void NotifyListeners ()
	{
		current = this;

		if (onFinished != null) onFinished();

		if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
			eventReceiver.SendMessage(callWhenFinished, this, SendMessageOptions.DontRequireReceiver);

		current = null;
	}

	/// <summary>
	/// Start the tweening process.
	/// </summary>

	static public FastSpringPosition Begin (GameObject go, Vector3 pos, float strength)
	{
		FastSpringPosition sp = go.GetComponent<FastSpringPosition>();
		if (sp == null) sp = go.AddComponent<FastSpringPosition>();
		sp.target = pos;
		sp.strength = strength;
		sp.onFinished = null;
		if (!sp.enabled) sp.enabled = true;
		return sp;
	}
}
