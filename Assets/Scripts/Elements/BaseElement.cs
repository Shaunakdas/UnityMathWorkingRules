﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseElement : MonoBehaviour {

	//-------------Common Attributes -------------------
	public string prefabName{get; set;}
	//Parent BaseElement
	public BaseElement Parent{get; set;}


	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// The element game object.
	/// </summary>
	public GameObject ElementGO;
	/// <summary>
	/// Initiates the GameObject Default properties based on its corresponding class attribute
	/// </summary>
	/// <param name="ElementGO">Element GameObject</param>
	virtual public void initGOProp(GameObject ElementGO){
	}
	/// <summary>
	/// Generates the Element GameObject
	/// </summary>
	/// <returns>The element Gameobject</returns>
	/// <param name="ElementGameObject">Element GameObject</param>
	virtual public GameObject generateElementGO(GameObject ElementGameObject){
		return ElementGameObject;
	}
	/// <summary>
	/// Updates the GameObject properties based on its child values
	/// </summary>
	/// <param name="ElementGO">Element GameObject</param>
	virtual public void updateGOProp(GameObject ElementGO){
	}
}
