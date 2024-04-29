using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public AnimationCurve curve;
    private Dictionary<string, int> soundMap = new Dictionary<string, int>(); 
    private AudioSource currentTrack;

    void Awake() {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
                return;
        }
        DontDestroyOnLoad(gameObject);
        int index = 0;
        // Loading.. 
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            Debug.Log(string.Format("Adding {0}, {1} to the map", s.audioName, index));
            soundMap.Add(s.audioName, index);
            index++;
        }
    }

    private Sound getSound(string name) {
        if(!soundMap.ContainsKey(name)) return null;
        int index = soundMap[name];
        return sounds[index];
    }

    public void Play(string name) {
        Sound s = getSound(name);
        if (s == null) {
            Debug.Log(string.Format("Sound: {0} not found!", name));
            return;
        }
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
            Debug.Log(currentTrack.volume);	
			yield return 0;
		}
        currentTrack.Stop();
        Debug.Log("Fade done");
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
            Debug.Log(currentTrack.volume);
            yield return null;
        }
        Debug.Log("Fading complete");
        currentTrack.volume = prevVolume;
        Debug.Log(currentTrack.volume);
    }

    public void fadeInNewTrack(string name, float fadeTime) {
        float transitionTime = fadeTime / 2 ;
        if(currentTrack != null) StartCoroutine(FadeOut(transitionTime, true, name));
        else StartCoroutine(FadeInto(name, fadeTime));
    }
}
