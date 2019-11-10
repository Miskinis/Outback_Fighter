using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class FlagButton : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement
{
    [NonSerialized] public UnityEvent onClick = new UnityEvent();

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        isOn = true;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        isOn = false;
    }

    protected override void Awake()
    {
        this.ObserveEveryValueChanged(x => interactable).Subscribe(value =>
        {
            if (value)
            {
                m_Group.RegisterFlagButton(this);
            }
            else
            {
                m_Group.UnregisterFlagButton(this);
            }
        });
    }

    /// <summary>
        /// Display settings for when a flagButton is activated or deactivated.
        /// </summary>
        public enum FlagButtonTransition
        {
            /// <summary>
            /// Show / hide the flagButton instantly
            /// </summary>
            None,

            /// <summary>
            /// Fade the flagButton in / out smoothly.
            /// </summary>
            Fade
        }

        [Serializable]
        /// <summary>
        /// UnityEvent callback for when a flagButton is flagButtond.
        /// </summary>
        public class FlagButtonEvent : UnityEvent<bool>
        {}

        /// <summary>
        /// Transition mode for the flagButton.
        /// </summary>
        public FlagButtonTransition flagButtonTransition = FlagButtonTransition.Fade;

        /// <summary>
        /// Graphic the flagButton should be working with.
        /// </summary>
        public Graphic graphic;

        [SerializeField]
        private FlagButtonGroup m_Group;

        /// <summary>
        /// Group the flagButton belongs to.
        /// </summary>
        public FlagButtonGroup group
        {
            get { return m_Group; }
            set
            {
                m_Group = value;
                SetFlagButtonGroup(m_Group, true);
                PlayEffect(true);
            }
        }

        /// <summary>
        /// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
        /// </summary>
        /// <example>
        /// <code>
        /// //Attach this script to a FlagButton GameObject. To do this, go to Create>UI>FlagButton.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     FlagButton m_FlagButton;
        ///     public Text m_Text;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the FlagButton GameObject
        ///         m_FlagButton = GetComponent<FlagButton>();
        ///         //Add listener for when the state of the FlagButton changes, to take action
        ///         m_FlagButton.onValueChanged.AddListener(delegate {
        ///                 FlagButtonValueChanged(m_FlagButton);
        ///             });
        ///
        ///         //Initialise the Text to say the first state of the FlagButton
        ///         m_Text.text = "First Value : " + m_FlagButton.isOn;
        ///     }
        ///
        ///     //Output the new state of the FlagButton into Text
        ///     void FlagButtonValueChanged(FlagButton change)
        ///     {
        ///         m_Text.text =  "New Value : " + m_FlagButton.isOn;
        ///     }
        /// }
        /// </code>
        /// </example>
        public FlagButtonEvent onValueChanged = new FlagButtonEvent();

        // Whether the flagButton is on
        [Tooltip("Is the flagButton currently on or off?")]
        [SerializeField]
        private bool m_IsOn;

        protected FlagButton()
        {}

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

#endif // if UNITY_EDITOR

        public virtual void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                onValueChanged.Invoke(m_IsOn);
#endif
        }

        public virtual void LayoutComplete()
        {}

        public virtual void GraphicUpdateComplete()
        {}

        protected override void OnDestroy()
        {
            if (m_Group != null)
                m_Group.EnsureValidState();
            base.OnDestroy();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetFlagButtonGroup(m_Group, false);
            PlayEffect(true);
        }

        protected override void OnDisable()
        {
            SetFlagButtonGroup(null, false);
            base.OnDisable();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            // Check if isOn has been changed by the animation.
            // Unfortunately there is no way to check if we don�t have a graphic.
            if (graphic != null)
            {
                bool oldValue = !Mathf.Approximately(graphic.canvasRenderer.GetColor().a, 0);
                if (m_IsOn != oldValue)
                {
                    m_IsOn = oldValue;
                    Set(!oldValue);
                }
            }

            base.OnDidApplyAnimationProperties();
        }

        private void SetFlagButtonGroup(FlagButtonGroup newGroup, bool setMemberValue)
        {
            // Sometimes IsActive returns false in OnDisable so don't check for it.
            // Rather remove the flagButton too often than too little.
            if (m_Group != null)
                m_Group.UnregisterFlagButton(this);

            // At runtime the group variable should be set but not when calling this method from OnEnable or OnDisable.
            // That's why we use the setMemberValue parameter.
            if (setMemberValue)
                m_Group = newGroup;

            // Only register to the new group if this FlagButton is active.
            if (newGroup != null && IsActive())
                newGroup.RegisterFlagButton(this);

            // If we are in a new group, and this flagButton is on, notify group.
            // Note: Don't refer to m_Group here as it's not guaranteed to have been set.
            if (newGroup != null && isOn && IsActive())
                newGroup.NotifyFlagButtonOn(this);
        }

        /// <summary>
        /// Whether the flagButton is currently active.
        /// </summary>
        /// <example>
        /// <code>
        /// /Attach this script to a FlagButton GameObject. To do this, go to Create>UI>FlagButton.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     FlagButton m_FlagButton;
        ///     public Text m_Text;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the FlagButton GameObject
        ///         m_FlagButton = GetComponent<FlagButton>();
        ///         //Add listener for when the state of the FlagButton changes, and output the state
        ///         m_FlagButton.onValueChanged.AddListener(delegate {
        ///                 FlagButtonValueChanged(m_FlagButton);
        ///             });
        ///
        ///         //Initialize the Text to say whether the FlagButton is in a positive or negative state
        ///         m_Text.text = "FlagButton is : " + m_FlagButton.isOn;
        ///     }
        ///
        ///     //Output the new state of the FlagButton into Text when the user uses the FlagButton
        ///     void FlagButtonValueChanged(FlagButton change)
        ///     {
        ///         m_Text.text =  "FlagButton is : " + m_FlagButton.isOn;
        ///     }
        /// }
        /// </code>
        /// </example>

        public bool isOn
        {
            get { return m_IsOn; }

            set
            {
                Set(value);
            }
        }

        /// <summary>
        /// Set isOn without invoking onValueChanged callback.
        /// </summary>
        /// <param name="value">New Value for isOn.</param>
        public void SetIsOnWithoutNotify(bool value)
        {
            Set(value, false);
        }

        void Set(bool value, bool sendCallback = true)
        {
            if (m_IsOn == value)
                return;

            // if we are in a group and set to true, do group logic
            m_IsOn = value;
            if (m_Group != null && IsActive())
            {
                if (m_IsOn || (!m_Group.AnyFlagButtonsOn() && !m_Group.allowSwitchOff))
                {
                    m_IsOn = true;
                    m_Group.NotifyFlagButtonOn(this, sendCallback);
                }
            }

            // Always send event when flagButton is clicked, even if value didn't change
            // due to already active flagButton in a flagButton group being clicked.
            // Controls like Dropdown rely on this.
            // It's up to the user to ignore a selection being set to the same value it already was, if desired.
            PlayEffect(flagButtonTransition == FlagButtonTransition.None);
            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("FlagButton.value", this);
                onValueChanged.Invoke(m_IsOn);
            }
        }

        /// <summary>
        /// Play the appropriate effect.
        /// </summary>
        private void PlayEffect(bool instant)
        {
            if (graphic == null)
                return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                graphic.canvasRenderer.SetAlpha(m_IsOn ? 1f : 0f);
            else
#endif
            graphic.CrossFadeAlpha(m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
        }

        /// <summary>
        /// Assume the correct visual state.
        /// </summary>
        protected override void Start()
        {
            PlayEffect(true);
        }

        private void InternalFlagButton()
        {
            if (!IsActive() || !IsInteractable())
                return;

            isOn = !isOn;
        }

        /// <summary>
        /// React to clicks.
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            InternalFlagButton();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            InternalFlagButton();
            
            var localColors = colors;
            localColors.disabledColor = Color.white;
            colors = localColors;
        
            onClick.Invoke();
        }
}