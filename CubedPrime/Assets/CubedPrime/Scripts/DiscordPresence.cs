using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordPresence : MonoBehaviour
{
    public long applicationID;
    [Space]
    public static string details = "Cubin' around at the speed of sound";
    public static string state = "Snarky funny flavor text!";
    [Space]
    public static string largeImage = "game_logo";
    public static string largeText = "Cubed Prime";
    private long time;    
    public Discord.Discord discord;
    private static bool instanceExists;
    void Awake()  {
        if (!instanceExists) {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1) Destroy(gameObject);
    }
    void Start() {
        // Log in with the Application ID
        discord = new Discord.Discord(applicationID, (System.UInt64)CreateFlags.NoRequireDiscord);
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    void Update()
    {
        // Destroy the GameObject if Discord isn't running
        try {
            discord.RunCallbacks();
        } catch {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        // Clean up Discord presence when the application quits
        if (discord != null)
        {
            // var activityManager = discord.GetActivityManager();
            // activityManager.ClearActivity((res) =>
            // {
            //     if (res != Result.Ok) Debug.LogWarning("Failed to clear Discord activity!");
                
            // });
            discord.Dispose();
        }
    }

    void LateUpdate() 
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        try {
            var activityManager = discord.GetActivityManager();
            var activity = new Activity
            {
                Details = details,
                State = state,
                Assets = 
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
                Timestamps =
                {
                    Start = time
                }
            };

            activityManager.UpdateActivity(activity, (res) => {
                if (res != Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            // If updating the status fails, Destroy the GameObject
            Destroy(gameObject);
        }
    }
}
