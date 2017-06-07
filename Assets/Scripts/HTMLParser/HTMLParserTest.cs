using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;

public class HTMLParserTest : MonoBehaviour {
	//List of Paragraphs of questions
	public List<Paragraph> ParagraphList;

	//Prefabs
	//Cell Prefabs
	public GameObject DragSourceCellPF,DropZoneCellPF,LatexTextCellPF,TextCellPF,TableCellPF,SelectBtnCellPF,SelectSignCellPF,NumLineLabelCellPF;
	//Line Prefabs
	public GameObject CombinationLinePF,NumLineDropLinePF,PrimeDivLinePF,TextLinePF,LatexTextLinePF,TableLinePF;
	//Paragraph Prefabs
	public GameObject QuestionStepParaPF,ComprehensionParaPF;
	//Row Prefabs
	public GameObject DragSourceLineRowPF;
	//Miscellanous PRefabs;
	//GameObjects
	// Use this for initialization
	public GameObject CenterContentScrollViewPF;
	void Start () {
		HTMLParser parser = new HTMLParser ();

		string text = System.IO.File.ReadAllText (@"Assets/Data/Question_Data.html");
		var html = new HtmlDocument ();
		html.LoadHtml (@text);
		parser.getParagraphList (html);
		ParagraphList = parser.ParagraphList;
		generateParaUI ();
		BasicGOOperation.RepositionChildTables (gameObject);
	}
	public void generateParaUI(){
		Paragraph para = ParagraphList [0];
		Debug.Log ((para.ParagraphStep == Paragraph.StepType.QuestionStep));
		if (para.ParagraphStep == Paragraph.StepType.QuestionStep) {
			//Paragraph is of type QuestionStep

			//Adding QuestionStepParaPF to the root GameObject
			GameObject QuestionStepParaGO = BasicGOOperation.InstantiateNGUIGO(QuestionStepParaPF,gameObject.transform);

			GameObject ParaContentTableGO = BasicGOOperation.getChildGameObject (QuestionStepParaGO, "ParaContentTable");
			//Getting LineList
			List<Line> LineList = para.LineList;

			//Checking for CenterContentScrollView
			GameObject LineTableGO = ParaContentTableGO;
			foreach (Line line in LineList) if (line.LineLocation == Line.LocationType.Default)
			{
				GameObject CenterContentGO = BasicGOOperation.InstantiateNGUIGO(CenterContentScrollViewPF,ParaContentTableGO.transform);
				LineTableGO = BasicGOOperation.getChildGameObject (CenterContentGO, "LineTablePF");
					break;
			}

			foreach (Line line in LineList) {
				generateLineUI (line,ParaContentTableGO,LineTableGO );
			}
		}
	}
	public void generateLineUI(Line line, GameObject ContentTableGO, GameObject CenterContentGO){
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_LINE_TYPE + line.prefabName)as GameObject;

		GameObject lineGO;
		//Based on line index and line type add line to the top/center/bottom of ContentTableGO
		switch (line.LineLocation) {
		case Line.LocationType.Top:
			//Adding lineGO to the ContentTableGO at the top
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, ContentTableGO.transform);
			lineGO.transform.SetAsFirstSibling ();
			break;
		case Line.LocationType.Default:
			//Adding QuestionStepParaPF to the root GameObject
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, CenterContentGO.transform);
			break;
		case Line.LocationType.Bottom:
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, ContentTableGO.transform);
			lineGO.transform.SetAsLastSibling ();
			break;
		default:
			lineGO = BasicGOOperation.InstantiateNGUIGO (prefab, CenterContentGO.transform);
			break;
		}

		line.updateGOProp (lineGO);
		foreach (Row row in line.RowList) {
			generateRowUI (row,lineGO );
		}
	}

	public void generateRowUI(Row row, GameObject lineGO){
		GameObject parentGO;
		//Based on row index and row type add row to the top/center/bottom of ContentTableGO
		switch (row.Type) {
		case Row.RowType.Default:
			//keeping lineGO as parent
			parentGO = lineGO;
			//Updating Column count of Parent Table based on cell
			if (row.Parent.GetType () == typeof(TableLine)) {
				Debug.Log ("Number of columns " + row.CellList.Count);
				lineGO.GetComponent<UITable> ().columns = row.CellList.Count;
			}
			break;
		case Row.RowType.DragSource:
			GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_ROW_TYPE + row.prefabName)as GameObject;
			GameObject rowGO = BasicGOOperation.InstantiateNGUIGO (prefab, lineGO.transform);
			//making Grid child of ScrollView as parent
			GameObject HorizontalScrollView = BasicGOOperation.getChildGameObject (rowGO, "ScrollView");
			parentGO = BasicGOOperation.getChildGameObject (HorizontalScrollView, "Grid");
			row.updateGOProp (rowGO);
			break;
		default:
			parentGO = lineGO;
			//Updating Column count of Parent Table based on cell
			if (row.Parent.GetType () == typeof(TableLine)) {
				Debug.Log ("Number of columns " + row.CellList.Count);
				lineGO.GetComponent<UITable> ().columns = row.CellList.Count;
			}
			break;
		}

		foreach (Cell cell in row.CellList) {
			generateCellUI (cell,parentGO );
		}
	}
	public void generateCellUI(Cell cell, GameObject parentGO){
		Debug.Log (LocationManager.COMPLETE_LOC_CELL_TYPE + cell.prefabName);
		GameObject prefab = Resources.Load (LocationManager.COMPLETE_LOC_CELL_TYPE + cell.prefabName)as GameObject;
		GameObject cellGO = BasicGOOperation.InstantiateNGUIGO (prefab, parentGO.transform);
		cell.updateGOProp (cellGO);
		if (cell.GetType()  == typeof(TableCell)) {
			//Handle TableCell here
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

}
