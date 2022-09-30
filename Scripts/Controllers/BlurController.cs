using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurController : MonoBehaviour {

    [Range(1, 20)] public float maxBlurSize = 5;
	public float blurSpeed = 0.03f;
	public float unblurSpeed = 0.1f;
    Image blur;
    bool blured = false;

    public bool IsMaxBlur {
        get {
            return blur.material.GetFloat("_Size") >= maxBlurSize;
        }
    }

    public bool IsMinBlur {
        get {
            return blur.material.GetFloat("_Size") <= 0;
        }
    }


	void Awake() {
        blur = GetComponent<Image>();
        blur.material.SetFloat("_Size", 0f);
	}

	void Update() {
        float blurSize = blur.material.GetFloat("_Size");

        if (blured && blurSize < maxBlurSize) {    
            SetBlurAmount(blurSize + blurSpeed);
            return;
        }
        if (!blured && blurSize > 0) {    
            SetBlurAmount(blurSize - unblurSpeed);
            return;
        }
	}

    void SetBlurAmount(float amount) {
        float blurSize = Mathf.Clamp(amount, 0, maxBlurSize);
        blur.material.SetFloat("_Size", blurSize);
    }

    public void SetBluredState(bool value) {
        blured = value;
    }
}
