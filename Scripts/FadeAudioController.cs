using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAudioController : MonoBehaviour {
	public AudioSource source;
	[Range(0, 1)] public float minVolume = 0;
	[Range(0, 1)] public float maxVolume = 1;
	public float fadeTime;

	public bool max = false;

	void Start() {
		if (source == null) {
			source = GetComponent<AudioSource>();
		}
	}

	void Update() {
		if (source) {
			if (max && source.volume < maxVolume) {
				source.volume += Time.deltaTime / fadeTime;
			}

			if (!max && source.volume > minVolume) {
				source.volume -= Time.deltaTime / fadeTime;
			}
			source.volume = Mathf.Clamp01(source.volume);
		}
	}

	public void SetFadeTimer(float t) {
		max = false;
		Invoke("SetMax", t);
	}

	void SetMax() {
		max = true;
	}
}
