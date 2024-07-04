using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;

public class AudioManager : MonoBehaviour
{
    public SoundCollection soundCollection; 
    public static AudioManager instance;
    public AnimationCurve curve;
    public static AudioSource currentTrack;
    public string currentTrackMeta;
    private static Dictionary<string, int> _soundMap;
    public ConcurrentQueue<string> trackQueue;
    public bool isPaused;
    public bool isGameOver;
    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy if another instance exists.
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
        int index = 0;
        _soundMap = new Dictionary<string, int>();
        trackQueue = new ConcurrentQueue<string>();

        // Loading new..
        foreach (SoundObject so in soundCollection.sounds) {
            var s = so.soundInstance;
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            if (s.addIntoQueue) trackQueue.Enqueue(s.audioName);
            // Debug.Log(string.Format("Adding {0}, {1} to the map", s.audioName, index));
            _soundMap.Add(s.audioName, index);
            index++;
        }

        CollectionExtensions.Shuffle(trackQueue);
    }
    void Update()
    {
        if (PlayerStats.instance != null && currentTrack != null)
        {
            PlayerStats.instance.currentlyPlaying = currentTrackMeta;
        }
        if (currentTrack != null && !currentTrack.isPlaying && !isPaused && !isGameOver)
        {
            if (currentTrack.loop) return;
            currentTrack = null;
            if (trackQueue.Count > 0)
            {
                string nextTrackName;
                if (trackQueue.TryDequeue(out nextTrackName))
                {
                    trackQueue.Enqueue(nextTrackName);
                    Play(nextTrackName);
                }
            }
        }
    }

    // This method is called when the application is paused or resumed
    private void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
    }
    private Sound getSound(string name)
    {
        if (!_soundMap.ContainsKey(name)) return null;
        int index = _soundMap[name];
        return soundCollection.sounds[index].soundInstance;
    }

    public void Play(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Attempted to play a sound with a null or empty name.");
            return;
        }
        // Debug.Log(name);
        Sound s = getSound(name);
        // Debug.Log(s.audioName);
        if (!s.isMusicTrack)
        {
            s.source.Play();
            return;
        }
        currentTrack = s.source;
        currentTrack.Play();
        currentTrackMeta = s.meta;
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.currentlyPlaying = s.meta;
        }
    }

    public void PlayOnShot(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Attempted to play a sound with a null or empty name.");
            return;
        }
        Sound s = getSound(name);
        s.source.PlayOneShot(s.source.clip);
        return;

    }

    public void fadeOutCurrentTrack(float fadeTime)
    {
        if (currentTrack == null) return;
        StartCoroutine(FadeOut(fadeTime));
    }

    IEnumerator FadeOut(float fadeTime, bool isToFadeIn = false, string nextTrack = "")
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
        if (isToFadeIn)
        {
            StartCoroutine(FadeInto(nextTrack, fadeTime));
        }
    }

    IEnumerator FadeInto(string name, float fadeTime)
    {
        float t = 0;
        Play(name);
        float prevVolume = currentTrack.volume;
        currentTrack.volume = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float normalizedTime = t / fadeTime;
            currentTrack.volume = Mathf.Clamp(curve.Evaluate(normalizedTime) * prevVolume, 0, prevVolume);
            yield return 0;
        }
        currentTrack.volume = prevVolume;
    }

    public void fadeInNewTrack(string name, float fadeTime)
    {
        float transitionTime = fadeTime / 2;
        if (currentTrack != null) StartCoroutine(FadeOut(transitionTime, true, name));
        else StartCoroutine(FadeInto(name, fadeTime));
    }

    public bool isPlaying(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Attempted to play a sound with a null or empty name.");
            return false;
        }
        Sound s = getSound(name);
        if (s.source.isPlaying) return true;
        else return false;
    }
}

public static class CollectionExtensions
{
    // Θ(N) Bleh.. its only done once. anyway
    public static void Shuffle<T>(this ConcurrentQueue<T> queue)
    {
        // Create a temporary list to store elements
        List<T> tempList = new List<T>();

        // Move all elements from the queue to the list
        while (queue.TryDequeue(out T item))
        {
            tempList.Add(item);
        }
        System.Random random = new System.Random();
        for (int i = tempList.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            T temp = tempList[i];
            tempList[i] = tempList[j];
            tempList[j] = temp;
        }
        queue.Clear();
        foreach (T item in tempList)
        {
            queue.Enqueue(item);
        }
    }
}