using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
        if(!s.isMusicTrack) return;
        currentTrack = s.source;
        currentTrack.Play();
    }

    public void fadeOutCurrentTrack (float fadeTime)
	{
        if(currentTrack == null) return;
		StartCoroutine(FadeOut(fadeTime));
	}

	IEnumerator FadeOut (float fadeTime)
	{
		float t = currentTrack.volume;

		while (t > 0f)
		{
			t -= Time.deltaTime / fadeTime;
            currentTrack.volume = curve.Evaluate(t);
            Debug.Log(currentTrack.volume);	
			yield return 0;
		}
	}
}
