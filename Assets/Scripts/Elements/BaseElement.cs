using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
public class BaseElement : MonoBehaviour {
	public string prefabName{get; set;}
	//Parent BaseElement
	public BaseElement Parent{get; set;}
	/// <summary>
	/// Initiates the GameObject properties based on its corresponding class attribute
	/// </summary>
	/// <param name="ElementGO">Element GameObject</param>
	virtual public void initGOProp(GameObject ElementGO){
	}
	/// <summary>
	/// Updates the GameObject properties based on its corresponding class attribute
	/// </summary>
	/// <param name="ElementGO">Element GameObject</param>
	virtual public void updateGOProp(GameObject ElementGO){
	}
	/// <summary>
	/// Generates the corresponding GameObject
	/// </summary>
	/// <returns>The element Gameobject</returns>
	/// <param name="ElementGameObject">Element GameObject</param>
	virtual public GameObject generateElementGO(GameObject ElementGameObject){
		return ElementGameObject;
	}
	/// <summary>
	/// The element game object.
	/// </summary>
	public GameObject ElementGO;
	/// <summary>
	/// Parses the children nodes to create child elements
	/// </summary>
	/// <param name="elementNode">Element HTML node.</param>
	virtual public void parseChildNode(HtmlNode elementNode){
	}
}
