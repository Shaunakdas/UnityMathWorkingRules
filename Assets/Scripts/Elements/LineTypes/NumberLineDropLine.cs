using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class NumberLineDropLine  : Line {
	public int LabelCount{ get; set; }
	NumberLineDisplay displayCtrl;

	List<GameObject> NumberMarkerList{ get; set; }
	//Flag to display Number Markers Title
	public bool displayNumber{get; set;}
	//For NumberLineDropJump
	public int DropStartIndex{get; set;}
	public int DropCount{ get; set; }
	public int JumpSize{ get; set; }

	///<summary>
	/// Constructor for NumberLine and NumberLineDrop
	/// </summary>
	public NumberLineDropLine(string labelCount, string type){	
		LabelCount = int.Parse(labelCount);
		getLineType (type);
	}

	///<summary>
	/// Constructor for NumberLineDropLine
	/// </summary>
	public NumberLineDropLine(string labelCount,string dropStartIndex,string dropCount,string jumpSize, string type){
		RowList = new List<Row>();	
		LabelCount = int.Parse(labelCount);
		getLineType (type);
		if (type == "number_line_drop_jump") {
			//Only for NumberLine Drop Jump
			DropStartIndex = int.Parse (dropStartIndex);
			DropCount = int.Parse (dropCount);
			JumpSize = int.Parse (jumpSize);
		}
	}
	/// <summary>
	/// Initializes a new instance of the NumberLineDropLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public NumberLineDropLine(HtmlNode line_node){
		RowList = new List<Row>();NumberMarkerList = new List<GameObject> ();
		string type = line_node.Attributes [HTMLParser.ATTR_TYPE].Value;
		Debug.Log ("Initializing NumberLineDropLine node of type text"+ type);
		LabelCount = int.Parse(line_node.Attributes [HTMLParser.ATTR_LABEL_COUNT].Value);
		getLineType (line_node.Attributes [HTMLParser.ATTR_TYPE].Value);
		getLocationType (line_node.Attributes [HTMLParser.ATTR_LOCATION_TYPE].Value);

		if (type == "number_line_drop_jump") {
			//Only for NumberLine Drop Jump
			DropStartIndex = int.Parse (line_node.Attributes [HTMLParser.ATTR_DROP_START_INDEX].Value);
			DropCount = int.Parse (line_node.Attributes [HTMLParser.ATTR_DROP_COUNT].Value);
			JumpSize = int.Parse (line_node.Attributes [HTMLParser.ATTR_JUMP_SIZE].Value);
		}

		prefabName = LocationManager.NAME_NUM_LINE_DROP_LINE;
	}
	public GameObject addChildGOToParentGO(BaseElement element){
		if (element.GetType () == typeof(NumberLineLabelCell)) {
			NumberLineLabelCell numberLineLabelCell = (NumberLineLabelCell)element;
			GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_TEXT_CELL)as GameObject;

			if (Type == LineType.NumberLineDrop) {
				if (numberLineLabelCell.Type == Cell.CellType.NumberLineLabel) {
					prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_TEXT_CELL)as GameObject;
				} else if (numberLineLabelCell.Type == Cell.CellType.NumberLineLabelAnswer) {
					prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_CELL)as GameObject;
				}
			} else if (Type == LineType.NumberLineSelect) {
				if (numberLineLabelCell.Type == Cell.CellType.NumberLineLabel) {
					prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_SELECT_BTN_CELL)as GameObject;
				} else if (numberLineLabelCell.Type == Cell.CellType.NumberLineLabelAnswer) {
					prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_SELECT_BTN_CELL)as GameObject;
				}
			}
			GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, NumberMarkerList [numberLineLabelCell.LabelIndex].transform);
			return cellGO;	
		} else {
			return null;
		}
	}


	public void getLineType(string type_text){
		switch (type_text) {
		case "number_line_drop": 
			Type = LineType.NumberLineDrop;
			break;
		case "number_line_select": 
			Type = LineType.NumberLineSelect;
			break;
		case "number_line_drop_jump": 
			Type = LineType.NumberLineDropJump;
			break;
		}
	}
	public void  initGOPRop(GameObject elementGO){
		ElementGO = elementGO;
		GameObject numberLineGrid = BasicGOOperation.getChildGameObject (ElementGO, "NumberLineGrid");
		initNumberLineGameObject (numberLineGrid);
	}
	override public void updateGOProp(GameObject elementGO){
	}
	public void initNumberLineGameObject(GameObject numberLineGrid){
		//Initialising Default Values
		displayCtrl.defaultValues ();
		//Changing values based on curent values
		displayCtrl.IntegerCount = LabelCount;
		//Calculatig dimensions
		displayCtrl.initNumberLineCalculations();

		numberLineGrid.GetComponent<UIGrid> ().cellHeight = displayCtrl.cellHeight;
		numberLineGrid.GetComponent<UIGrid> ().cellWidth = displayCtrl.cellWidth;
		numberLineGrid.GetComponent<UIGrid> ().Reposition ();

		GameObject smallMarkPF = Resources.Load (LocationManager.LOC_CELL_TYPE + LocationManager.NAME_NUM_LINE_MARK_SMALL_CELL)as GameObject;
		GameObject bigMarkPF = Resources.Load (LocationManager.LOC_CELL_TYPE + LocationManager.NAME_NUM_LINE_MARK_BIG_CELL)as GameObject;
		initNumberLineMarkers (numberLineGrid, smallMarkPF, bigMarkPF);
	}
	public void initNumberLineMarkers(GameObject numberLineGrid, GameObject smallMarkerPF, GameObject numberMarkerPF){

		//First Number Marker
		GameObject firstNumberMarkerGO = (GameObject) BasicGOOperation.InstantiateNGUIGO (numberMarkerPF,numberLineGrid.transform,"StartNumberMarker");
		addItemToBigMarker (firstNumberMarkerGO, LabelCount);
		NumberMarkerList.Add (firstNumberMarkerGO);
		for (int i = (displayCtrl.IntegerCount-1); i > -1; i--) {
			//First fill small markers
			for (int j = 0; j < displayCtrl.numberBreak-1; j++) {
				GameObject smallMarkerGO = (GameObject) BasicGOOperation.InstantiateNGUIGO (smallMarkerPF,numberLineGrid.transform,"SmallMarker_"+i+"_"+j);
			}
			//Fill Number Marker
			GameObject numberMarkerGO = (GameObject) BasicGOOperation.InstantiateNGUIGO (numberMarkerPF,numberLineGrid.transform,"NumberMarker"+i);
			addItemToBigMarker (numberMarkerGO, i);
			NumberMarkerList.Add (numberMarkerGO);
		}
	}
	public void addItemToBigMarker(GameObject numberMarkerGO, int number){
		GameObject dropZonePF = Resources.Load (LocationManager.LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_CELL)as GameObject;
		GameObject selectBtnPF = Resources.Load (LocationManager.LOC_CELL_TYPE + LocationManager.NAME_SELECT_BTN_CELL)as GameObject;
		GameObject itemGO;
		if (displayNumber) numberMarkerGO.GetComponentInChildren<TEXDrawNGUI> ().text = ( number).ToString() ;
		switch (Type) {
		case LineType.NumberLineDrop:
			itemGO = BasicGOOperation.InstantiateNGUIGO (dropZonePF, numberMarkerGO.transform); 
			//Set target text
			break;
		case LineType.NumberLineSelect:
			itemGO = BasicGOOperation.InstantiateNGUIGO (selectBtnPF, numberMarkerGO.transform); 
			SelectableButtonCell.updateSelectBtnGO (itemGO, number.ToString ());
			break;
		case LineType.NumberLineDropJump:
			itemGO = BasicGOOperation.InstantiateNGUIGO (dropZonePF, numberMarkerGO.transform); 
			break;
		}
	}
}
