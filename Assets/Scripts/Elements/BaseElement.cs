using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
public class BaseElement : MonoBehaviour {
	//-------------Common Attributes -------------------
	public Paragraph ParagraphRef;
	public string prefabName{get; set;}
	public HtmlNode htmlNode;
	//Parent BaseElement
	public BaseElement Parent{get; set;}
	public BaseElement CurrentChild{get; set;}
	/// <summary>
	/// Type of interaction in current element
	/// </summary>
	public enum Interaction{Drag,Drop,Select,Mixed,Display,Default};
	public Interaction InteractionType = Interaction.Display;

	public ScoreTracker scoreTracker = new ScoreTracker();
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
	/// <summary>
	/// Find SiblingIndex in UIElement structure
	/// </summary>
	virtual public int siblingIndex(){
		return 0;
	}
	virtual public void setChildParagraphRef(){
	}
	//-------------Based on Element sibling index, setting AnalyticsId -------------------
	public int AnalyticsId = 0;
	public void setAnalyticsIdFromAttr(HtmlNode elem_node){
		HtmlAttribute analyticsIdAttr = elem_node.Attributes [AttributeManager.ATTR_ANALYTICS_ID];
		if (analyticsIdAttr != null) {
			AnalyticsId = int.Parse(analyticsIdAttr.Value);
		}
	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// The element game object.
	/// </summary>
	public GameObject ElementGO;
	/// <summary>
	/// Sets the current child.
	/// </summary>
	/// <param name="_childElement">Child element.</param>
	virtual public void setCurrentChild(BaseElement _childElement){
		CurrentChild = _childElement;
	}
	/// <summary>
	/// Initiates the GameObject Default properties based on its corresponding class attribute
	/// </summary>
	/// <param name="ElementGO">Element GameObject</param>
	virtual protected void initGOProp(GameObject ElementGO){
	}

	/// <summary>
	/// Generates the Element GameObject
	/// </summary>
	/// <returns>The element Gameobject</returns>
	/// <param name="ElementGameObject">Element GameObject</param>
	virtual public GameObject generateElementGO(GameObject _elementGO){
		initGOProp (_elementGO);
		updateGOProp (_elementGO);
		ElementGO = _elementGO;
		return _elementGO;
	}
	/// <summary>
	/// Updates the GameObject properties based on its child values
	/// </summary>
	virtual protected void updateGOProp(GameObject ElementGO){
	}
	//-------------For Animations -------------------
	virtual public void setChildAnalyticsId(){
	}
	//-------------For Score Values -------------------
	virtual public void setChildScoreValues(){
		setupDefaultScoreValues ();
		adjustForWeightage();
		setupScoreValues ();
	}
	virtual protected void setupDefaultScoreValues(){
	}
	virtual protected void adjustForWeightage(){
	}
	virtual protected void setupScoreValues(){
	}
	virtual public void setupQuestionRef(){
	}
	//-------------For Animations -------------------
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


	//-------------Post Entity -------------------
	virtual public void postEntityOps(){
		postScoreCalc ();
		postStarCalc ();
		postDiffLevelCalc ();
	}
	protected void postScoreCalc(){
	}
	protected void postStarCalc(){
	}
	protected void postDiffLevelCalc(){
	}
}
