﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguesExploration_DataHandler : MonoBehaviour
{
    protected Dictionary<string, DialoguesExploration_Template> dialoguesExplorationDic;
    [SerializeField] private DialoguesExplorationVM dialoguesExplorationVM;
    [SerializeField] private InterpretHandler interpretHandler;

    public Dictionary<string, DialoguesExploration_Template> GetDialogueExplorationDic
    {
        get { return dialoguesExplorationDic; }
    }

    protected void Awake()
    {
        dialoguesExplorationDic = new Dictionary<string, DialoguesExploration_Template>();
    }

    private void Start()
    {
        interpretHandler.EventOnPreparingInterpretData.AddListener(EventInterpretHandler);
    }

    private void EventInterpretHandler()
    {
        dialoguesExplorationDic = dialoguesExplorationVM.Interpert();
        //Debug.Log("DialoguesExploration interpret completed");
        //foreach (KeyValuePair<string, DialoguesExploration_Template> diaExploration in dialoguesExplorationDic)
        //{
        //    Debug.Log(string.Format("ID {0}, Count Dialogues = {1}, Spawn ID = {2}", diaExploration.Key, diaExploration.Value.DialoguesList.Count, diaExploration.Value.SpawnItemId));
        //}
    }
}
