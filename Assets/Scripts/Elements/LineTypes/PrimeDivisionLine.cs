using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimeDivisionLine : Line {
	public Type PrimeDivisionType{get; set;}
	public int TargetInt{get; set;}
	//Constructor
	public PrimeDivisionLine(string integer, string type){
		TargetInt = int.Parse(integer);
		if (type == "prime_division") PrimeDivisionType = Type.PrimeDivision ;
	}
}

