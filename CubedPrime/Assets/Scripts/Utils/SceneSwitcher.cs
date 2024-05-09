using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts
{
    public class SceneSwitcher : MonoBehaviour
    {
        public Image img;
        public AnimationCurve curve;
        public LevelDetails[] levels;
        public static SceneSwitcher instance;
        public int levelToGoto;

        public Level levelToLoad;

        void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (instance == null) instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            StartCoroutine(FadeIn());
        }

        public void FadeTo(string sceneToFadeInto)
        { 
            StartCoroutine(FadeOut(sceneToFadeInto));
        }

        private IEnumerator FadeIn()
        {
            var t = 1f;
            while (t > 0)
            {
                t -= Time.deltaTime;
                var color = img.color;
                var a = curve.Evaluate(t);
                color.a = a;
                img.color = color;
                yield return 0;
            }
        }
    
        private IEnumerator FadeOut(string scene)
        {
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime;
                var color = img.color;
                var a = curve.Evaluate(t);
                color.a = a;
                img.color = color;
                yield return 0;
            }

            SceneManager.LoadScene(scene);
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //img = FindObjectOfType<FadeImage>().fadeIMG;
            StartCoroutine(FadeIn());
        }
    }
}