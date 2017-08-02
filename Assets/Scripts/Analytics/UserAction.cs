using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserAction {

	public static TargetEntity MasterEntity = new TargetEntity();
	static List<AnalyticsEvent> EventList = new List<AnalyticsEvent>();

	public static void addUserEvent(TargetEntity _target,int _targetId,AnalyticsEvent.Event _event,System.DateTime _time){ 
		AnalyticsTracker.addEvent(_target,_targetId,_event,_time); 
		EventList.Add( new AnalyticsEvent(_target,_targetId,_event,_time)); 
	}
}
