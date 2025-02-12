﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Display : MonoBehaviour
{
    #region Description
    private const string INST_CODING = "สเตตัส Coding ช่วยในการเขียนโปรแกรม";
    private const string INST_DESIGN = "สเตตัส Design ช่วยในการวิเคราะห์ปัญหา ออกแบบโครงสร้างระบบ";
    private const string INST_TESTING = "สเตตัส Testing ช่วยในการตรวจสอบความถูกต้องของโปรแกรม ว่าทำงานตรงตามความต้องการหรือไม่";
    private const string INST_ART = "สเตตัส Art ช่วยในการทำงานเกี่ยวกับภาพนิ่ง และภาพเคลื่อนไหว";
    private const string INST_SOUND = "สเตตัส Sound ช่วยในการทำงานเกี่ยวกับเรื่องเสียงภายในเกม เพื่อให้ผู้เล่นได้รับประสบการณ์ทางด้านอารมณ์เสียงให้สอดคล้องไปกับธีมของเกม";
    #endregion

    #region Events
    public Events.EventOnPointEnterStatusSlot OnPointEnterStatusSlot;
    public Events.EventOnPointExitStatusSlot OnPointExitStatusSlot;
    public Events.EventOnLeftClickStatusSlot OnLeftClickStatusSlot;
    public Events.EventOnStatusUpLevel OnStatusUpLevel;
    #endregion

    protected CharacterStatusController characterStatusController;
    protected PlayerAction playerAction;

    [SerializeField] private Transform itemsParent;
    public List<BaseStatusSlot> statusSlots;

    private int lastIndex;

    bool displayed = false;


    private void Awake()
    {
        //set Item Slots
        statusSlots = new List<BaseStatusSlot>();
        if (itemsParent != null)
            itemsParent.GetComponentsInChildren(includeInactive: true, result: statusSlots);

    }


    private void Start()
    {
        characterStatusController = CharacterStatusController.Instance;
        playerAction = PlayerAction.Instance;

        if(!ReferenceEquals(characterStatusController, null))
        {
            characterStatusController.OnStatusUpdated.AddListener(OnstatusUpdatedHandler);
        }

        for (int index = 0; index < statusSlots.Count; index++)
        {
            statusSlots[index].OnLeftClickStatusSlot.AddListener(OnLeftClickStatusSlotHandler);
            statusSlots[index].OnPointEnterStatusSlot.AddListener(OnPointEnterStatusSlotHandler);
            statusSlots[index].OnPointExitStatusSlot.AddListener(OnPointExitStatusSlotHandler);
        }

        displayed = false;
    }

    private void OnstatusUpdatedHandler()
    {
        DisplayedStatus();
        OnStatusUpLevel?.Invoke(statusSlots[lastIndex]);
    }

    private void Update()
    {
        if (!displayed)
        {
            DisplayedStatus();
            displayed = true;
        }
    }

    private void DisplayedStatus()
    {
        for (int i = 0; i < statusSlots.Count; i++)
        {
            if(!ReferenceEquals(playerAction, null))
            {
                switch (statusSlots[i].TYPE)
                {
                    case StatusType.Coding:
                        statusSlots[i].VALUE = playerAction.GetTotalCodingStatus();
                        statusSlots[i].DESCRIPTION = INST_CODING;
                        break;
                    case StatusType.Design:
                        statusSlots[i].VALUE = playerAction.GetTotalDesignStatus();
                        statusSlots[i].DESCRIPTION = INST_DESIGN;
                        break;
                    case StatusType.Testing:
                        statusSlots[i].VALUE = playerAction.GetTotalTestingStatus();
                        statusSlots[i].DESCRIPTION = INST_TESTING;
                        break;
                    case StatusType.Art:
                        statusSlots[i].VALUE = playerAction.GetTotalArtStatus();
                        statusSlots[i].DESCRIPTION = INST_ART;
                        break;
                    case StatusType.Sound:
                        statusSlots[i].VALUE = playerAction.GetTotalSoundStatus();
                        statusSlots[i].DESCRIPTION = INST_SOUND;
                        break;
                    default:
                        break;
                }
                statusSlots[i].Index = i;
            }

            
        }
    }

    private void OnPointExitStatusSlotHandler(BaseStatusSlot statusSlot)
    {
        OnPointExitStatusSlot?.Invoke(statusSlot);
    }

    private void OnPointEnterStatusSlotHandler(BaseStatusSlot statusSlot)
    {
        OnPointEnterStatusSlot?.Invoke(statusSlot);
    }

    private void OnLeftClickStatusSlotHandler(BaseStatusSlot statusSlot, bool selected)
    {
        OnLeftClickStatusSlot?.Invoke(statusSlot, selected);
        lastIndex = statusSlot.Index;
    }
    protected virtual void OnValidate()
    {
        
        if (itemsParent == null)
            itemsParent = transform.GetChild(0).GetComponent<Transform>();
    }
}
