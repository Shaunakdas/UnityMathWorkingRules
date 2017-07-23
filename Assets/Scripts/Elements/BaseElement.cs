﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
public class BaseElement : MonoBehaviour {
	//-------------Common Attributes -------------------
	public Paragraph ParagraphRef;
	public string prefabName{get; set;}
	//Parent BaseElement
	public BaseElement Parent{get; set;}
	/// <summary>
	/// Type of interaction in current element
	/// </summary>
	public enum Interaction{Drag,Drop,Select,Mixed,Display,Default};
	public Interaction InteractionType = Interaction.Display;
	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	/// <summary>
	/// Parses the children nodes to create child elements
	/// </summary>
	/// <param name="elementNode">Element HTML node.</param>
	virtual public void parseChildNode(HtmlNode elementNode){
		HtmlAttribute attr_interact = elementNode.Attributes [AttributeManager.ATTR_INTERACTION];
		if (attr_interact != null) {
			InteractionType = (Interaction)System.Enum.Parse (typeof(Interaction),StringWrapper.ConvertToPascalCase(attr_interact.Value),true);
		}
	}
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
	/// Find SiblingIndex in UIElement structure
	/// </summary>
	virtual public int siblingIndex(){
		return 0;
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
	virtual public void updateGOProp(GameObject ElementGO){
	}
	virtual public void setChildParagraphRef(){

	}
	/// <summary>
	/// Hides the element GameObject.
	/// </summary>
	virtual public void hideElementGO(){
	}
	/// <summary>
	/// Displays the element GameObject.
	/// </summary>
	virtual public void displayElementGO(){
	}

}
