using UnityEngine;
using AnimationOrTween;
using System.Collections.Generic;

/// <summary>
/// tab label color
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UITabLabelColor")]
public class UITabLabelColor : UIWidgetContainer
{
	/// <summary>
	/// List of all the active labels currently in the scene.
	/// </summary>

	static public BetterList<UITabLabelColor> list = new BetterList<UITabLabelColor>();

	/// <summary>
	/// Current tablabel that sent a state change notification.
	/// </summary>

	static public UITabLabelColor current;

	/// <summary>
	/// If set to anything other than '0', all active toggles in this group will behave as radio buttons.
	/// </summary>

	public int group = 0;

	/// <summary>
	/// Label that's visible when the 'isActive' status is 'true'.
	/// </summary>

	public UILabel activeSprite;

	/// <summary>
	/// label active color
	/// </summary>

	public Color activeColor;

	/// <summary>
	/// label normal color
	/// </summary>

	public Color normalColor;

	/// <summary>
	/// Whether the toggle starts checked.
	/// </summary>

	public bool startsActive = false;

	/// <summary>
	/// Callbacks triggered when the label's state changes.
	/// </summary>

	public List<EventDelegate> onChange = new List<EventDelegate>();

	public delegate bool Validate(bool choice);

	/// <summary>
	/// Want to validate the choice before committing the changes? Set this delegate.
	/// </summary>

	public Validate validator;

	/// <summary>
	/// Deprecated functionality. Use the 'group' option instead.
	/// </summary>

	[HideInInspector]
	[SerializeField]
	GameObject eventReceiver;
	[HideInInspector]
	[SerializeField]
	string functionName = "OnActivate";
	[HideInInspector]
	[SerializeField]
	bool startsChecked = false; // Use 'startsActive' instead

	bool mIsActive = true;
	bool mStarted = false;

	/// <summary>
	/// Whether the toggle is checked.
	/// </summary>

	public bool value
	{
		get
		{
			return mStarted ? mIsActive : startsActive;
		}
		set
		{
			if (!mStarted) startsActive = value;
			else if (group == 0 || value || !mStarted) Set(value);
		}
	}

	[System.Obsolete("Use 'value' instead")]
	public bool isChecked { get { return value; } set { this.value = value; } }

	/// <summary>
	/// Return the first active toggle within the specified group.
	/// </summary>

	static public UITabLabelColor GetActiveToggleLabel(int group)
	{
		for (int i = 0; i < list.size; ++i)
		{
			UITabLabelColor toggle = list[i];
			if (toggle != null && toggle.group == group && toggle.mIsActive)
				return toggle;
		}
		return null;
	}

	void OnEnable() { list.Add(this); }
	void OnDisable() { list.Remove(this); }

	/// <summary>
	/// Activate the initial state.
	/// </summary>

	void Start()
	{
		if (startsChecked)
		{
			startsChecked = false;
			startsActive = true;
#if UNITY_EDITOR
			NGUITools.SetDirty(this);
#endif
		}

		// Auto-upgrade
		if (!Application.isPlaying)
		{
			if (Application.isPlaying && activeSprite != null)
				activeSprite.color = startsActive ? activeColor : normalColor;

			if (EventDelegate.IsValid(onChange))
			{
				eventReceiver = null;
				functionName = null;
			}
		}
		else
		{
			mIsActive = !startsActive;
			mStarted = true;
			Set(startsActive);
		}
	}

	/// <summary>
	/// Check or uncheck on click.
	/// </summary>

	void OnClick() { if (enabled) value = !value; }

	/// <summary>
	/// Fade out or fade in the active sprite and notify the OnChange event listener.
	/// </summary>

	public void Set(bool state)
	{
		if (validator != null && !validator(state)) return;

		if (!mStarted)
		{
			mIsActive = state;
			startsActive = state;
			if (activeSprite != null) activeSprite.color = state ? activeColor : normalColor;
		}
		else if (mIsActive != state)
		{
			// Uncheck all other toggles
			if (group != 0 && state)
			{
				for (int i = 0, imax = list.size; i < imax; )
				{
					UITabLabelColor cb = list[i];
					if (cb != this && cb.group == group) cb.Set(false);

					if (list.size != imax)
					{
						imax = list.size;
						i = 0;
					}
					else ++i;
				}
			}

			// Remember the state
			mIsActive = state;

			// Tween the color of the active sprite
			if (activeSprite != null)
			{
				if (!NGUITools.GetActive(this))
				{
					activeSprite.color = mIsActive ? activeColor : normalColor;
				}
				else
				{
					TweenColor.Begin(activeSprite.gameObject, 0.15f, mIsActive ? activeColor : normalColor);
				}
			}

			if (current == null)
			{
				UITabLabelColor tog = current;
				current = this;

				if (EventDelegate.IsValid(onChange))
				{
					EventDelegate.Execute(onChange);
				}
				else if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
				{
					// Legacy functionality support (for backwards compatibility)
					eventReceiver.SendMessage(functionName, mIsActive, SendMessageOptions.DontRequireReceiver);
				}
				current = tog;
			}
		}
	}
}
