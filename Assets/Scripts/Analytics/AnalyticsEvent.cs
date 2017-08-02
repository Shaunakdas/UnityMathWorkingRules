using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsEvent {
	public enum Event{StartRequest,Start,Visible,VisibilityEnd,EndRequest,End,Activated,Attempted,CorrectAnswered,IncorrectAnswered,
		Corrected,Deactivated,RequestStart,ResponseReceived,ResponseFailed,ResponseTimeout}
	public Event ActionEvent = Event.Activated;

	TargetEntity EventEntity;
	int TargetEntityId = 0;

	System.DateTime EventTime = System.DateTime.Now;

	public AnalyticsEvent(){
	}
	public AnalyticsEvent(TargetEntity _target,int _targetId,Event _event,System.DateTime _time){
		EventEntity = _target;
		TargetEntityId = _targetId;
		ActionEvent = _event;
		EventTime = _time;
	}
}
