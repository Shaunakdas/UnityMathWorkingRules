using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
	const float ANIMATION_DURATION = 2f;
	const float TIMER_ANIM_DURATION = 10f;
	const float TIMER_WARNING_DELAY = 7f;
	public void correctAnim(int _id, GameObject _elementGO, EventDelegate _nextEvent){
		switch (_id) {
		case 1:
			correctSizeAnim (_elementGO,_nextEvent);
			break;
		case 2:
			correctBlastAnim (_elementGO,_nextEvent);
			break;
		case 3:
			correctLocationAnim (_elementGO,_nextEvent);
			break;
		case 4:
			correctExpandAnim (_elementGO,_nextEvent);
			break;
		case 5:
			correctTickOnTopAnim (_elementGO, _nextEvent);
			break;

		}
	}
	public void correctSizeAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Size (->Small->Big->Normal), Colour(->Green)
		Debug.Log("correctAnim 1: Size (->Small->Big->Normal), Colour(->Green) ");
		UISprite elementSprite = _elementGO.GetComponent<UISprite>();
		elementSprite.color = new Color (0f, 1f, 0f);
		if (_elementGO.GetComponent<TweenScale> () == null)
			_elementGO.AddComponent<TweenScale> ();
		TweenScale _tweenScale = _elementGO.GetComponent<TweenScale> ();
		_tweenScale.style = UITweener.Style.Once;
		_tweenScale.from.x = (elementSprite.transform.localScale.x*0.95f); _tweenScale.to.x = (elementSprite.transform.localScale.x*1.2f);
		_tweenScale.animationCurve = new AnimationCurve (
			new Keyframe (0f, 0f), new Keyframe (0.5f, 1.2f),  new Keyframe (0.75f, 0.8f), new Keyframe (1f, 1f)
		);
		_tweenScale.duration = ANIMATION_DURATION;
//		Debug.Log (_nextEvent.ToString()); 
		if (_nextEvent != null) {
			
			Debug.Log (_nextEvent.ToString()); 
			_tweenScale.onFinished.Add (_nextEvent);
		}

	}
	public void correctBlastAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Blast Circular, Camera bobble
		Debug.Log("correctAnim 2:  ");
	}
	public void correctLocationAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Location (->Top->Bottom->Mid)X3, Colour(->Green)
		Debug.Log("correctAnim 3: Location (->Top->Bottom->Mid)X3, Colour(->Green) ");
		UISprite elementSprite = _elementGO.GetComponent<UISprite>();
		elementSprite.color = new Color (0f, 1f, 0f);
		if (_elementGO.GetComponent<TweenPosition> () == null)
			_elementGO.AddComponent<TweenPosition> ();
		TweenPosition _tweenPos = _elementGO.GetComponent<TweenPosition> ();
		Vector3 currentPos = elementSprite.transform.localPosition;
		_tweenPos.from = new Vector3(currentPos.x,currentPos.y*1.05f,currentPos.z);
		_tweenPos.to = new Vector3(currentPos.x,currentPos.y*0.98f,currentPos.z);
		_tweenPos.animationCurve = new AnimationCurve (
			new Keyframe (0f, 0f), new Keyframe (0.285f, 2f),  new Keyframe (0.524f, 0.33f),new Keyframe (0.72f, 1.66f),new Keyframe (0.86f, 0.66f), new Keyframe (1f, 1f)
		);
		_tweenPos.duration = ANIMATION_DURATION;
		if (_nextEvent != null) {
			Debug.Log ("Tween Finished and calling next Event"+_nextEvent.ToString());
			_tweenPos.onFinished.Add (_nextEvent);
		}
	}
	public void correctExpandAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Depth(+10), Size(->Screen Size)
		Debug.Log("correctAnim 4: Depth(+10), Size(->Screen Size) ");
	}
	public void correctTickOnTopAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Tick Mark on Top Size(0.5->1)
		Debug.Log("correctAnim 5: Tick Mark on Top Size(0.5->1) ");
	}

	public void incorrectAnim(int _id, GameObject _elementGO, EventDelegate _nextEvent, bool delete){
		switch (_id) {
		case 1:
			incorrectSizeAnim (_elementGO,_nextEvent,delete);
			break;
		case 2:
			incorrectLocationAnim (_elementGO,_nextEvent,delete);
			break;
		case 3:
			incorrectCrossAnim (_elementGO,_nextEvent,delete);
			break;
		case 4:
			incorrectExpandAnim(_elementGO,_nextEvent,delete);
			break;
		case 5:
			incorrectCrossOnTopAnim (_elementGO, _nextEvent,delete);
			break;
		case 6:
			incorrectDestroyAnim (_elementGO, _nextEvent,delete);
			break;
		}
	}
	public void incorrectSizeAnim(GameObject _elementGO, EventDelegate _nextEvent, bool delete){
		//Size (->Big), Colour(->Red)
		Debug.Log("incorrectAnim 1: Size (->Big), Colour(->Red) ");
		UISprite elementSprite = _elementGO.GetComponent<UISprite>();
		elementSprite.color = new Color (1f, 0f, 0f);
		if (_elementGO.GetComponent<TweenScale> () == null)
			_elementGO.AddComponent<TweenScale> ();
		TweenScale _tweenScale = _elementGO.GetComponent<TweenScale> ();
		_tweenScale.to.x = (elementSprite.transform.localScale.x*1.4f);
		_tweenScale.style = UITweener.Style.Once;
		_tweenScale.animationCurve = new AnimationCurve (
			new Keyframe (0f, 0f), new Keyframe (0.5f, 1.2f),  new Keyframe (0.75f, 0.8f), new Keyframe (1f, 1f)
		);
		_tweenScale.duration = ANIMATION_DURATION;
		if((delete != null)&&delete){
			EventDelegate.Set(_tweenScale.onFinished, delegate{ NGUITools.Destroy(_elementGO); });
		}
		if (_nextEvent != null) {
			Debug.Log (" next Event"+_nextEvent.ToString());
			_tweenScale.onFinished.Add (_nextEvent);
		}
	}
	public void incorrectDestroyAnim(GameObject _elementGO, EventDelegate _nextEvent, bool delete){
		//Size (->Big), Colour(->Red)
		Debug.Log("incorrectAnim 6: Size (->Small) ");
//		UISprite elementSprite = _elementGO.GetComponent<UISprite>();
//		elementSprite.color = new Color (1f, 0f, 0f);
		if (_elementGO.GetComponent<TweenScale> () == null)
			_elementGO.AddComponent<TweenScale> ();
		TweenScale _tweenScale = _elementGO.GetComponent<TweenScale> ();
		_tweenScale.to.x = (0f);
		_tweenScale.animationCurve = new AnimationCurve (
			new Keyframe (0f, 0f), new Keyframe (1f, 1f)
		);
		_tweenScale.duration = ANIMATION_DURATION;
		if((delete != null)&&delete){
			EventDelegate.Set(_tweenScale.onFinished, delegate{ NGUITools.Destroy(_elementGO); });
		}
		if (_nextEvent != null) {
			_tweenScale.onFinished.Add (_nextEvent);
		}
	}
	public void incorrectLocationAnim(GameObject _elementGO, EventDelegate _nextEvent, bool delete){
		//Location (->Left->Right->Mid)X3 , Timer decreased by zooming into decrease sector of circle
		Debug.Log("incorrectAnim 2: Location (->Left->Right->Mid)X3 ");
		UISprite elementSprite = _elementGO.GetComponent<UISprite>();
//		elementSprite.color = new Color (0f, 1f, 0f);
		if (_elementGO.GetComponent<TweenPosition> () == null)
			_elementGO.AddComponent<TweenPosition> ();
		TweenPosition _tweenPos = _elementGO.GetComponent<TweenPosition> ();
		Vector3 currentPos = elementSprite.transform.localPosition;
		_tweenPos.from = new Vector3(currentPos.x*6f,currentPos.y,currentPos.z);
		_tweenPos.to = new Vector3(currentPos.x*1f,currentPos.y,currentPos.z);
		_tweenPos.animationCurve = new AnimationCurve (
			new Keyframe (0f, 0f), new Keyframe (0.285f, 2f),  new Keyframe (0.524f, 0.33f),new Keyframe (0.72f, 1.66f),new Keyframe (0.86f, 0.66f), new Keyframe (1f, 1f)
		);
		_tweenPos.duration = ANIMATION_DURATION;
		if((delete != null)&&delete){
			EventDelegate.Set(_tweenPos.onFinished, delegate{ NGUITools.Destroy(_elementGO.GetComponentInChildren<CustomDragDropItem>().gameObject); });
		}
		if (_nextEvent != null) {
			_tweenPos.onFinished.Add (_nextEvent);
		}
	}
	public void incorrectCrossAnim(GameObject _elementGO, EventDelegate _nextEvent, bool delete){
		//Duplicate Sprite of same size Size(-> 1.2X), Colour Red, Cross(Center on Top)
		Debug.Log("incorrectAnim 3: Duplicate Sprite of same size Size(-> 1.2X), Colour Red, Cross(Center on Top) ");
	}
	public void incorrectExpandAnim(GameObject _elementGO, EventDelegate _nextEvent, bool delete){
		//Text cutthrough
		Debug.Log("incorrectAnim 4: Text cutthrough ");
	}
	public void incorrectCrossOnTopAnim(GameObject _elementGO, EventDelegate _nextEvent, bool delete){
		//Cross Mark on Top Size(0.5->1)
		Debug.Log("incorrectAnim 5: Cross Mark on Top Size(0.5->1) ");
	}

	public void correctionAnim(int _id, GameObject _elementGO, EventDelegate _nextEvent){
		switch (_id) {
		case 1:
			correctionWaitAnim (_elementGO,_nextEvent);
			break;
		case 2:
			correctionBackgroundAnim (_elementGO,_nextEvent);
			break;
		case 3:
			correctionTextOnTopAnim (_elementGO,_nextEvent);
			break;
		case 4:
			correctionDragAnim(_elementGO,_nextEvent);
			break;

		}
	}
	public void correctionWaitAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Wait(2 sec)
		Debug.Log("correctionAnim 1: Wait(2 sec) ");
	}
	public void correctionBackgroundAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Text(->Correct) Colour(->Blue)
		Debug.Log("correctionAnim 2: Text(->Correct) Colour(->Blue) ");
	}
	public void correctionTextOnTopAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//CorrectText shown on Top of incorrect text
		Debug.Log("correctionAnim 1: CorrectText shown on Top of incorrect text ");
	}
	public void correctionDragAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//
	}

	public GameObject startTimerAnim(int _id, GameObject _elementGO, EventDelegate _nextEvent, bool warning){
		GameObject TimerAnimGO = null;
		switch (_id) {
		case 1:
//			TimerAnimGO = startBackgroundTimerAnim (_elementGO,_nextEvent, warning);
			break;
		case 2:
//			correctionBackgroundAnim (_elementGO,_nextEvent);
			break;
		case 3:
//			correctionTextOnTopAnim (_elementGO,_nextEvent);
			break;
		case 4:
//			correctionDragAnim(_elementGO,_nextEvent);
			break;

		}
		return TimerAnimGO;
	}
	public GameObject startBackgroundTimerAnim( GameObject _elementGO, EventDelegate _nextEvent,bool warning){
		GameObject BGAnimPF = Resources.Load (LocationManager.COMPLETE_LOC_ANIM_TYPE + LocationManager.NAME_BG_TIMER_ANIM)as GameObject;
		GameObject BGAnimGO = BasicGOOperation.InstantiateNGUIGO (BGAnimPF, _elementGO.transform);
		//Setting size

		BGAnimGO.GetComponent<UIWidget>().width = (int)NGUITools.screenSize.x;
		BGAnimGO.GetComponent<UIWidget> ().height = (int)BasicGOOperation.ElementSize (_elementGO).y;
		BGAnimGO.GetComponent<TweenFill> ().duration = TIMER_ANIM_DURATION;
		if (warning)
			startBackgroundWarningAnim (BGAnimGO);
		if (_nextEvent != null)
			BGAnimGO.GetComponent<TweenFill> ().onFinished.Add (_nextEvent);

		Debug.Log(BGAnimGO.transform.position.x/BasicGOOperation.scale.x);
		Debug.Log(BGAnimGO.transform.localPosition);
		Vector3 currPos =  BGAnimGO.transform.position; 
		BGAnimGO.transform.position = new Vector3 (0f,currPos.y,0f);
		return BGAnimGO;
	}
	public void startBackgroundWarningAnim( GameObject _backgroundGO){
		//starting warning Anim
		TweenAlpha bgAlpha= _backgroundGO.GetComponent<TweenAlpha> ();
		bgAlpha.duration = 1f;
		bgAlpha.delay = TIMER_WARNING_DELAY;
		bgAlpha.PlayForward ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
