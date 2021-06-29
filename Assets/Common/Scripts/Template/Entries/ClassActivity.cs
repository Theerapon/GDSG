﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassActivity : MonoBehaviour
{
    private TimeManager _timeManager;
    private ClassActivities_Template _definition;
    private bool _hasClass;
    private bool _isOpening;
    private ScheduleEvent _scheduleEvent;

    #region Get
    public string Id { get => _definition.Id; }
    public ClassActivityType ActivityType { get => _definition.ActivityType; }
    public string Activity_name { get => _definition.Activity_name; }
    public Sprite Icon { get => _definition.Icon; }
    public Day Day { get => _definition.Day; }
    public int Start_time_hour { get => _definition.Start_time_hour; }
    public int Start_time_minute { get => _definition.Start_time_minute; }
    public int End_time_hour { get => _definition.End_time_hour; }
    public int End_time_minute { get => _definition.End_time_minute; }
    public List<string> RegisterId { get => _definition.RegisterId; }
    public bool HasClass { get => _hasClass; }
    public bool IsOpening { get => _isOpening; }
    #endregion

    public ClassActivity(ClassActivities_Template classActivities_Template)
    {
        _definition = classActivities_Template;
        _timeManager = TimeManager.Instance;
        _scheduleEvent = ScheduleEvent.None;
    }

    public void EnableClass(ScheduleEvent scheduleEvent, Day currentDay)
    {
        if(Day == currentDay)
        {
            _scheduleEvent = scheduleEvent;
            _hasClass = true;
            Debug.Log(string.Format("Class {0} is opening", Activity_name));
        }
        
    }

    public void DisableClass()
    {
        _hasClass = false;
        Debug.Log(string.Format("Class {0} is closing", Activity_name));
    }
}
