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
	static bool isTexDraw(GameObject _elementGO){
		return (_elementGO.GetComponent<TEXDrawNGUI> () != null);
	}
	static bool notScrollView(GameObject _elementGO){
		return true;
	}
	static bool isChildOfGrid(GameObject _elementGO){
		return (_elementGO.GetComponentInParent<UIGrid>() != null);
	}
	static bool isButton(GameObject _elementGO){
		return (_elementGO.GetComponent<UIButton>() != null);
	}
	static public GameObject resizeChildren(GameObject _elementGO){
//			_elementGO.transform.localScale = new Vector3(scale_x,scale_y,1);
		//		_elementGO.transform.localScale.y = scale_y;
		float scale_x = ((float)Screen.width / 480);
		float scale_y = ((float)Screen.height / 900);
		foreach (UIGrid grid in  _elementGO.GetComponentsInChildren<UIGrid> ()){
			grid.cellWidth = scale_x * grid.cellWidth;
			grid.cellHeight = scale_y * grid.cellHeight;
		}
		foreach (UIPanel panel in  _elementGO.GetComponentsInChildren<UIPanel> ()){
			Vector4 region = panel.baseClipRegion;
			panel.baseClipRegion = new Vector4 (region.x, region.y, region.z * scale_x, region.w * scale_y);
//			Vector3 initial = panel.GetComponent<RectTransform> ().sizeDelta;
//			panel.GetComponent<RectTransform> ().sizeDelta = new Vector3(scale_x * initial.x, scale_y *initial.y, initial.z) ;
//			panel.width = scale_x * panel.width;
//			panel.height = scale_y * panel.height;
		}
		foreach (UIWidget widget in _elementGO.GetComponentsInChildren<UIWidget> ()){
//			Debug.Log (widget.gameObject.name);
			if (isTexDraw (widget.gameObject)) {
				widget.width = (int)(scale_x * widget.width);
				widget.gameObject.GetComponent<TEXDrawNGUI> ().size = scale_y * widget.gameObject.GetComponent<TEXDrawNGUI> ().size;
			} else if (isChildOfGrid (widget.gameObject)) {
				Debug.Log ("isChildOfGrid"+ widget.gameObject.name );
				Debug.Log ("isChildOfGrid"+ widget.gameObject.name + " child of "+widget.gameObject.GetComponentInParent<UIGrid>().gameObject.name);
			} else if (widget.isAnchored) {
				Debug.Log ("isAnchored"+ widget.gameObject.name);
			} else {
				widget.height = (int)(scale_y *widget.height);
				widget.width = (int)(scale_x * widget.width);
			}
//			widget.gameObject.transform.localScale = new Vector3(scale_x,scale_y,1);
//			widget.height = (Screen.height/900)*(widget.height);
//			widget.width = (Screen.width/400)*(widget.width);
		}
		return _elementGO;
	}
	static public GameObject SetAsScreenSize(GameObject _elementGO){
		SetAsScreenHeight (_elementGO,0);SetAsScreenWidth (_elementGO,0);
		return _elementGO;
	}
	static public GameObject SetAsScreenWidth(GameObject _elementGO, int padding){
		UIWidget elementWidget = _elementGO.GetComponent<UIWidget> ();
		if (elementWidget != null) {
			elementWidget.width = Screen.width - padding;
		}
		return _elementGO;
	}
	static public GameObject SetAsScreenHeight(GameObject _elementGO, int padding){
		UIWidget elementWidget = _elementGO.GetComponent<UIWidget> ();
		if (elementWidget != null) {
			elementWidget.height = Screen.height - padding;
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
