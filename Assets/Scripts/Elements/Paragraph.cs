using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
public class Paragraph : BaseElement{
	public enum StepType {Comprehension,QuestionStep};
	public StepType ParagraphStep;

	public enum AlignType {Vertical,Horizontal};
	public AlignType ParagraphAlign;

	public int tableCol;
	//For all types
	public List<Line> LineList{get; set;}

	//For QuestionStep 
	public enum CorrectType {SingleCorrect,MultipleCorrect}
	public CorrectType ParagraphCorrect;
	public Paragraph(){
	}
	/// <summary>
	/// Initializes a new instance of the Paragraph class with correctType attribute
	/// </summary>
	/// <param name="correctType">Correct type.</param>
	public Paragraph(string correctType){
		switch (correctType) {
		case "single_correct": 
			ParagraphCorrect = CorrectType.SingleCorrect;
			break;
		case "multiple_correct": 
			ParagraphCorrect = CorrectType.MultipleCorrect;
			break;
		}
	}
	/// <summary>
	/// Initializes a new instance of the Paragraph class with HTMLNode attribute
	/// </summary>
	/// <param name="para"></param>
	public Paragraph(HtmlNode para_node){
		LineList = new List<Line> ();
		Debug.Log ("Initializing paragraph node of type "+para_node.Attributes [AttributeManager.ATTR_TYPE].Value);

		switch (para_node.Attributes [AttributeManager.ATTR_TYPE].Value) {
		case "comprehension": 
			ParagraphStep = StepType.Comprehension;
			prefabName = LocationManager.NAME_COMPREHENSION_PARA;
			break;
		case "question_step": 
			ParagraphStep = StepType.QuestionStep;
			prefabName = LocationManager.NAME_QUESTION_STEP_PARA;
			switch (para_node.Attributes [AttributeManager.ATTR_CORRECT_TYPE].Value) {
			case "single_correct": 
				ParagraphCorrect = CorrectType.SingleCorrect;
				break;
			case "multiple_correct": 
				ParagraphCorrect = CorrectType.MultipleCorrect;
				break;
			}
			switch (para_node.Attributes [AttributeManager.ATTR_ALIGN].Value) {
			case "horizontal": 
				ParagraphAlign = AlignType.Horizontal;
				tableCol = 0;
				break;
			case "vertical": 
				ParagraphAlign = AlignType.Vertical;
				tableCol = 1;
				break;
			}
			break;
		}

	}
	/// <summary>
	/// Parses the Paragraph Node to generate Line nodes
	/// </summary>
	public void parseParagraph(HtmlNode para_node){
//		HtmlNodeCollection node_list = para_node.SelectNodes ("//" + HTMLParser.LINE_TAG);
		IEnumerable<HtmlNode> node_list = para_node.Elements(AttributeManager.TAG_LINE) ;

		if (node_list != null) {
//			Debug.Log ("There are " + node_list.Count + " nodes of type: " + HTMLParser.LINE_TAG);

			foreach (HtmlNode line_node in node_list) {
				Line newLine = new Line (line_node);
				string line_type = line_node.Attributes [AttributeManager.ATTR_TYPE].Value;
				switch (line_type) {
				case "text": 
					newLine = new TextLine (line_node);
					break;
				case "post_submit_text": 
					newLine = new TextLine (line_node);
					break;
				case "incorrect_submit_text": 
					newLine = new TextLine (line_node);
					break;
				case "table": 
					newLine = new TableLine (line_node);
					break;
				case "prime_division": 
					newLine = new PrimeDivisionLine (line_node);
					break;
				case "number_line_drop": 
					newLine = new NumberLineDropLine (line_node);
					break;
				case "combination": 
					newLine = new CombinationLine (line_node);
					break;
				}
				//Populate Child Row nodes inside Line Node
				newLine.parseLine (line_node);

				newLine.Parent = this;
				//Add Line node into LineList
				LineList.Add (newLine);

			}
		}
	}
	override public GameObject generateElementGO(GameObject parentGO){
		//Setting targetText of child drop zone cell;
		populateCellTargetText ();
		GameObject QuestionStepParaPF = Resources.Load (LocationManager.COMPLETE_LOC_PARAGRAPH_TYPE + prefabName)as GameObject;
		GameObject ParaContentTableGO = null;
		GameObject CenterContentScrollViewPF = Resources.Load (LocationManager.COMPLETE_LOC_OTHER_TYPE + LocationManager.NAME_CENTER_CONTENT_SCROLL_VIEW)as GameObject;
		if (ParagraphStep == Paragraph.StepType.QuestionStep) {
			//Paragraph is of type QuestionStep

			//Adding QuestionStepParaPF to the root GameObject
			GameObject QuestionStepParaGO = BasicGOOperation.InstantiateNGUIGO(QuestionStepParaPF,parentGO.transform);

			ParaContentTableGO = BasicGOOperation.getChildGameObject (QuestionStepParaGO, "ParaContentTable");
			ParaContentTableGO.GetComponent<UITable> ().columns = tableCol;
			//Checking for Center Content Scroll View
			bool isCenterContentPresent = false; GameObject CenterContentGO = parentGO;
			foreach (Line line in LineList) if (line.LineLocation == Line.LocationType.Default)
			{
				CenterContentGO = BasicGOOperation.InstantiateNGUIGO(CenterContentScrollViewPF,ParaContentTableGO.transform);
				GameObject LineTableGO = BasicGOOperation.getChildGameObject (CenterContentGO, "LineTablePF");
				isCenterContentPresent = true;
				break;
			}

			foreach (Line line in LineList) {
				line.generateElementGO (ParaContentTableGO);

			}
			if (isCenterContentPresent)
				resizeCenterContent (CenterContentGO, ParaContentTableGO);
			BasicGOOperation.CheckAndRepositionTable (ParaContentTableGO);

		}
		return ParaContentTableGO;
	}
	public void resizeCenterContent(GameObject CenterContentGO,GameObject ParaContentTableGO){
		Vector2 remainingSize = ParaContentTableGO.transform.parent.gameObject.GetComponent<UISprite> ().localSize;
		//Calculate remaining width after subtracting from Parent height/width of PAraContentTableGO 
		foreach (Transform childTransform in ParaContentTableGO.transform) {
			if (childTransform.gameObject != CenterContentGO) {
				//Check for size of other GameObjects other than Default Types
				Bounds childBounds = NGUIMath.CalculateAbsoluteWidgetBounds(childTransform);
				remainingSize = remainingSize - new Vector2(childBounds.size.x,childBounds.size.y);
			}
		}
		UIPanel centerPanel = CenterContentGO.GetComponent<UIPanel> ();
		if (ParagraphAlign == AlignType.Horizontal) {
			//Change CenterContentGO.width
//			centerPanel.
//			centerPanel.width = remainingSize.x;

		} else {
			//Change CenterContentGO.height
//			centerPanel.height = remainingSize.y;
		}


	}
	//Populating Target Text based on DragSource Reference
	public void populateCellTargetText(){
		Debug.Log ("Populating Target Text of each drop zone cell");
		//Tracking all DropZone Cell TargetText
		List<DropZoneRowCell> dropZoneCellList = new List<DropZoneRowCell> ();
		List<DragSourceCell> dragSourceCellList = new List<DragSourceCell> ();
		//Parse through whole line list and its contents to get DragZoneRow Cell with valid id
		foreach (Line line in LineList) {
			foreach (Row row in line.RowList) {
				foreach (var cell in row.CellList) {
//					Debug.Log ("Traversing through cell List current type" + cell.GetType ().ToString () + cell.CellId);
					if ((cell.GetType () == typeof(DropZoneRowCell))&& (cell.CellId != null)) {
//						Debug.Log ("Found one Drop zone Cell");
						DropZoneRowCell dropCell = (DropZoneRowCell)cell;
						Debug.Log ("Found one Drop zone Cell" + dropCell.CellId);
						dropZoneCellList.Add (dropCell);
					}
				}
			}
		}
		foreach (Line line in LineList) {
			foreach (Row row in line.RowList) {
				foreach (var cell in row.CellList) {
					if ((cell.GetType () == typeof(DragSourceCell)) && (cell.CellId != null)) {
						DragSourceCell dragCell = (DragSourceCell)cell;
						Debug.Log ("Found one Drag source Cell"+ dragCell.CellId);
						dragSourceCellList.Add (dragCell);
					}
				}
			}
		}
		foreach (DragSourceCell dragCell in dragSourceCellList) {
			DropZoneRowCell dropZone = dropZoneCellList.Find (x => x.CellId == dragCell.CellId);
			Debug.Log ("Changing Target Text of id" + dragCell.DisplayText +dropZone.CellId);
			dropZone.TargetText = dragCell.DisplayText;
		}
	}
}
