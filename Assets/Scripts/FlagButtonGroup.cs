using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FlagButtonGroup : UIBehaviour
{
    [SerializeField] private bool m_AllowSwitchOff = false;

    /// <summary>
    /// Is it allowed that no flagButton is switched on?
    /// </summary>
    /// <remarks>
    /// If this setting is enabled, pressing the flagButton that is currently switched on will switch it off, so that no flagButton is switched on. If this setting is disabled, pressing the flagButton that is currently switched on will not change its state.
    /// Note that even if allowSwitchOff is false, the FlagButton Group will not enforce its constraint right away if no flagButtons in the group are switched on when the scene is loaded or when the group is instantiated. It will only prevent the user from switching a flagButton off.
    /// </remarks>
    public bool allowSwitchOff
    {
        get { return m_AllowSwitchOff; }
        set { m_AllowSwitchOff = value; }
    }

    private List<FlagButton> m_FlagButtons = new List<FlagButton>();

    public void UpdateSelectablesNavigation()
    {
        var count = m_FlagButtons.Count;
        if (count < 1) return;
        
        if (m_FlagButtons[0] == GetComponentInChildren<FlagButton>())
        {
            m_FlagButtons.Reverse();
        }
        
        for (var i = 0; i < count; i++)
        {
            var flagButton = m_FlagButtons[i];
            var navigation = new Navigation {mode = Navigation.Mode.Explicit};

            if(flagButton.interactable == false) continue;
            
            if (i == 0)
            {
                navigation.selectOnLeft = m_FlagButtons[m_FlagButtons.Count - 1];
            }
            else
            {
                navigation.selectOnLeft = m_FlagButtons[i - 1];
            }

            if (i + 1 >= m_FlagButtons.Count)
            {
                navigation.selectOnRight = m_FlagButtons[0];
            }
            else if(i + 1 < m_FlagButtons.Count)
            {
                navigation.selectOnRight = m_FlagButtons[i + 1];
            }
            flagButton.navigation = navigation;
        }
    }

    protected FlagButtonGroup()
    {
    }

    /// <summary>
    /// Because all the FlagButtons have registered themselves in the OnEnabled, Start should check to
    /// make sure at least one FlagButton is active in groups that do not AllowSwitchOff
    /// </summary>
    protected override void Start()
    {
        EnsureValidState();
        base.Start();
    }

    private void ValidateFlagButtonIsInGroup(FlagButton flagButton)
    {
        if (flagButton == null || !m_FlagButtons.Contains(flagButton))
            throw new ArgumentException(string.Format("FlagButton {0} is not part of FlagButtonGroup {1}", new object[] {flagButton, this}));
    }

    /// <summary>
    /// Notify the group that the given flagButton is enabled.
    /// </summary>
    /// <param name="flagButton">The flagButton that got triggered on.</param>
    /// <param name="sendCallback">If other flagButtons should send onValueChanged.</param>
    public void NotifyFlagButtonOn(FlagButton flagButton, bool sendCallback = true)
    {
        ValidateFlagButtonIsInGroup(flagButton);
        // disable all flagButtons in the group
        for (var i = 0; i < m_FlagButtons.Count; i++)
        {
            if (m_FlagButtons[i] == flagButton)
                continue;

            if (sendCallback)
                m_FlagButtons[i].isOn = false;
            else
                m_FlagButtons[i].SetIsOnWithoutNotify(false);
        }
    }

    /// <summary>
    /// Unregister a flagButton from the group.
    /// </summary>
    /// <param name="flagButton">The flagButton to remove.</param>
    public void UnregisterFlagButton(FlagButton flagButton)
    {
        if (m_FlagButtons.Contains(flagButton))
        {
            m_FlagButtons.Remove(flagButton);
            UpdateSelectablesNavigation();
        }
    }

    /// <summary>
    /// Register a flagButton with the flagButton group so it is watched for changes and notified if another flagButton in the group changes.
    /// </summary>
    /// <param name="flagButton">The flagButton to register with the group.</param>
    public void RegisterFlagButton(FlagButton flagButton)
    {
        if (!m_FlagButtons.Contains(flagButton))
        {
            m_FlagButtons.Add(flagButton);
            UpdateSelectablesNavigation();
        }
    }

    /// <summary>
    /// Ensure that the flagButton group still has a valid state. This is only relevant when a FlagButtonGroup is Started
    /// or a FlagButton has been deleted from the group.
    /// </summary>
    public void EnsureValidState()
    {
        if (!allowSwitchOff && !AnyFlagButtonsOn() && m_FlagButtons.Count != 0)
        {
            m_FlagButtons[0].isOn = true;
            NotifyFlagButtonOn(m_FlagButtons[0]);
        }
    }

    /// <summary>
    /// Are any of the flagButtons on?
    /// </summary>
    /// <returns>Are and of the flagButtons on?</returns>
    public bool AnyFlagButtonsOn()
    {
        return m_FlagButtons.Find(x => x.isOn) != null;
    }

    /// <summary>
    /// Returns the flagButtons in this group that are active.
    /// </summary>
    /// <returns>The active flagButtons in the group.</returns>
    /// <remarks>
    /// FlagButtons belonging to this group but are not active either because their GameObject is inactive or because the FlagButton component is disabled, are not returned as part of the list.
    /// </remarks>
    public IEnumerable<FlagButton> ActiveFlagButtons()
    {
        return m_FlagButtons.Where(x => x.isOn);
    }

    /// <summary>
    /// Switch all flagButtons off.
    /// </summary>
    /// <remarks>
    /// This method can be used to switch all flagButtons off, regardless of whether the allowSwitchOff property is enabled or not.
    /// </remarks>
    public void SetAllFlagButtonsOff(bool sendCallback = true)
    {
        bool oldAllowSwitchOff = m_AllowSwitchOff;
        m_AllowSwitchOff = true;

        if (sendCallback)
        {
            for (var i = 0; i < m_FlagButtons.Count; i++)
                m_FlagButtons[i].isOn = false;
        }
        else
        {
            for (var i = 0; i < m_FlagButtons.Count; i++)
                m_FlagButtons[i].SetIsOnWithoutNotify(false);
        }

        m_AllowSwitchOff = oldAllowSwitchOff;
    }
}