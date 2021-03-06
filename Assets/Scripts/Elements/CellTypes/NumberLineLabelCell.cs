﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class NumberLineLabelCell : Cell {

	//-------------Common Attributes -------------------
	public enum LabelType{Label,LabelAnswer}
	public LabelType LineLabel{ get; set; }

	public int LabelIndex{ get; set; }
	//Constructor
	public NumberLineLabelCell(string type, string labelIndex):base(type){
		LabelIndex = int.Parse(labelIndex);
	}

	public Vector3 labelDefaultLocation{ get; set; }
	public Vector2 labelDefaultSize{ get; set; }
	public void setDefaultProp(){
		labelDefaultLocation = new Vector3 (-60f, 0f, 0f);
		labelDefaultSize = new Vector2 (100f, 60f);
	}


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	public void getLabelType(string type_text){
		switch (Type) {
		case CellType.NumberLineLabel: 
			LineLabel = LabelType.Label;
			break;
		case CellType.NumberLineLabelAnswer: 
			LineLabel = LabelType.LabelAnswer;
			break;
		}

	}
	/// <summary>
	/// Initializes a new instance of the NumberLineLabelCell class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public NumberLineLabelCell(HtmlNode cell_node):base(cell_node){
		//		CellList = new List<Cell> ();
		setDefaultProp ();
		getLabelType(cell_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		LabelIndex = int.Parse(cell_node.Attributes [AttributeManager.ATTR_LABEL_INDEX].Value);
		prefabName = LocationManager.NAME_NUM_LINE_LABEL_CELL;
		DisplayText = cell_node.InnerText;
	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	/// <summary>
	/// Generates the Element GameObjects.
	/// </summary>
	/// <param name="parentGO">Parent G.</param>
	override public GameObject generateElementGO(GameObject parentGO){

		NumberLineDropLine numberLine = (NumberLineDropLine)this.Parent.Parent;
		GameObject cellGO = numberLine.addItemToBigMarker (LabelIndex,DisplayText,LineLabel );
		updateGOProp (cellGO);
		BasicGOOperation.CheckAndRepositionTable (cellGO);
		return cellGO;
	}
}
