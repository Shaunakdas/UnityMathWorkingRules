using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	static public GameObject SetAsScreenSize(GameObject _elementGO){
		SetAsScreenHeight (_elementGO);SetAsScreenWidth (_elementGO);
		return _elementGO;
	}
	static public GameObject SetAsScreenWidth(GameObject _elementGO){
		UIWidget elementWidget = _elementGO.GetComponent<UIWidget> ();
		if (elementWidget != null) {
			elementWidget.height = Screen.height;
		}
		return _elementGO;
	}
	static public GameObject SetAsScreenHeight(GameObject _elementGO){
		UIWidget elementWidget = _elementGO.GetComponent<UIWidget> ();
		if (elementWidget != null) {
			elementWidget.width = Screen.width;
		}
		return _elementGO;
	}
	static public GameObject SetAsScreenTop(GameObject _elementGO){
		Vector2 elementSize = BasicGOOperation.ElementSize (_elementGO); Vector3 elementPos = _elementGO.transform.localPosition;
		_elementGO.transform.localPosition =new Vector3 (elementPos.x, ((Screen.height/2) - (elementSize.y / 2)), elementPos.z);
		return _elementGO;
	}
	static public GameObject SetAsScreenBottom(GameObject _elementGO){
		Vector2 elementSize = BasicGOOperation.ElementSize (_elementGO); Vector3 elementPos = _elementGO.transform.localPosition;
		_elementGO.transform.localPosition =new Vector3 (elementPos.x, ((-Screen.height/2) + (elementSize.y / 2)), elementPos.z);
		return _elementGO;
	}
	static public GameObject SetTableAsScreenBottom(GameObject _elementGO){
		Vector2 elementSize = BasicGOOperation.ElementSize (_elementGO); Vector3 elementPos = _elementGO.transform.localPosition;
		_elementGO.transform.localPosition =new Vector3 (elementPos.x, (-Screen.height/2), elementPos.z);
		return _elementGO;
	}
	static public GameObject SetTableAsScreenTop(GameObject _elementGO){
		Vector2 elementSize = BasicGOOperation.ElementSize (_elementGO); Vector3 elementPos = _elementGO.transform.localPosition;
		_elementGO.transform.localPosition =new Vector3 (elementPos.x, Screen.height/2, elementPos.z);
		return _elementGO;
	}
}
