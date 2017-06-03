using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line {
	public enum Type {Text,PostSubmitText,IncorrectSubmitText,Table,NumberLineDrop,NumberLineDropJump,NumberLineSelect,PrimeDivision,CombinationProduct,CombinationSum,CombinationProductSum};


	public List<Row> RowList {get; set;}

	//Constructor
	public Line(){
	}


}
