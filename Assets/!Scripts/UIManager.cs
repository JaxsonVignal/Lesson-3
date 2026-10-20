using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    //vars 

    //parent object of all 10 frames every frame is accessed through this variable
    [SerializeField] private Transform framesBar;


    [SerializeField] private GameObject strikeMessage;
    [SerializeField] private GameObject spareMessage;
    [SerializeField] private float messageDuration = 1.5f;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text finalScoreText;


    private void Start()
    {
        //prepping our scene for new game of bowling 
        ResetAllFrames();
        strikeMessage.SetActive(false);
        spareMessage.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    //method to reset the point values for each from before the game starts 
    public void ResetAllFrames()
    {
        //a loop to iterate through the frames under framesBar and set the values back to default 
        for (int i = 0; i < framesBar.childCount; i++)
        {
            FrameUI frame = framesBar.GetChild(i).GetComponent<FrameUI>();
            frame.SetFrameNumber(i + 1);
        }
    }

    //method to choose current frame and set the values for they frame based on what throw we are currently on 
    public void SetFrameValue(int frameIndex, int throwNumber, int score)
    {
        //accessing the frame game object at index frameIndex then updating its value 
        FrameUI frame = framesBar.GetChild(frameIndex).GetComponent<FrameUI>();
        frame.UpdateScore(throwNumber, score);
    }

    //same as above but for the final score for each frame 
    public void SetFrameTotal(int frameIndex, int total)
    {
        FrameUI frame = framesBar.GetChild(frameIndex).GetComponent<FrameUI>();
        frame.UpdateTotal(total);
    }


    //shows strike text then disables text again after 1.5 seconds by using invoke with messageDuration as the time value 
    public void ShowStrike()
    {
        //setting the game object defined as strikeMessage to true
        strikeMessage.SetActive(true);
        //preform HideMessage after 1.5 seconds (messageDuration)
        Invoke(nameof(HideMessages), messageDuration);
    }

    public void ShowSpare()
    {
        spareMessage.SetActive(true);
        Invoke(nameof(HideMessages), messageDuration);
    }

    //method to hide strike and spare text again 
    private void HideMessages()
    {
        strikeMessage.SetActive(false);
        spareMessage.SetActive(false);
    }



    public void ShowGameOver(int finalScore)
    {
        finalScoreText.text = finalScore.ToString();
        gameOverScreen.SetActive(true);
    }
}