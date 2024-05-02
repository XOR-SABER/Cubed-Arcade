using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public AnimationCurve curve;
    public static AudioSource currentTrack;
    private static Dictionary<string, int> _soundMap;

    void Awake() {
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy if another instance exists.
            return;
        }
        instance = this;
        
        DontDestroyOnLoad(gameObject);
        int index = 0;
        _soundMap = new Dictionary<string, int>();
        // Loading.. 
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            Debug.Log(string.Format("Adding {0}, {1} to the map", s.audioName, index));
            _soundMap.Add(s.audioName, index);
            index++;
        }
    }
    private Sound getSound(string name) {
        if(!_soundMap.ContainsKey(name)) return null;
        int index = _soundMap[name];
        return sounds[index];
    }

    public void Play(string name) {
        if (string.IsNullOrEmpty(name)) {
            Debug.LogError("Attempted to play a sound with a null or empty name.");
            return;
        }
        Sound s = getSound(name);
        if (s == null || s.source == null) {
            Debug.Log(string.Format("Sound: {0} not found!", name));
            if(_soundMap.Values.Count == 0) Debug.Log("The _soundmap is empty!");
            foreach(var v in _soundMap) {
                Debug.Log(v.Value);
            }
            Debug.Log(sounds.Length);
            foreach (var item in sounds)
            {
                Debug.Log(item.audioName);
            }
            return;
        }
        Debug.Log(s.audioName);
        if(!s.isMusicTrack) {
            s.source.Play();
            return;
        }
        currentTrack = s.source;
        currentTrack.Play();
    }

    public void fadeOutCurrentTrack (float fadeTime)
	{
        if(currentTrack == null) return;
		StartCoroutine(FadeOut(fadeTime));
	}

	IEnumerator FadeOut (float fadeTime, bool isToFadeIn = false, string nextTrack = "")
	{
		float t = currentTrack.volume;
        float prevVolume = currentTrack.volume;

		while (t > 0f)
		{
			t -= Time.deltaTime / fadeTime;
            currentTrack.volume = curve.Evaluate(t);
			yield return 0;
		}
        currentTrack.Stop();
        currentTrack.volume = prevVolume;
        currentTrack = null;
        if(isToFadeIn) {
            StartCoroutine(FadeInto(nextTrack, fadeTime));
        }
	}

    IEnumerator FadeInto(string name, float fadeTime) {
        float t = 0;
        Play(name);
        float prevVolume = currentTrack.volume;
        currentTrack.volume = 0;
        Debug.Log("Fading in");

		while (t < fadeTime) {
            t += Time.deltaTime;
            float normalizedTime = t / fadeTime;
            currentTrack.volume = Mathf.Clamp(curve.Evaluate(normalizedTime) * prevVolume, 0, prevVolume);
            yield return 0;
        }
        currentTrack.volume = prevVolume;
    }

    public void fadeInNewTrack(string name, float fadeTime) {
        float transitionTime = fadeTime / 2 ;
        if(currentTrack != null) StartCoroutine(FadeOut(transitionTime, true, name));
        else StartCoroutine(FadeInto(name, fadeTime));
    }
}