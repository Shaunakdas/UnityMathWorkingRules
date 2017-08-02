using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnalyticsTracker  {
	static List<AnalyticsEvent> EventList;

	public static void addEvent(TargetEntity _target,int _targetId,AnalyticsEvent.Event _event,System.DateTime _time){  
		EventList.Add( new AnalyticsEvent(_target,_targetId,_event,_time)); 
	}
}
