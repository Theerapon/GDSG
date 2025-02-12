﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Characters_Menu_Handler : MonoBehaviour
{
    #region INSI
    private const string INST_STATUS = "Status";
    private const string INST_BONUS = "Bonus";
    private const string INST_HardSkill = "Hard Skill";
    private const string INST_SoftSkill = "Soft Skill";
    private const int INST_VALUE_ONE = 1;
    private const int INST_VALUE_TEN = 10;
    #endregion

    [Header("Descriptino Display")]
    [SerializeField] protected HardSkills_Display hardSkill_display;
    [SerializeField] protected Softskills_Display softSkill_display;
    [SerializeField] protected Status_Display status_display;
    [SerializeField] protected BaseBonusSlot bonusSlot;

    [Header("Generator")]
    [SerializeField] protected Bonus_Generator bonus_Generator;
    [SerializeField] protected Status_Generator status_Generator;

    protected PlayerAction playerAction;
    protected CharacterStatusController charactersStatusController;
    protected SoftSkillsController softSkillsController;

    #region GameObjects
    [Header("Game Objects")]
    [SerializeField] protected GameObject description_box;
    [SerializeField] protected GameObject title_description;
    [SerializeField] protected GameObject status_description;
    [SerializeField] protected GameObject bonus_description;
    [SerializeField] protected GameObject hardskill_description;
    [SerializeField] protected GameObject softskill_description;
    #endregion

    #region Status Point
    [Header("Status Points")]
    [SerializeField] protected TMP_Text _statusPointsTMP;
    [SerializeField] protected TMP_Text _sofSkillPointsTMP;
    #endregion

    #region Title
    [Header("Title Description")]
    [SerializeField] protected TMP_Text text_title_name;
    [SerializeField] protected TMP_Text text_sub_type;
    #endregion

    #region Status Description
    [Header("Status Description")]
    [SerializeField] protected TMP_Text text_status_value;
    [SerializeField] protected TMP_Text text_status_description;
    [SerializeField] protected Button[] default_valueStatusUpgrade_Button;
    #endregion

    #region Bonus Description
    [Header("Bonus Description")]
    [SerializeField] protected TMP_Text text_bonus_charm;
    [SerializeField] protected TMP_Text text_bonus_base_bootupProject;
    [SerializeField] protected TMP_Text text_bonus_golden_bootupProject;
    [SerializeField] protected TMP_Text text_bonus_base_bootupMotivation;
    [SerializeField] protected TMP_Text text_bonus_golden_bootupMotivation;
    [SerializeField] protected TMP_Text text_bonus_base_energy;
    [SerializeField] protected TMP_Text text_bonus_golden_energy;
    [SerializeField] protected TMP_Text text_bonus_bug_chance;
    [SerializeField] protected TMP_Text text_bonus_time_course;
    [SerializeField] protected TMP_Text text_bonus_time_transport;
    [SerializeField] protected TMP_Text text_bonus_drop_rate;
    #endregion

    #region Hard Skill Description
    [Header("Hard Skills Description")]
    [SerializeField] protected TMP_Text text_hardskill_level;
    [SerializeField] protected TMP_Text text_hardskill_currentExp;
    [SerializeField] protected TMP_Text text_hardskill_requiredExp;
    [SerializeField] protected TMP_Text text_hardskill_description;
    [SerializeField] protected Image image_hardskill_exp;
    #endregion

    #region SOft Skill Description
    [Header("Soft Skills Description")]
    [SerializeField] protected TMP_Text text_softskill_level;
    [SerializeField] protected TMP_Text text_softskill_description;
    #endregion

    #region Field
    private string _currentSoftSkillId;
    private StatusType _currentStatusId;

    private int _cureentSoftSkillUpgrade;
    private int _currentStatusValueUpgrade;
    #endregion

    private void Start()
    {
        softSkillsController = SoftSkillsController.Instance;
        charactersStatusController = CharacterStatusController.Instance;
        playerAction = PlayerAction.Instance;
        _currentSoftSkillId = string.Empty;
        _currentStatusId = StatusType.None;
        _cureentSoftSkillUpgrade = INST_VALUE_ONE;
        _currentStatusValueUpgrade = INST_VALUE_ONE;
        OnButtonClicked(default_valueStatusUpgrade_Button[0]);

        if(!ReferenceEquals(charactersStatusController, null))
        {
            charactersStatusController.OnStatusPointsUpdated.AddListener(OnStatusPointsUpdatedHandler);
            charactersStatusController.OnSoftSkillPointsUpdated.AddListener(OnSoftSkillPointsUpdatedHandler);
        }

        if (!ReferenceEquals(status_display, null))
        {
            //Events status
            status_display.OnLeftClickStatusSlot.AddListener(SelectedStatusDisplayed);
            status_display.OnPointEnterStatusSlot.AddListener(DisplayedStatusDescription);
            status_display.OnPointExitStatusSlot.AddListener(UnDisplayedStatusDescription);
            status_display.OnStatusUpLevel.AddListener(OnStatusUpLevelHandler);
        }

        if (!ReferenceEquals(bonusSlot, null))
        {
            //Events Bonus
            bonusSlot.OnLeftClickBonusSlotEvent.AddListener(SelectedBonusDisplayed);
            bonusSlot.OnPointEnterBonusSlotEvent.AddListener(DisplayedBonusDescription);
            bonusSlot.OnPointExitBonusSlotEvent.AddListener(UnDisplayedBonusDescription);
        }

        if (!ReferenceEquals(hardSkill_display, null))
        {
            //Events Bonus
            hardSkill_display.OnLeftClickHardSkillSlotEvent.AddListener(SelectedHardSkillDisplayed);
            hardSkill_display.OnPointEnterHardSkillSlotEvent.AddListener(DisplayedHardSkillDescription);
            hardSkill_display.OnPointExitHardSkillSlotEvent.AddListener(UnDisplayedHardSkillDescription);
        }

        if(!ReferenceEquals(softSkill_display, null))
        {
            softSkill_display.OnLeftClickSoftSkillSlotEvent.AddListener(SelectedSoftSkillDisplayed);
            softSkill_display.OnPointEnterSoftSkillSlotEvent.AddListener(DisplayedSoftSkillDescription);
            softSkill_display.OnPointExitSoftSkillSlotEvent.AddListener(UnDisplayedSoftSkillDescription);
            softSkill_display.OnSoftSkillUpLevel.AddListener(OnSoftSkillUpLevelHandler);
        }

        Reset();
    }

    private void OnSoftSkillUpLevelHandler(BaseSoftSkillSlot slot)
    {
        DisplayedSoftSkillDescription(slot);
    }

    private void OnStatusUpLevelHandler(BaseStatusSlot slot)
    {
        DisplayedStatusDescription(slot);
    }

    private void Reset()
    {
        DisplayDescriptionBox(false);
        status_description.SetActive(false);
        bonus_description.SetActive(false);
        hardskill_description.SetActive(false);
        softskill_description.SetActive(false);
        SetStatusPoints();
        SetSoftSkillPoints();
    }

    private void OnStatusPointsUpdatedHandler()
    {
        SetStatusPoints();
    }

    private void OnSoftSkillPointsUpdatedHandler()
    {
        SetSoftSkillPoints();
    }

    #region Status
    private void SelectedStatusDisplayed(BaseStatusSlot statusSlot, bool selected)
    {
       SetSelected(statusSlot, selected);
    }
    private void DisplayedStatusDescription(BaseStatusSlot statusSlot)
    {
        Reset();
        //title
        DisplayDescriptionBox(true);
        SetTitleText(statusSlot.TYPE.ToString(), INST_STATUS);

        //description
        status_description.SetActive(true);
        text_status_value.text = statusSlot.VALUE.ToString();
        text_status_description.text = statusSlot.DESCRIPTION;
    }
    private void UnDisplayedStatusDescription(BaseStatusSlot statusSlot)
    {
        DisplayDescriptionBox(false);
        status_description.SetActive(false);
    }


    #endregion
    #region Bonus
    private void SelectedBonusDisplayed(BaseBonusSlot bonusSlot, bool selected)
    {
        SetSelected(bonusSlot, selected);
    }

    private void DisplayedBonusDescription(BaseBonusSlot bonusSlot)
    {
        Reset();
        //title
        DisplayDescriptionBox(true);
        SetTitleText(bonusSlot.TITLE, INST_BONUS);
        
        //description
        bonus_description.SetActive(true);
        text_bonus_charm.text = string.Format("{0:p2}", playerAction.GetTotalBonusCharm());
        text_bonus_base_bootupProject.text = string.Format("{0:p2}", playerAction.GetTotalBonusBootUpProject()); 
        text_bonus_golden_bootupProject.text = string.Format("{0:p2}", playerAction.GetTotalBonusBootUpProjectGoldenTime());
        text_bonus_base_bootupMotivation.text = string.Format("{0:p2}", playerAction.GetTotalBonusBootUpMotivation());
        text_bonus_golden_bootupMotivation.text = string.Format("{0:p2}", playerAction.GetTotalBonusBootUpMotivationGoldenTime());
        text_bonus_base_energy.text = string.Format("{0:p2}", playerAction.GetTotalBonusReduceEnergyConsume());
        text_bonus_golden_energy.text = string.Format("{0:p2}", playerAction.GetTotalBonusReduceEnergyConsumeGoldenTime());
        text_bonus_bug_chance.text = string.Format("{0:p2}", playerAction.GetTotalBonusReduceBugChance());
        text_bonus_time_course.text = string.Format("{0:p2}", playerAction.GetTotalBonusReduceTimeCourse());
        text_bonus_time_transport.text = string.Format("{0:p2}", playerAction.GetTotalBonusReduceTimeTransport());
        text_bonus_drop_rate.text = string.Format("{0:p2}", playerAction.GetTotalBonusIncreaseDropRate());
    }

    private void UnDisplayedBonusDescription(BaseBonusSlot bonusSlot)
    {
        DisplayDescriptionBox(false);
        bonus_description.SetActive(false);
    }
    #endregion
    #region Hard Skills
    private void SelectedHardSkillDisplayed(BaseHardSkillSlot hardSkillSlot, bool selected)
    {
        SetSelected(hardSkillSlot, selected);
    }
    private void DisplayedHardSkillDescription(BaseHardSkillSlot hardSkillSlot)
    {
        Reset();
        //title
        DisplayDescriptionBox(true);
        SetTitleText(hardSkillSlot.HARDSKILL.Hardskill_name, INST_HardSkill);

        hardskill_description.SetActive(true);
        text_hardskill_level.text = hardSkillSlot.HARDSKILL.CurrentLevel.ToString();
        text_hardskill_currentExp.text = hardSkillSlot.HARDSKILL.CurrentExp.ToString();
        text_hardskill_requiredExp.text = hardSkillSlot.HARDSKILL.GetExpRequire().ToString();
        text_hardskill_description.text = hardSkillSlot.HARDSKILL.Hardskill_description;
        image_hardskill_exp.fillAmount = hardSkillSlot.HARDSKILL.GetExpFillAmount();

        status_Generator.CreateTemplate(hardSkillSlot.HARDSKILL);
    }
    private void UnDisplayedHardSkillDescription(BaseHardSkillSlot hardSkillSlot)
    {
        DisplayDescriptionBox(false);
        hardskill_description.SetActive(false);
    }




    #endregion
    #region Soft Skills
    private void SelectedSoftSkillDisplayed(BaseSoftSkillSlot softSkillSlot, bool selected)
    {
        SetSelected(softSkillSlot, selected);
    }

    private void DisplayedSoftSkillDescription(BaseSoftSkillSlot softSkillSlot)
    {
        Reset();
        //title
        DisplayDescriptionBox(true);
        SetTitleText(softSkillSlot.SOFTSKILL.GetSoftSkillName(), INST_SoftSkill);

        softskill_description.SetActive(true);
        text_softskill_level.text = softSkillSlot.SOFTSKILL.GetCurrentSoftSkillLevel().ToString();
        text_softskill_description.text = softSkillSlot.SOFTSKILL.GetSoftSkillDescription().ToString();

        bonus_Generator.CreateTemplate(softSkillSlot.SOFTSKILL);
    }

    private void UnDisplayedSoftSkillDescription(BaseSoftSkillSlot softSkillSlot)
    {
        DisplayDescriptionBox(false);
        softskill_description.SetActive(false);
    }
    #endregion
    protected virtual void OnValidate()
    {
        if (description_box == null)
            description_box = transform.GetChild(3).gameObject;
        
        if (title_description == null)
            title_description = transform.GetChild(3).GetChild(0).gameObject;

        if (status_description == null)
            status_description = transform.GetChild(3).GetChild(1).gameObject;

        if (bonus_description == null)
            bonus_description = transform.GetChild(3).GetChild(2).gameObject;

        if (hardskill_description == null)
            hardskill_description = transform.GetChild(3).GetChild(3).gameObject;

        if (softskill_description == null)
            softskill_description = transform.GetChild(3).GetChild(4).gameObject;

        if (status_display == null)
            status_display = transform.GetChild(0).GetComponent<Status_Display>();

    }

    private void SetStatusPoints()
    {
        _statusPointsTMP.text = charactersStatusController.CurrentStatusPoints.ToString();
    }

    private void SetSoftSkillPoints()
    {
        _sofSkillPointsTMP.text = charactersStatusController.CurrentSoftSkillPoints.ToString();
    }

    private void DisplayDescriptionBox(bool actived)
    {
        description_box.SetActive(actived);
        title_description.SetActive(actived);
    }

    private void SetTitleText(string title, string type)
    {
        text_title_name.text = title;
        text_sub_type.text = type;
    }

    #region Set selected displayed
    private void SetSelected(BaseBonusSlot slot, bool selected)
    {
        if (selected)
        {
            UnDisplayedBonusDescription(slot);
            SetAllSelectedToFalse(false);
            slot.IsSelected(false);
        }
        else
        {
            SetAllSelectedToFalse(true);
            slot.IsSelected(true);
            DisplayedBonusDescription(slot);
        }


    }
    private void SetSelected(BaseHardSkillSlot slot, bool selected)
    {
        if (selected)
        {
            UnDisplayedHardSkillDescription(slot);
            SetAllSelectedToFalse(false);
            slot.IsSelected(false);
        }
        else
        {
            SetAllSelectedToFalse(true);
            slot.IsSelected(true);
            DisplayedHardSkillDescription(slot);
        }
    }
    private void SetSelected(BaseSoftSkillSlot slot, bool selected)
    {
        if (selected)
        {
            UnDisplayedSoftSkillDescription(slot);
            SetAllSelectedToFalse(false);
            slot.IsSelected(false);
        }
        else
        {
            SetAllSelectedToFalse(true);
            slot.IsSelected(true);
            _currentSoftSkillId = slot.SOFTSKILL.SoftSkill_ID;
            DisplayedSoftSkillDescription(slot);
        }
    }
    private void SetSelected(BaseStatusSlot slot, bool selected)
    {
        if (selected)
        {
            UnDisplayedStatusDescription(slot);
            SetAllSelectedToFalse(false);
            slot.IsSelected(false);
        }
        else
        {
            SetAllSelectedToFalse(true);
            slot.IsSelected(true);
            _currentStatusId = slot.TYPE;
            DisplayedStatusDescription(slot);
        }
    }
    #endregion

    private void SetAllSelectedToFalse(bool ottherSelected)
    {
        foreach (BaseHardSkillSlot hardSlot in hardSkill_display.hardSkillSlots)
        {
            hardSlot.IsSelected(false);
            hardSlot.SetOtherSelected(ottherSelected);
        }

        foreach (BaseSoftSkillSlot softSlot in softSkill_display.softSkillSlots)
        {
            softSlot.IsSelected(false);
            softSlot.SetOtherSelected(ottherSelected);
        }

        foreach (BaseStatusSlot statusSlot in status_display.statusSlots)
        {
            statusSlot.IsSelected(false);
            statusSlot.SetOtherSelected(ottherSelected);
        }

        _currentStatusId = StatusType.None;
        _currentSoftSkillId = string.Empty;

        bonusSlot.IsSelected(false);
        bonusSlot.SetOtherSelected(ottherSelected);
    }

    #region Value points
    public void SetValueUpgradeStatusToOne(Button clickedButton)
    {
        _currentStatusValueUpgrade = INST_VALUE_ONE;
        OnButtonClicked(clickedButton);
    }

    public void SetValueUpgradeStatusToTen(Button clickedButton)
    {
        _currentStatusValueUpgrade = INST_VALUE_TEN;
        OnButtonClicked(clickedButton);   
    }

    public void SetAllButtonsInteractable(Button clickedButton)
    {
        foreach (Button button in default_valueStatusUpgrade_Button)
        {
            if (button == clickedButton)
            {
                clickedButton.interactable = false;
            }
            else
            {
                button.interactable = true;
            }

        }

    }

    public void OnButtonClicked(Button clickedButton)
    {
        int buttonIndex = System.Array.IndexOf(default_valueStatusUpgrade_Button, clickedButton);

        if (buttonIndex == -1)
            return;

        SetAllButtonsInteractable(clickedButton);
    }
    #endregion

    public void UpLevelSoftSkill()
    {
        if (charactersStatusController.HasSoftSkillStatusPointEnough(_cureentSoftSkillUpgrade))
        {
            charactersStatusController.UpgradeSoftSkillStatus(_cureentSoftSkillUpgrade);
            softSkillsController.LevelUPSoftSkillById(_currentSoftSkillId);
            
        }
    }

    public void UpLevelCharcterStatus()
    {
        if (charactersStatusController.HasCharacterStatusPointEnough(_currentStatusValueUpgrade))
        {
            charactersStatusController.UpgradeCharacterStatus(_currentStatusId, _currentStatusValueUpgrade);
        }
    }
}
