﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassActivityController : Manager<ClassActivityController>
{
    private ClassActivities_DataHandler _classActivities_DataHandler;
    private Dictionary<string, ClassActivity> _classActivitiesDic;

    public Dictionary<string, ClassActivity> ClassActivitiesDic { get => _classActivitiesDic; }

    [SerializeField] private TimeManager _timeManager;

    protected override void Awake()
    {
        base.Awake();
        _classActivitiesDic = new Dictionary<string, ClassActivity>();
        _classActivities_DataHandler = FindObjectOfType<ClassActivities_DataHandler>();
        _timeManager = TimeManager.Instance;
        _timeManager.OnTenMinute.AddListener(OnTenMinuteHandler);

        //initializing Class activity
        if (!ReferenceEquals(_classActivities_DataHandler.GetClassActivitiesDic, null))
        {
            foreach (KeyValuePair<string, ClassActivities_Template> classActivity in _classActivities_DataHandler.GetClassActivitiesDic)
            {
                _classActivitiesDic.Add(classActivity.Key, new ClassActivity(classActivity.Value));
            }
        }
    }

    private void OnTenMinuteHandler()
    {
        float hour = _timeManager.Hour;
        float minute = _timeManager.Minute;

        foreach (KeyValuePair<string, ClassActivity> classActivity in _classActivitiesDic)
        {
            if (classActivity.Value.HasClass)
            {
                classActivity.Value.CheckTimeToOpen(hour, minute);
            }
        }
    }

    public void EnableClass(ClassActivityType classActivityType, ScheduleEvent scheduleEvent, Day day)
    {
        foreach(KeyValuePair<string, ClassActivity> classActivity in _classActivitiesDic)
        {
            if(classActivityType == classActivity.Value.ActivityType)
            {
                classActivity.Value.EnableClass(scheduleEvent, day);
            }
        }
    }

    public void ClearClassEvent()
    {
        foreach (KeyValuePair<string, ClassActivity> classActivity in _classActivitiesDic)
        {
            classActivity.Value.DisableClass();
        }
    }
}
