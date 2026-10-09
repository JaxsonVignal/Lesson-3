using Unity.Burst.Intrinsics;
using UnityEngine;

public class ScoreManager1 : MonoBehaviour
{

    //var used to store our scores for each frame 
    private int[] frameScores = new int[10];

    //vars used to allow our systems backend to know what frame and what throw we are on 
    private int currentFrame = 0;
    private int currentThrow = 1;

    

    //method to set the score for each frame int fallenPins is passed into the method in order for us to prefroms calculations
    public void SetFrameScore(int fallenPins)
    {
        //adds fallen pins to the current total of the current frame
        frameScores[currentFrame] += fallenPins;
        Debug.Log("Frame " + (currentFrame + 1) + ", throw " + currentThrow + ": " + fallenPins + " pins");

        //checks if the player is on the first or second throw for the game if on 1st we iterate current throw and stay in the same frame if not (currentThrow = 2) we go to next frame
        if (currentThrow == 1)
        {
            currentThrow++;
        }
        else
        {
            NextFrame();
        }
    }

    //simple method that iterates currentFrame(increase by 1) and resets currentThrow to 1 so we are ready to use the variablse again in the next frame
    private void NextFrame()
    {
        currentFrame++;
        currentThrow = 1;
    }
}

