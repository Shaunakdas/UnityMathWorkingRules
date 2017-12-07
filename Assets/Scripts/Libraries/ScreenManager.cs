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
	//-------------Based on Children Types, resizing based on screen size -------------------
	//Checking GameObject type
	static bool isTexDraw(GameObject _elementGO){
		return (_elementGO.GetComponent<TEXDrawNGUI> () != null);
	}
	static bool isSprite(GameObject _elementGO){
		return (_elementGO.GetComponent<UI2DSprite> () != null);
	}
	static UIWidget resizeTexDraw(Vector2 scale, UIWidget widget){
		widget.width = (int)(scale.x * widget.width);
		widget.gameObject.GetComponent<TEXDrawNGUI> ().size = scale.y * widget.gameObject.GetComponent<TEXDrawNGUI> ().size;
		return widget;
	}
	static bool isUILabel(GameObject _elementGO){
		return (_elementGO.GetComponent<UILabel> () != null);
	}
	static UIWidget resizeUILabel(Vector2 scale, UIWidget widget){
		widget.gameObject.GetComponent<UILabel> ().fontSize = (int)(scale.y * widget.gameObject.GetComponent<UILabel> ().fontSize);
		return widget;
	}
	//Resizing Children based on its type
	static GameObject resizePanelChildren(Vector2 scale, GameObject _elementGO){
		foreach (UIPanel panel in  _elementGO.GetComponentsInChildren<UIPanel> ()){
			Vector4 region = panel.baseClipRegion;
			panel.baseClipRegion = new Vector4 (region.x, region.y, region.z * scale.x, region.w * scale.y);
		}
		return _elementGO;
	}
	static GameObject resizeGridChildren(Vector2 scale, GameObject _elementGO){
		foreach (UIGrid grid in  _elementGO.GetComponentsInChildren<UIGrid> ()){
			grid.cellWidth = scale.x * grid.cellWidth;
			grid.cellHeight = scale.y * grid.cellHeight;
		}
		return _elementGO;
	}
	static GameObject resizeWidgetChildren(Vector2 scale, GameObject _elementGO){
		Debug.Log ("GameObject resizeWidgetChildren"+ _elementGO.name);
		foreach (UIWidget widget in _elementGO.GetComponentsInChildren<UIWidget> ()){
			if (isTexDraw (widget.gameObject)) {
				resizeTexDraw (scale, widget);
			} else if (isUILabel (widget.gameObject)) {
				resizeUILabel (scale, widget);

				if ((widget.GetComponent<UILabel>().overflowMethod != UILabel.Overflow.ResizeFreely) && (!widget.isAnchored)){
					widget.height = (int)(scale.y *widget.height);
					widget.width = (int)(scale.x * widget.width);
				}
			} else if (widget.isAnchored) {
				//				Debug.Log ("isAnchored"+ widget.gameObject.name);
			} else {
				widget.height = (int)(scale.y *widget.height);
				widget.width = (int)(scale.x * widget.width);
			}
		}
		return _elementGO;
	}
	/// <summary>
	/// Resizes the children based on its type.
	/// </summary>
	/// <returns>GameObject itself</returns>
	/// <param name="_elementGO">Element Gameobject</param>
	static public GameObject resizeChildren(GameObject _elementGO){
		float scale_x = ((float)Screen.width / 480);
		float scale_y = ((float)Screen.height / 900);
		Vector2 newScale = new Vector2 (scale_x, scale_y);
		resizePanelChildren (newScale, _elementGO);
		resizeGridChildren (newScale, _elementGO);
		resizeWidgetChildren (newScale, _elementGO);
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
	static public GameObject SetAsScreenLeft(GameObject _elementGO){
		Vector2 elementSize = BasicGOOperation.ElementSize (_elementGO); Vector3 elementPos = _elementGO.transform.localPosition;
		_elementGO.transform.localPosition =new Vector3 ( -Screen.width/2, elementPos.y, elementPos.z);
		return _elementGO;
	}
	static public GameObject SetAsScreenRight(GameObject _elementGO){
		Vector2 elementSize = BasicGOOperation.ElementSize (_elementGO); Vector3 elementPos = _elementGO.transform.localPosition;
		_elementGO.transform.localPosition =new Vector3 ( Screen.width/2, elementPos.y, elementPos.z);
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
