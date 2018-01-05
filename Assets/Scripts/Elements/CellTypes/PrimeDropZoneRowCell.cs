using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimeDropZoneRowCell : DropZoneRowCell {

	//-------------Based on Element Attributes, creating GameObject -------------------

	public PrimeDropZoneRowCell(Paragraph paraRef):base(){
		ParagraphRef = paraRef;
	}
	override public GameObject generateElementGO(GameObject parentGO){
		return ElementGO;
	}
}
