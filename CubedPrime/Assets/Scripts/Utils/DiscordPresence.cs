using UnityEngine;
using Discord;

public class DiscordPresence : MonoBehaviour
{
    public long applicationID;
    [Space]
    // Because of the static you can update them anywhere..
    [SerializeField] 
    public static string details = "Main Menu";
    [SerializeField] 
    // Cubin' around at the speed of sound
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
        else if (FindObjectsOfType(GetType()).Length > 1) {
            Debug.Log("Discord presence already exists");
            Destroy(gameObject);
        }
    }
    void Start() {
        // Log in with the Application ID
        discord = new Discord.Discord(applicationID, (System.UInt64)CreateFlags.NoRequireDiscord);
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    void Update() {
        // Destroy the GameObject if Discord isn't running
        try {
            discord.RunCallbacks();
        } catch {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit() {
        // Get rid of the discord OBJ. 
        if (discord != null) discord.Dispose();
    }

    void LateUpdate() {
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
