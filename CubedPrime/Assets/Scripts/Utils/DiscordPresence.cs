using UnityEngine;

// This code only works on desktop builds.
#if UNITY_STANDALONE
using Discord;
#endif

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
    private static DiscordPresence instance;
    #if UNITY_STANDALONE
    public Discord.Discord discord;
    #endif
    
    void Awake()  {
        if (instance == null) instance = this;
        else {
            Debug.Log("Discord presence already exists");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #if UNITY_STANDALONE
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
    #endif
}
