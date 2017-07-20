﻿using System;
using UnityEngine;
using System.Collections.Generic;
public class BasicGOOperation : MonoBehaviour{	
	//Base methods
	static private Vector3 _scale;
	static public Vector3 scale{
		get { 
			if (_scale != null) {
				return _scale;
			} else {
				return new Vector3 (0.002136752f, 0.002136752f, 0.002136752f);
			}
		}
		set { _scale = value; }
	}
	/// <summary>
	/// Get Child GameObject of given name
	/// </summary>
	/// <param name="parentGameObject">Parent GameObject</param>
	/// <param name="withName">Name of child GameObject</param>
	static public GameObject getChildGameObject(GameObject parentGameObject, string withName) {
		//Author: Isaac Dart, June-13.
		Transform[] ts = parentGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}
	/// <summary>
	/// Instantiate NGUI Child GameObject with a given prefab and parent transform
	/// </summary>
	/// <param name="parent">Parent Transform</param>
	/// <param name="prefab">Prefab for child GameObject</param>
	public static GameObject InstantiateNGUIGO(GameObject prefab, Transform parent) {
		return NGUITools.AddChild(parent.gameObject,prefab);
		//		return (GameObject)Instantiate(prefab,parent);
	}
	/// <summary>
	/// Instantiate NGUI Child GameObject with a given prefab, parent transform and a given name
	/// </summary>
	/// <param name="parent">Parent Transform</param>
	/// <param name="prefab">Prefab for child GameObject</param>
	public static GameObject InstantiateNGUIGO(GameObject prefab, Transform parent, string name) {
		GameObject child = InstantiateNGUIGO (prefab, parent);
		child.name = name;
		return child;
		//		return (GameObject)Instantiate(prefab,parent);
	}
	/// <summary>
	/// Instantiate Unity GUI Child GameObject with a given prefab and parent transform
	/// </summary>
	/// <param name="parent">Parent Transform</param>
	/// <param name="prefab">Prefab for child GameObject</param>
	static public GameObject InstantiateUnityGO(GameObject prefab, Transform parent) {
		return (GameObject)Instantiate(prefab,parent);
		//		return (GameObject)Instantiate(prefab,parent);
	}
	/// <summary>
	/// Delete all GameObjects in a given list and clear list also.
	/// </summary>
	/// <param name="GOList">List of GameObjects</param>
	public static void destroyGOList(List<GameObject> GOList){
		GOList.ForEach (imageObject => Destroy (imageObject));
		GOList.Clear ();
	}
	/// <summary>
	/// Delete all child GameObjects of a given parent GameObject.
	/// </summary>
	/// <param name="GO">Parent GameObject</param>
	public static void destroyChildGOList(GameObject GO){
		foreach (Transform child in GO.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
	/// <summary>
	/// Get size of text based on its length.
	/// </summary>
	/// <param name="text">text</param>
	public static float getTextSize(string text){
		TextGenerationSettings settings = new TextGenerationSettings();
		settings.textAnchor = TextAnchor.MiddleCenter;
		settings.color = Color.red;
		settings.generationExtents = new Vector2(500.0F, 200.0F);
		settings.pivot = Vector2.zero;
		settings.richText = true;
		settings.font = Resources.Load<Font>("Arial");
		settings.fontSize = 70;
		settings.fontStyle = FontStyle.Normal;
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		TextGenerator generator = new TextGenerator();
		generator.Populate(text, settings);
		float width = generator.GetPreferredWidth(text, settings);
		Debug.Log("Preferred width of " +text +" :"+ width);
		return width;
	}
	/// <summary>
	/// Get size of text based on its length of text.
	/// </summary>
	/// <param name="text">text</param>
	public static float getNGUITextSize(string text){
		float width =text.ToCharArray().Length*20f;
//		Debug.Log("Preferred width based on size of " +text +" :"+ width);
		return width;
	}
	/// <summary>
	/// Get random text generated from a list of all small cap letters.
	/// </summary>
	public static char GetRandomLetter()
	{
		string chars = "abcdefghijklmnopqrstuvwxyz";
		System.Random rand = new System.Random();

		int num = rand.Next(0, chars.Length -1);
		return chars[num];
	}
	/// <summary>
	/// Repositions all the table and grid in children GameObjects
	/// </summary>
	public static void RepositionChildTables(GameObject GO){
		foreach (UITable table in GO.GetComponentsInChildren<UITable>()){
//			Debug.Log ("Repositioning Table" + table.gameObject.name);
			table.Reposition ();
		}
		foreach (UIGrid grid in GO.GetComponentsInChildren<UIGrid>()){
//			Debug.Log ("Repositioning Table" + grid.gameObject.name);
			grid.Reposition ();
		}
	}
	/// <summary>
	/// Repositions all the table and grid in all parents GameObjects
	/// </summary>
	public static void RepositionParentTables(GameObject GO){
		foreach (UITable table in GO.GetComponentsInParent<UITable>()){
//			Debug.Log ("Repositioning Table" + table.gameObject.name);
			table.Reposition ();
		}
		foreach (UIGrid grid in GO.GetComponentsInParent<UIGrid>()){
//			Debug.Log ("Repositioning Table" + grid.gameObject.name);
			grid.Reposition ();
		}
	}
	/// <summary>
	/// Repositions table or grid 
	/// </summary>
	public static void CheckAndRepositionTable(GameObject GO){
		if (GO.GetComponent<UITable> () != null) {
			GO.GetComponent<UITable> ().Reposition ();
		}
		if (GO.GetComponent<UIGrid> () != null) {
			GO.GetComponent<UIGrid> ().Reposition ();
		}

	}
	/// <summary>
	/// Gets the width of the element game object.
	/// </summary>
	/// <param name="ElementGO">Element GameObject</param>
	public static Vector2 ElementSize(GameObject GO){
		ResizeToFitChildGO (GO);
		Vector3 finalSize = NGUIMath.CalculateAbsoluteWidgetBounds (GO.transform).size;
		finalSize.x = finalSize.x / scale.x; finalSize.y = finalSize.y / scale.y;
		Debug.Log ("ElementSize" + finalSize.x + " and " + finalSize.y);
		return finalSize;
	}

	/// <summary>
	/// Resizes to fit child GameObject list.
	/// </summary>
	/// <param name="ParentGameObject">Parent game object.</param>
	static public void ResizeToFitChildGO(GameObject ParentGameObject){
		RepositionChildTables (ParentGameObject);
//		Debug.Log ("ResizeToFitChildGO child count"+(NGUIMath.CalculateRelativeWidgetBounds(ParentGameObject.transform.GetChild(0).GetChild(0)).size.y));
		Vector3 finalSize = NGUIMath.CalculateRelativeWidgetBounds (ParentGameObject.transform).size;
//		finalSize.x = finalSize.x / scale.x; finalSize.y = finalSize.y / scale.y;
		if (ParentGameObject.GetComponent<UIWidget> () != null) {
			Debug.Log ("ResizeToFitChildGO name"+ ParentGameObject.name+ finalSize.x + " and " + finalSize.y);
			ParentGameObject.GetComponent<UIWidget> ().width = (int)finalSize.x;
			ParentGameObject.GetComponent<UIWidget> ().height = (int)finalSize.y;
		}
	}
	static public void ResizeToFitTargetGO(GameObject ParentGameObject, GameObject TargetGO){
		Vector3 finalSize = NGUIMath.CalculateAbsoluteWidgetBounds (TargetGO.transform).size;
		finalSize.x = finalSize.x / scale.x; finalSize.y = finalSize.y / scale.y;
		if (ParentGameObject.GetComponent<UIWidget> () != null) {
			Debug.Log ("ResizeToFitChildGO" + finalSize.x + " and " + finalSize.y);
			ParentGameObject.GetComponent<UIWidget> ().width = (int)finalSize.x;
			ParentGameObject.GetComponent<UIWidget> ().height = (int)finalSize.y;
		}
	}
	static public Vector3 ScaledBounds(GameObject TargetGO){
		Vector3 finalSize = NGUIMath.CalculateAbsoluteWidgetBounds (TargetGO.transform).size;
		finalSize.x = finalSize.x / scale.x; finalSize.y = finalSize.y / scale.y; finalSize.z = finalSize.z / scale.z;
		return finalSize;
	}
	static public Vector3 NGUIPosition(Transform transform){
		Vector3 finalSize = transform.position;
		finalSize.x = finalSize.x / scale.x; finalSize.y = finalSize.y / scale.y; finalSize.z = finalSize.z / scale.z;
		return finalSize;
	}
	static public Vector3 ScaledVector(Vector3 vector3){
		vector3.x = vector3.x / scale.x; vector3.y = vector3.y / scale.y; vector3.z = vector3.z / scale.z;
		return vector3;
	}
	static public void setText(GameObject TextGO, string text){
		if (TextGO.GetComponent<UILabel> () != null) {
			TextGO.GetComponent<UILabel> ().text = text;
		} else {
			TextGO.GetComponent<TEXDrawNGUI> ().text = text;
		}
	}
	static public void hideElementGO(GameObject ParentGO){
		if (ParentGO.GetComponent<UIWidget> () != null) {
			ParentGO.GetComponent<UIWidget> ().alpha = 0f;
		} else {
			foreach (Transform childTf in ParentGO.transform) {
				UIWidget childWidget = childTf.gameObject.GetComponent<UIWidget> ();
				if (childWidget != null){
					childWidget.alpha = 0f;
				}
			}
		}
	}
	static public void displayElementGO(GameObject ParentGO){
		if (ParentGO.GetComponent<UIWidget> () != null) {
			ParentGO.GetComponent<UIWidget> ().alpha = 1f;
		}else {
			foreach (Transform childTf in ParentGO.transform) {
				UIWidget childWidget = childTf.gameObject.GetComponent<UIWidget> ();
				if (childWidget != null){
					childWidget.alpha = 1f;
				}
			}
		}
	}
	static public void displayElementGOAnim(GameObject ParentGO){
		if (ParentGO.GetComponent<UIWidget> () != null) {
			alphaAnim (ParentGO.GetComponent<UIWidget>(), 0f, 1f);
		}else {
			foreach (Transform childTf in ParentGO.transform) {
				UIWidget childWidget = childTf.gameObject.GetComponent<UIWidget> ();
				if (childWidget != null){
					alphaAnim (childWidget, 0f, 1f);
				}
			}
		}
	}

	static public void displayElementGOAnim(GameObject ParentGO, EventDelegate nextAnim){
		if (ParentGO.GetComponent<UIWidget> () != null) {
			alphaAnim (ParentGO.GetComponent<UIWidget> (), 0f, 1f, nextAnim);
		} else {
			//Go through the list of child transforms of ParentGO and start animation if they have UIWidget Component
			for (int i = 0; i < ParentGO.transform.childCount; i++) {
				UIWidget childWidget = ParentGO.transform.GetChild (i).GetComponent<UIWidget>();
				if (childWidget != null){
					if (i != ParentGO.transform.childCount - 1) {
						alphaAnim (childWidget, 0f, 1f);
					} else {
						alphaAnim (childWidget, 0f, 1f, nextAnim);
					}
				}
			}
		}
	}
	static public void alphaAnim(UIWidget _elementWidget, float _fromAlpha, float _toAlpha){
		if (_elementWidget.gameObject.GetComponent<TweenAlpha> () == null) {
			_elementWidget.gameObject.AddComponent<TweenAlpha> ();
		}
		TweenAlpha elementTweenAlpha = _elementWidget.gameObject.GetComponent<TweenAlpha> ();
		if (elementTweenAlpha != null) {
			elementTweenAlpha.from = _fromAlpha; 
			elementTweenAlpha.to = _toAlpha; 
			elementTweenAlpha.duration = 2f;
			elementTweenAlpha.Play (true);
		}

	}
	static public void alphaAnim(UIWidget _elementWidget, float _fromAlpha, float _toAlpha, EventDelegate nextAnim){
		if (_elementWidget.gameObject.GetComponent<TweenAlpha> () == null) {
			_elementWidget.gameObject.AddComponent<TweenAlpha> ();
		}
		TweenAlpha elementTweenAlpha = _elementWidget.gameObject.GetComponent<TweenAlpha> ();
		if (elementTweenAlpha != null) {
			elementTweenAlpha.from = _fromAlpha; 
			elementTweenAlpha.to = _toAlpha; 
			elementTweenAlpha.duration = 2f;
			elementTweenAlpha.onFinished.Add (nextAnim);
			elementTweenAlpha.Play (true);

		}

	}
}


