using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
	const float ANIMATION_DURATION = 1f;
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
		UISprite elementSprite = _elementGO.GetComponent<UISprite>();
		elementSprite.color = new Color (0f, 1f, 0f);
		if (_elementGO.GetComponent<TweenScale> () == null)
			_elementGO.AddComponent<TweenScale> ();
		TweenScale _tweenScale = _elementGO.GetComponent<TweenScale> ();
		_tweenScale.from.x = (elementSprite.transform.localScale.x*0.95f); _tweenScale.to.x = (elementSprite.transform.localScale.x*1.2f);
		_tweenScale.animationCurve = new AnimationCurve (
			new Keyframe (0f, 0f), new Keyframe (0.5f, 1.2f),  new Keyframe (0.75f, 0.8f), new Keyframe (1f, 1f)
		);
		_tweenScale.duration = ANIMATION_DURATION;
		if (_nextEvent != null)
			_tweenScale.onFinished.Add (_nextEvent);

	}
	public void correctBlastAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Blast Circular, Camera bobble
	}
	public void correctLocationAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Location (->Top->Bottom->Mid)X3, Colour(->Green)
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
		if (_nextEvent != null)
			_tweenPos.onFinished.Add (_nextEvent);
	}
	public void correctExpandAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Depth(+10), Size(->Screen Size)
	}
	public void correctTickOnTopAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Tick Mark on Top Size(0.5->1)
	}

	public void incorrectAnim(int _id, GameObject _elementGO, EventDelegate _nextEvent){
		switch (_id) {
		case 1:
			incorrectSizeAnim (_elementGO,_nextEvent);
			break;
		case 2:
			incorrectLocationAnim (_elementGO,_nextEvent);
			break;
		case 3:
			incorrectCrossAnim (_elementGO,_nextEvent);
			break;
		case 4:
			incorrectExpandAnim(_elementGO,_nextEvent);
			break;
		case 5:
			incorrectCrossOnTopAnim (_elementGO, _nextEvent);
			break;

		}
	}
	public void incorrectSizeAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Size (->Big), Colour(->Red)
		UISprite elementSprite = _elementGO.GetComponent<UISprite>();
		elementSprite.color = new Color (1f, 0f, 0f);
		if (_elementGO.GetComponent<TweenScale> () == null)
			_elementGO.AddComponent<TweenScale> ();
		TweenScale _tweenScale = _elementGO.GetComponent<TweenScale> ();
		 _tweenScale.to.x = (elementSprite.transform.localScale.x*1.2f);
		_tweenScale.animationCurve = new AnimationCurve (
			new Keyframe (0f, 0f), new Keyframe (0.5f, 1.2f),  new Keyframe (0.75f, 0.8f), new Keyframe (1f, 1f)
		);
		_tweenScale.duration = ANIMATION_DURATION;
		if (_nextEvent != null)
			_tweenScale.onFinished.Add (_nextEvent);
	}
	public void incorrectLocationAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Location (->Left->Right->Mid)X3 , Timer decreased by zooming into decrease sector of circle
	}
	public void incorrectCrossAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Duplicate Sprite of same size Size(-> 1.2X), Colour Red, Cross(Center on Top)
	}
	public void incorrectExpandAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Text cutthrough
	}
	public void incorrectCrossOnTopAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Cross Mark on Top Size(0.5->1)
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
	}
	public void correctionBackgroundAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//Text(->Correct) Colour(->Blue)
	}
	public void correctionTextOnTopAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//CorrectText shown on Top of incorrect text
	}
	public void correctionDragAnim(GameObject _elementGO, EventDelegate _nextEvent){
		//
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
