using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonitorAnimController : MonoBehaviour {

	public delegate void AnimFinished();
	public event AnimFinished OnShowFinished;
    public event AnimFinished OnHideFinished;

    public bool IsVisible {
        get; 
        private set;
    } = false;

    public bool IsAnimating {
        get; 
        private set;
    } = false;

    void OnShowAnimEnd() {
        IsVisible = true;
        IsAnimating = false;
        OnShowFinished?.Invoke();
    }

    void OnHideAnimEnd() {
        IsVisible = false;
        IsAnimating = false;
        OnHideFinished?.Invoke();
    }

    Animator anim;
    void Awake() {
        anim = GetComponent<Animator>();
    }

    //------------------------------------

    public void ShowUIMonitor() {
        if (!IsVisible) {
            anim.SetBool("show", true);
            IsAnimating = true;
        }
    }

    public void HideUIMonitor() {
        if (IsVisible) {
            anim.SetBool("show", false);
            IsAnimating = true;
        }
    }
}
