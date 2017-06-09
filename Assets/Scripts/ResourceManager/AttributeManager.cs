using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager  {

	public const string BODY_TAG = "body";
	public const string TAG_P = "p",TAG_LINE = "line",TAG_ROW="trow",TAG_CELL = "tcell";
	public const string ATTR_ANSWER = "answer",ATTR_TYPE="type";
	public const string ATTR_COL_COUNT = "col";
	//QuestionStep Paragraph
	public const string ATTR_CORRECT_TYPE = "correctType",ATTR_ALIGN="align";
	//NumberLineDrop Line
	public const string ATTR_LABEL_COUNT = "labelCount",ATTR_LABEL_INDEX = "labelIndex";
	//NumberLineDropJump Line
	public const string ATTR_DROP_START_INDEX="dropStartIndex",ATTR_DROP_COUNT="dropStartIndex",ATTR_JUMP_SIZE="attr_jump_size";
	//Combination Line 
	public const string ATTR_OUTPUT_VISIBLE="outputVisible";
	//DragSourceLine Row 
	public const string ATTR_START="start",ATTR_END="end",ATTR_SOURCE_TYPE="sourceType";
	//DropZone Row
	public const string ATTR_SIZE="size";
	//DragSource and DropZoneRow Cell 
	public const string ATTR_ID="id",ATTR_TAG="tag";
	//PrimeDisivion Line
	public const string ATTR_TARGET="target";
	//Line Location
	public const string ATTR_LOCATION_TYPE="locationType";

}
