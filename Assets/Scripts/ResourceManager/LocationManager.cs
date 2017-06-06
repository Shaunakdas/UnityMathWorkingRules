using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager {
	//Prefabs Types Location
	public const string LOC_PREFAB_TYPE = "Prefabs/";
	//Elements Types Location
	public const string LOC_ELEMENT_TYPE = "Elements/";

	//Paragraph Types Location
	public const string LOC_PARAGRAPH_TYPE = "ParagraphTypes/";
	public const string COMPLETE_LOC_PARAGRAPH_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_PARAGRAPH_TYPE;
	//Paragraph Prefabs
	public const string NAME_QUESTION_STEP_PARA = "QuestionStepParaPF";
	public const string NAME_COMPREHENSION_PARA = "ComprehensionParaPF";
//	public GameObject QuestionStepParaPF,ComprehensionParaPF;

	//Line Types Location
	public const string LOC_LINE_TYPE = "LineTypes/";
	public const string COMPLETE_LOC_LINE_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_LINE_TYPE;
	//Line Prefabs
	public const string NAME_COMBINATION_LINE = "CombinationLinePF";
	public const string NAME_NUM_LINE_DROP_LINE = "NumLineDropLinePF";
	public const string NAME_PRIME_DIV_LINE = "PrimeDivLinePF";
	public const string NAME_TEXT_LINE = "TextLinePF";
	public const string NAME_LATEX_TEXT_LINE = "LatexTextLinePF";
	public const string NAME_TABLE_LINE = "TableLinePF";
	//	public GameObject CombinationLinePF,NumLineDropLinePF,PrimeDivLinePF,TextLinePF,LatexTextLinePF,TableLinePF;

	//Row Types Location
	public const string LOC_ROW_TYPE = "RowTypes/";
	public const string COMPLETE_LOC_ROW_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_ROW_TYPE;
	//Row Prefabs
	public const string NAME_DRAG_SOURCE_LINE_ROW = "DragSourceLineRowPF";
//	public GameObject DragSourceLineRowPF;

	//Cell Types Location
	public const string LOC_CELL_TYPE = "CellTypes/";
	public const string COMPLETE_LOC_CELL_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_CELL_TYPE;
	//Cell Prefab Names
	public const string NAME_DRAG_SOURCE_CELL = "DragSourceCellPF";
	public const string NAME_DROP_ZONE_CELL = "DropZoneCellPF";
	public const string NAME_LATEX_TEXT_CELL = "LatexTextCellPF";
	public const string NAME_TEXT_CELL = "TextCellPF";
	public const string NAME_TABLE_CELL = "TableCellPF";
	public const string NAME_SELECT_BTN_CELL = "SelectBtnCellPF";
	public const string NAME_SELECT_SIGN_CELL = "SelectSignCellPF";
	public const string NAME_NUM_LINE_LABEL_CELL = "NumLineLabelCellPF";
	//	public GameObject DragSourceCellPF,DropZoneCellPF,LatexTextCellPF,TextCellPF,TableCellPF,SelectBtnCellPF,SelectSignCellPF,NumLineLabelCellPF;

	//Other Types Location
	public const string LOC_OTHER_TYPE = "OtherTypes/";
	public const string COMPLETE_LOC_OTHER_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_OTHER_TYPE;
	//Other Prefabs;
	public const string NAME_CENTER_CONTENT_SCROLL_VIEW = "CenterContentScrollViewPF";
}
