﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class PrimeDivisionLine : Line {
	public int TargetInt{get; set;}
	PrimeDivision primeDivision;
	//Constructor
	public PrimeDivisionLine(string integer, string type){
		RowList = new List<Row>();
		TargetInt = int.Parse(integer);
		getLineType (type);  
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public PrimeDivisionLine(HtmlNode line_node){
		
		RowList = new List<Row>();
		Debug.Log ("Initializing PrimeDivisionLine node of type text"+ line_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		getLineType (line_node.Attributes [AttributeManager.ATTR_TYPE].Value);
		getLocationType (line_node.Attributes [AttributeManager.ATTR_LOCATION_TYPE].Value);
		TargetInt = int.Parse(line_node.Attributes [AttributeManager.ATTR_TARGET].Value);
		primeDivision = new PrimeDivision (TargetInt);

		prefabName = LocationManager.NAME_PRIME_DIV_LINE;
	}
	public void getLineType(string type_text){
		switch (type_text) {
		case "text": 
			Type = LineType.PrimeDivision;
			break;
		}
	}
	override public void initGOProp(GameObject elementGO){
		ElementGO = elementGO;
		GameObject primeDivisionGrid = BasicGOOperation.getChildGameObject (ElementGO, "PrimeDivisionGrid");
		initPrimeDivLineGameObject (primeDivisionGrid);
		Vector3 finalSize = NGUIMath.CalculateAbsoluteWidgetBounds (elementGO.transform).size;
		elementGO.GetComponent<UISprite> ().width = (int)finalSize.x; elementGO.GetComponent<UISprite> ().height = (int)finalSize.y;
	}
	public void initPrimeDivLineGameObject(GameObject primeDivisionGrid){
		int levelCount = primeDivision.PrimeFactorList.Count;
		GameObject primeDivisionLevelPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_PRIME_DIV_LEVEL_CELL)as GameObject;

		for (int i = 0; i < levelCount; i++) {
			GameObject primeDivisionLevel = BasicGOOperation.InstantiateNGUIGO (primeDivisionLevelPF, primeDivisionGrid.transform);
			populatePrimeDivLevel (primeDivisionLevel,i);
		}
	}
	public void populatePrimeDivLevel(GameObject primeDivisionLevel, int index){
		//Adding Dividend
		int dividend = primeDivision.PrimeDividendList[index];
		GameObject dividendLabelCellPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_LATEX_TEXT_CELL)as GameObject;
		GameObject dividendLabelCell = BasicGOOperation.InstantiateNGUIGO (dividendLabelCellPF, primeDivisionLevel.transform);
		dividendLabelCell.GetComponent<TEXDrawNGUI>().text = dividend.ToString();dividendLabelCell.GetComponent<TEXDrawNGUI> ().autoFit = TexDrawLib.Fitting.Off;
		//Setting the size and location
		dividendLabelCell.GetComponent<TEXDrawNGUI>().height = 90;dividendLabelCell.GetComponent<TEXDrawNGUI>().width = 72;
		Vector3 divLocation = dividendLabelCell.transform.localPosition; divLocation.x = 53; dividendLabelCell.transform.localPosition = divLocation;

		//Adding Divisor
		int factor = primeDivision.PrimeFactorList[index];
		GameObject factorDropCellPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_CELL)as GameObject;
		GameObject factorDropCell = BasicGOOperation.InstantiateNGUIGO (factorDropCellPF, primeDivisionLevel.transform);
		Vector3 factorLocation = factorDropCell.transform.localPosition; factorLocation.x = -50;factorDropCell.transform.localPosition = factorLocation;
	}

}

