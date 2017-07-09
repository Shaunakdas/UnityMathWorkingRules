using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager {
	//Prefabs Types Location
	public const string LOC_PREFAB_TYPE = "Prefabs/";
	//Elements Types Location
	public const string LOC_ELEMENT_TYPE = "Elements/";
	//Body Type Location
	public const string LOC_BODY_TYPE = "BodyTypes/";
	public const string COMPLETE_LOC_BODY_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_BODY_TYPE;
	//Body Prefabs
	public const string NAME_COMPREHENSION_BODY = "ComprehensionBodyPF";
	public const string NAME_PARA_COUNTER = "ParaCounterPF";
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
	public const string NAME_NUM_LINE_DROP_LINE = "NumberLineDropLinePF";
	public const string NAME_PRIME_DIV_LINE = "PrimeDivisionLinePF";
	public const string NAME_TEXT_LINE = "TextLinePF";
	public const string NAME_LATEX_TEXT_LINE = "LatexTextLinePF";
	public const string NAME_TABLE_LINE = "TableLinePF";
	public const string NAME_SUBMIT_LINE = "SubmitLinePF";
	//	public GameObject CombinationLinePF,NumLineDropLinePF,PrimeDivLinePF,TextLinePF,LatexTextLinePF,TableLinePF;

	//Row Types Location
	public const string LOC_ROW_TYPE = "RowTypes/";
	public const string COMPLETE_LOC_ROW_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_ROW_TYPE;
	//Row Prefabs
	public const string NAME_HORI_DRAG_SOURCE_LINE_ROW = "HoriDragSourceLineRowPF";
	public const string NAME_VERTI_DRAG_SOURCE_LINE_ROW = "VertiDragSourceLineRowPF";
	public const string NAME_HORIZONTAL_SCROLL_ROW = "HorizontalScrollRowPF";
//	public GameObject DragSourceLineRowPF;

	//Cell Types Location
	public const string LOC_CELL_TYPE = "CellTypes/";
	public const string COMPLETE_LOC_CELL_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_CELL_TYPE;
	//Cell Prefab Names
	public const string NAME_DRAG_SOURCE_CELL = "DragSourceCellPF";
	public const string NAME_DROP_ZONE_CELL = "DropZoneCellPF";
	public const string NAME_DROP_ZONE_TABLE_CELL = "DropZoneTableCellPF";
	public const string NAME_DROP_ZONE_TABLE_ITEM_CELL = "DropZoneTableItemCellPF";
	public const string NAME_DROP_ZONE_HOLDER_CELL = "DropZoneHolderPF";
	public const string NAME_LATEX_TEXT_CELL = "LatexTextCellPF";
	public const string NAME_TEXT_CELL = "TextCellPF";
	public const string NAME_TABLE_CELL = "TableLinePF";
	public const string NAME_FRACTION_TABLE_CELL = "FractionTableCellPF";
	public const string NAME_FRACTION_BAR = "FractionBarPF";

	public const string NAME_EXPONENT_TABLE_CELL = "ExponentTableCellPF";
	public const string NAME_SELECT_BTN_CELL = "SelectBtnCellPF";
	public const string NAME_SELECT_SIGN_CELL = "SelectSignCellPF";
	public const string NAME_NUM_LINE_LABEL_CELL = "NumLineLabelCellPF";
	public const string NAME_NUM_LINE_MARK_BIG_CELL =  "NumLineMarkBigCellPF";
	public const string NAME_NUM_LINE_MARK_SMALL_CELL =  "NumLineMarkSmallCellPF";
	public const string NAME_PRIME_DIV_LEVEL_CELL = "PrimeDivLevelCellPF";
	//	public GameObject DragSourceCellPF,DropZoneCellPF,LatexTextCellPF,TextCellPF,TableCellPF,SelectBtnCellPF,SelectSignCellPF,NumLineLabelCellPF;

	//Other Types Location
	public const string LOC_OTHER_TYPE = "OtherTypes/";
	public const string COMPLETE_LOC_OTHER_TYPE = LOC_PREFAB_TYPE+LOC_ELEMENT_TYPE+LOC_OTHER_TYPE;
	//Other Prefabs;
	public const string NAME_CENTER_CONTENT_SCROLL_VIEW = "CenterContentScrollViewPF";
	public const string NAME_CONTENT_TABLE = "ContentTable";
	public const string NAME_EMPTY_CONTAINER = "EmptyContainer";
	public const string NAME_START_WORKING_RULE_BTN = "StartWorkingRuleBtnPF";
	public const string NAME_POST_SUBMIT_TABLE = "PostSubmitTablePF";

}
