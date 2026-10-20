using UnityEngine;
using TMPro;

public class FrameUI : MonoBehaviour


{   
    //vars 
    [SerializeField] private TMP_Text frameNumberText;

    //vars we used today 

    //vars to display the int values for each throw
    [SerializeField] private TMP_Text throw1Text;
    [SerializeField] private TMP_Text throw2Text;
    [SerializeField] private TMP_Text throw3Text;
    [SerializeField] private TMP_Text totalText;


    //Writes one throw's score into its box; public so UIManager can call it
    public void UpdateScore(int throwNumber, int score)
    {
        //if checks to see what throw we are currently on
        if (throwNumber == 1)
        {
            //access the throw 1 text FOR THIS FRAME and set its value to the value of the score int sent to a string so we can read in the UI 
            throw1Text.text = score.ToString();
        }
        else if (throwNumber == 2)
        {
            throw2Text.text = score.ToString();
        }
        else if (throwNumber == 3)
        {
            throw3Text.text = score.ToString();
        }
    }

    //writes the total score each frame to the total text tmp 
    public void UpdateTotal(int total)
    {
        //access total text and update with parsed score value 
        totalText.text = total.ToString();
    }

    public void SetFrameNumber(int frameNumber)
    {
        frameNumberText.text = frameNumber.ToString();
        throw1Text.text = "";
        throw2Text.text = "";
        totalText.text = "";

        if (throw3Text != null)
        {
            throw3Text.text = "";
        }
    }
}