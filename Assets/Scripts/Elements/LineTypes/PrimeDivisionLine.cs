using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class PrimeDivisionLine : Line {

	//-------------Common Attributes -------------------
	public int TargetInt{get; set;}
	PrimeDivision primeDivision;


	//-------------Parsing HTML Node and initiating Element Attributes -------------------
	//Constructor
	public PrimeDivisionLine(string integer, string type):base(){
		TargetInt = int.Parse(integer); 
	}
	/// <summary>
	/// Initializes a new instance of the TableLine class with HTMLNode attribute
	/// </summary>
	/// <param name="para">Para.</param>
	public PrimeDivisionLine(HtmlNode line_node):base(line_node){
		TargetInt = int.Parse(line_node.Attributes [AttributeManager.ATTR_TARGET].Value);
		primeDivision = new PrimeDivision (TargetInt);

		prefabName = LocationManager.NAME_PRIME_DIV_LINE;
	}

	//-------------Based on Element Attributes, creating GameObject -------------------
	override protected void initGOProp(GameObject elementGO){
		ElementGO = elementGO;
		GameObject primeDivisionGrid = BasicGOOperation.getChildGameObject (ElementGO, "PrimeDivisionGrid");
		initPrimeDivLineGameObject (primeDivisionGrid);
		BasicGOOperation.ResizeToFitChildGO (elementGO);
	}
	/// <summary>
	/// Initializes PrimeDivision List GameObject
	/// </summary>
	/// <param name="primeDivisionGrid">Prime division grid.</param>
	public void initPrimeDivLineGameObject(GameObject primeDivisionGrid){
		int levelCount = primeDivision.PrimeFactorList.Count;
		GameObject primeDivisionLevelPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_PRIME_DIV_LEVEL_CELL)as GameObject;
		Row row = new Row ();
		row.Parent = this;
		RowList.Add (row);
		for (int i = 0; i < levelCount; i++) {
			GameObject primeDivisionLevel = BasicGOOperation.InstantiateNGUIGO (primeDivisionLevelPF, primeDivisionGrid.transform);
			populatePrimeDivLevel (primeDivisionLevel,i,row);
		}
	}
	/// <summary>
	/// Populates each prime div level.
	/// </summary>
	/// <param name="primeDivisionLevel">Prime division level GameObject</param>
	/// <param name="index">Index.</param>
	public void populatePrimeDivLevel(GameObject primeDivisionLevel, int index, Row row){
		//Adding Dividend
		int dividend = primeDivision.PrimeDividendList[index];
		GameObject dividendLabelCellPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_LATEX_TEXT_CELL)as GameObject;
		GameObject dividendLabelCell = BasicGOOperation.InstantiateNGUIGO (dividendLabelCellPF, primeDivisionLevel.transform);
		dividendLabelCell.GetComponent<TEXDrawNGUI>().text = dividend.ToString();dividendLabelCell.GetComponent<TEXDrawNGUI> ().autoFit = TexDrawLib.Fitting.Off;
		//Setting the size and location
		dividendLabelCell.GetComponent<TEXDrawNGUI>().height = 90;dividendLabelCell.GetComponent<TEXDrawNGUI>().width = 72;
		float scale_x = ((float)ScreenManager.ScreenWidth() / 480);
		float scale_y = ((float)Screen.height / 900);
		Vector3 divLocation = dividendLabelCell.transform.localPosition; divLocation.x = 53*scale_x; dividendLabelCell.transform.localPosition = divLocation;

		//Adding Divisor
		int factor = primeDivision.PrimeFactorList[index];
//		GameObject factorDropCellPF = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + LocationManager.NAME_DROP_ZONE_CELL)as GameObject;
//		GameObject factorDropCell = BasicGOOperation.InstantiateNGUIGO (factorDropCellPF, primeDivisionLevel.transform);
		//To access DropZoneRowCell methods
		PrimeDropZoneRowCell dropCell = new PrimeDropZoneRowCell (this.ParagraphRef);
		row.CellList.Add (dropCell);
		dropCell.Parent = row;
		GameObject factorDropCell = dropCell.generateDropZoneHolderGO(primeDivisionLevel,StringWrapper.splitTargetText(factor.ToString()),false);
		Vector3 factorLocation = factorDropCell.transform.localPosition;

		factorLocation.x = -75*scale_x;
		factorLocation.y = 50*scale_y;
		factorDropCell.transform.localPosition = factorLocation;
	}
//	override public GameObject generateElementGO(GameObject parentGO){
////		GameObject _elementGO =   generateDropZoneHolderGO (parentGO, TargetTextList, idPresent);
////		ElementGO = _elementGO;
//		//_elementGO add to list of aniamtion
//		return ElementGO;
//	}
}

