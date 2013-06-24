using UnityEngine;
using System.Collections;

/// <summary>
/// UpDown Button, changes state based on if it is up or down
/// </summary>
[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownButton")]
public class tk2dUIUpDownDisableButton : tk2dUIBaseItemControl
{
    /// <summary>
    /// State that will be active if up and deactivated if down
    /// </summary>
    public GameObject upStateGO;

    /// <summary>
    /// State that will be active if down and deactivated if up
    /// </summary>
    public GameObject downStateGO;
	
    /// <summary>
    /// State that will be active if disabled and deactivated otherwise
    /// </summary>
    public GameObject disabledStateGO;
	
	
    /// <summary>
    /// If true disabled state will show and button will not be clickable
    /// </summary>
    //
	private bool disabled = false;
    public bool Disabled
    {
        get { return disabled; }
        set { 
			disabled = value; 
			uiItem.enabled = !value;
			SetState();
		}
    }
	
    /// <summary>
    /// Use OnRelase instead of OnUp to toggle state
    /// </summary>
    [SerializeField]
    private bool useOnReleaseInsteadOfOnUp = false;

    public bool UseOnReleaseInsteadOfOnUp
    {
        get { return useOnReleaseInsteadOfOnUp; }
    }

    private bool isDown = false;

    void Start()
    {
        SetState();
    }

    void OnEnable()
    {
        if (uiItem)
        {
            uiItem.OnDown += ButtonDown;
            if (useOnReleaseInsteadOfOnUp)
            {
                uiItem.OnRelease += ButtonUp;
            }
            else
            {
                uiItem.OnUp += ButtonUp;
            }
        }
    }

    void OnDisable()
    {
        if (uiItem)
        {
            uiItem.OnDown -= ButtonDown;
            if (useOnReleaseInsteadOfOnUp)
            {
                uiItem.OnRelease -= ButtonUp;
            }
            else
            {
                uiItem.OnUp -= ButtonUp;
            }
        }
    }

    private void ButtonUp()
    {
        isDown = false;
        SetState();
    }

    private void ButtonDown()
    {
        isDown = true;
        SetState();
    }

    private void SetState()
    {
		if (disabled) {
	        ChangeGameObjectActiveStateWithNullCheck(upStateGO, false);
	        ChangeGameObjectActiveStateWithNullCheck(downStateGO, false);
	        ChangeGameObjectActiveStateWithNullCheck(disabledStateGO, true);
		} else {
	        ChangeGameObjectActiveStateWithNullCheck(disabledStateGO, false);
	        ChangeGameObjectActiveStateWithNullCheck(upStateGO, !isDown);
	        ChangeGameObjectActiveStateWithNullCheck(downStateGO, isDown);
		}
    }

    /// <summary>
    /// Internal do not use
    /// </summary>
    public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
    {
        useOnReleaseInsteadOfOnUp = state;
    }
}
