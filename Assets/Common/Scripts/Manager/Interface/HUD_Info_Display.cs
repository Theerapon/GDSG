﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD_Info_Display : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private TMP_Text dateCalendar;
    [SerializeField] private TMP_Text timeCalendar;
    [SerializeField] private Image icon;

    [Header("Energy and Motivation")]
    [SerializeField] private Image energy_bar;
    [SerializeField] private Image motivation_bar;
    [SerializeField] private TMP_Text energy;
    [SerializeField] private TMP_Text motivation;

    [Header("Money")]
    [SerializeField] private TMP_Text money;

    [Header("Resourse")]
    [SerializeField] private Sprite image_day;
    [SerializeField] private Sprite image_night;

    private CharacterStatusController characterStatusController;
    private TimeManager timeManager;

    protected void Start()
    {
        timeManager = TimeManager.Instance;
        if (!ReferenceEquals(timeManager, null))
        {
            timeManager.OnDateCalendar.AddListener(HandleOnDateCalendar);
            timeManager.OnTimeCalendar.AddListener(HandleOnTimeCalendar);
            timeManager.OnTimeChange.AddListener(HandlerTimeChange);
            timeManager.NotificationAll();
        }

        characterStatusController = CharacterStatusController.Instance;
        
        if(!ReferenceEquals(characterStatusController, null))
        {
            characterStatusController.OnEnergyUpdated.AddListener(EnergyHandler);
            characterStatusController.OnMotivationUpdated.AddListener(MotivationHandler);
            characterStatusController.OnMoneyUpdated.AddListener(MoneyHandler);
            characterStatusController.ValidateDisplay();
        }

    }

    private void MoneyHandler()
    {
        money.text = characterStatusController.CharacterStatus.CurrentMoney.ToString();
    }

    private void MotivationHandler()
    {
        motivation_bar.fillAmount = CalculateFillAmountMotivation();
        motivation.text = string.Format("{0:n0} / {1:n0}", characterStatusController.CurrentMotivation, characterStatusController.Default_maxMotivation);
    }

    private float CalculateFillAmountMotivation()
    {
        return (float)characterStatusController.CharacterStatus.CurrentMotivation / characterStatusController.CharacterStatus.Default_maxMotivation;
    }

    private void EnergyHandler()
    {
        energy_bar.fillAmount = CalculateFillAmountEnergy();
        energy.text = string.Format("{0:n0} / {1:n0}", characterStatusController.CurrentEnergy, characterStatusController.Default_maxEnergy);
    }

    private float CalculateFillAmountEnergy()
    {
        return (float)characterStatusController.CharacterStatus.CurrentEnergy / characterStatusController.CharacterStatus.Default_maxEnergy;
    }

    private void HandlerTimeChange(bool isDay)
    {
        if (isDay)
        {
            icon.sprite = image_day;
        }
        else
        {
            icon.sprite = image_night;
        }
    }

    private void HandleOnTimeCalendar(string time)
    {
        timeCalendar.text = time.ToUpper();
    }

    private void HandleOnDateCalendar(string date)
    {
        dateCalendar.text = date.ToUpper();
    }

}
