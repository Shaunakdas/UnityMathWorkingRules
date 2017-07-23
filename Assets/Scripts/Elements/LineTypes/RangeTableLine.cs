using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class RangeTableLine  : TableLine {

	//-------------Common Attributes -------------------
	public int rangeStart = 0;
	public int rangeEnd = 0;
	public int rangeCount = 0;


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public RangeTableLine(){
	}
	//Constructor
	public RangeTableLine(string type):base(type){
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public RangeTableLine(HtmlNode line_node):base(line_node){
	}
	override public void parseChildNode(HtmlNode line_node){
		IEnumerable<HtmlNode> node_list = line_node.Elements(AttributeManager.TAG_ROW) ;
		if (node_list!=null) {
			foreach (HtmlNode row_node in node_list) {
				Debug.Log ("Content of row_node : " + row_node.InnerHtml);
				Row row = new Row (row_node);
				row.Parent = this;
				RowList.Add(row);
			}
		}
	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	override public void updateGOProp(GameObject _elementGO){
		base.updateGOProp(_elementGO);
	}

	//-------------Static methods to create/update GameObject components for Correct/Incorrect Check-------------------
	//For Selectable Button Holder
	public void updateSelBtnHolder(GameObject _selBtnGO,bool _selBtnBool){
		base.updateSelBtnHolder (_selBtnGO, _selBtnBool);
	}
	public GameObject addSubmitBtnGO(){
		return null;
	}
}
