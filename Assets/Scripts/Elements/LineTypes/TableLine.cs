using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class TableLine  : Line {

	//-------------Common Attributes -------------------
	public int ColumnCount {get; set;}
	public int RowCount {get; set;}
	/// <summary>
	/// Gets or sets Flag indicating whether this <see cref="TableLine"/> has selectable cell and hence contain SelBtnHolder Component.
	/// </summary>
	/// <value><c>true</c> if table contains any Selectable Cell; otherwise, <c>false</c>.</value>
	public bool SelBtnFlag{ get; set; }


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public TableLine(){
	}
	//Constructor
	public TableLine(string type):base(){
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TableLine(HtmlNode line_node):base(line_node){
		prefabName = LocationManager.NAME_TABLE_LINE;
		HtmlAttribute attr_col = line_node.Attributes [AttributeManager.ATTR_COL_COUNT];
		if (attr_col != null) {
			ColumnCount = int.Parse (attr_col.Value);
		}

	}


	//-------------Based on Element Attributes, creating GameObject -------------------
	override protected void updateGOProp(GameObject ElementGO){
//		Debug.Log ("COLUMN_COUNT"+ColumnCount);
		ElementGO.GetComponent<UITable> ().columns = ColumnCount;
		SelBtnQuestionChecker selBtnHolder = ElementGO.GetComponent<SelBtnQuestionChecker> ();

		if (selBtnHolder != null) {
			selBtnHolder.ContainerElem = this;
			selBtnHolder.setParentCorrectCount (Parent as Paragraph, this);
		}
		BasicGOOperation.CheckAndRepositionTable (ElementGO);
	}

	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	//For Selectable Button Holder
	public void updateSelBtnHolder(GameObject _selBtnGO,bool _selBtnBool){
		SelBtnFlag = true;

		ElementGO.AddComponent<SelBtnQuestionChecker> (); 
		SelBtnQuestionChecker question = ElementGO.GetComponent<SelBtnQuestionChecker> ();
		//Ref Variables
		question.ContainerElem = this;
		question.addToMasterLine (this);
		//Seltn Specific Variables
		question.addSelectBtn (_selBtnGO, _selBtnBool);
		//Changing Paragraph's correctType
		question.setParentCorrectCount(Parent as Paragraph,this);
	}
	public GameObject addSubmitBtnGO(){
		return null;
	}
}
