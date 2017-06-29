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
	public TableLine(string type){
		RowList = new List<Row>();
		getLineType (type);
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public TableLine(HtmlNode line_node):base(line_node){
		prefabName = LocationManager.NAME_TABLE_LINE;
	}
	override public void getLineType(string type_text){
		switch (type_text) {
		case "text": 
			Type = LineType.Table;
			break;
		}
	}


	//-------------Based on Element Attributes, creating GameObject -------------------
	override public void updateGOProp(GameObject ElementGO){
		ElementGO.GetComponent<UITable> ().columns = ColumnCount;
		SelBtnHolder selBtnHolder = ElementGO.GetComponent<SelBtnHolder> ();
		if (selBtnHolder != null) {
			selBtnHolder.setParentCorrectCount (Parent as Paragraph, this);
		}
		BasicGOOperation.CheckAndRepositionTable (ElementGO);
	}

	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	//For Selectable Button Holder
	public void updateSelBtnHolder(GameObject _selBtnGO,bool _selBtnBool){
		SelBtnFlag = true;
		ElementGO.AddComponent<SelBtnHolder> (); SelBtnHolder holderScript = ElementGO.GetComponent<SelBtnHolder> ();
		holderScript.addSelectBtn (_selBtnGO, _selBtnBool);
		//Changing Paragraph's correctType
		holderScript.setParentCorrectCount(Parent as Paragraph,this);
	}
	public GameObject addSubmitBtnGO(){
		return null;
	}
}
