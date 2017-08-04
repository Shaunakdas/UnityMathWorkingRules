using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager  {
	public const string BODY_TAG = "body";
	public const string TAG_BODY = "body",TAG_P = "p",TAG_LINE = "line",TAG_ROW="trow",TAG_CELL = "tcell";
	public const string ATTR_ANALYTICS_ID = "analyticsId";
	public const string ATTR_INTERACTION = "interaction";
	public const string ATTR_ANSWER = "answer",ATTR_TYPE="type";
	public const string ATTR_COL_COUNT = "col";
	//QuestionStep Paragraph
	public const string ATTR_CORRECT_TYPE = "correctType",ATTR_ALIGN="align";
	//NumberLineDrop Line
	public const string ATTR_LABEL_COUNT = "labelCount",ATTR_LABEL_INDEX = "labelIndex";
	//NumberLineDropJump Line
	public const string ATTR_DROP_START_INDEX="dropStartIndex",ATTR_DROP_COUNT="dropStartIndex",ATTR_JUMP_SIZE="attrJumpSize";
	//Combination Line 
	public const string ATTR_OUTPUT_VISIBLE="outputVisible";
	//RangeTable Line
	public const string ATTR_COUNT = "count",ATTR_SORT_ORDER = "sortOrder", ATTR_CORRECT_ITEM_TYPE = "correctItemType", ATTR_CORRECT_TARGET = "correctTarget", ATTR_ELIMINATE="eliminate", ATTR_CORRECTION_COUNT = "correctionCount";
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
	//Score Tracking
	public const string MAX_SCORE="maxScore",MIN_SCORE="minScore",MAX_QUES_SCORE="maxQuesScore",MIN_QUES_SCORE="minQuesScore",MAX_OPTION_SCORE="maxOptionScore",MIN_OPTION_SCORE="minOptionScore";
	public const string SCORE_WEIGHTAGE="scoreWeightage";
	//Time Tracking
	public const string MAX_TIME="maxTime",IDEAL_TIME="idealTime",MAX_QUES_TIME="maxQuesTime",IDEAL_QUES_TIME="idealQuesTime",MAX_OPTION_TIME="maxOptionTime",IDEAL_OPTION_TIME="idealOptionTime";

}
