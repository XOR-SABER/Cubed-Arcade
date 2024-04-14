
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    private bool isTextUpdated = false; 
    [SerializeField]
    private TypingText bottomText;
    [SerializeField]
    public static string status = "Dunno yet..";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTextUpdated) {
            isTextUpdated = true;
            bottomText.startType(status);
            bottomText.startType("Texting the backlog buffer");
        }
    }
}
