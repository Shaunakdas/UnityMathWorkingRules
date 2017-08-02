using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEntity {
	public enum Type{ComprehensionBody,Paragraph,Line,Question,Option,API}
	public Type EntityType = Type.ComprehensionBody;

	public int EntityId = 0;

	public List<TargetEntity> ChildEntityList;

	public TargetEntity(){
	}
	public TargetEntity(string _elementType,int _entityId){
		EntityType = (Type)System.Enum.Parse (typeof(Type),_elementType,true);
		EntityId = _entityId;
	}
}
