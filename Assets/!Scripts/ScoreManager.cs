using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    //create a var for our UI manager so this script can access its methods 
    [SerializeField] private UIManager uiManager;

    private int[] frameScores = new int[10];
    private int currentFrame = 0;
    private int currentThrow = 1;
    private int totalScore = 0;

    private bool hasSpareBonus = false;
    private int strikeBonusThrows = 0;

    public void SetFrameScore(int fallenPins)
    {

        //pass the required values into our UI Manager script to prefrom the functions with these values 
        uiManager.SetFrameValue(currentFrame, currentThrow, fallenPins);

        if (hasSpareBonus)
        {
            frameScores[currentFrame - 1] += fallenPins;
            hasSpareBonus = false;
        }

        if (strikeBonusThrows > 0)
        {
            frameScores[currentFrame - 1] += fallenPins;
            strikeBonusThrows--;
        }

        frameScores[currentFrame] += fallenPins;
        Debug.Log("Frame " + (currentFrame + 1) + ", throw " + currentThrow + ": " + fallenPins + " pins");

        if (currentFrame == 9)
        {
            //use the method to score last frame rather then the first 9 frames 
            ScoreLastFrame(fallenPins);
            //return so that we do not use any other functions from this method 
            return;
        }

        if (currentThrow == 1)
        {
            if (fallenPins == 10)
            {
                Debug.Log("Strike!");
                uiManager.ShowStrike();
                strikeBonusThrows = 2;
                NextFrame();
            }
            else
            {
                currentThrow++;
            }
        }
        else
        {
            if (frameScores[currentFrame] == 10)
            {
                Debug.Log("Spare!");
                uiManager.ShowSpare();
                hasSpareBonus = true;
            }

            NextFrame();
        }
    }

    //method to score final frame only 
    private void ScoreLastFrame(int fallenPins)
    {

        //check if on throw 1 
        if (currentThrow == 1)
        {
            //check if throw 1 = strike
            if (fallenPins == 10)
            {
                Debug.Log("Strike!");
                uiManager.ShowStrike();
                gameManager.ResetAllPins();
            }

            currentThrow++;
        }

        //check if throw 2
        else if (currentThrow == 2)
        {
            //take total score of frame and subtract value from second throw 
            int firstThrow = frameScores[currentFrame] - fallenPins;

            //if first throw = 10 we got strike 
            if (firstThrow == 10)
            {
                //now we check value of second throw on its own to see if we got strike
                if (fallenPins == 10)
                {
                    Debug.Log("Strike!");
                    uiManager.ShowStrike();
                    gameManager.ResetAllPins();
                }

                currentThrow++;
            }
            //if our first trow does not = 10 they we have to check if the value for our first 2 throws = 10 if so we got a spare and should display the spare message 
            else if (frameScores[currentFrame] == 10)
            {
                Debug.Log("Spare!");
                uiManager.ShowSpare();
                gameManager.ResetAllPins();
                currentThrow++;
            }
            //if we dint hit spare we go next frame 
            else
            {
                NextFrame();
            }
        }

        //checkls if we are on last throw (throw 3)
        else
        {
            //checks if we got strike on throw 3 
            if (fallenPins == 10)
            {
                Debug.Log("Strike!");
                uiManager.ShowStrike();
            }
            //goes to next frame and ends the game 
            NextFrame();
        }
    }

    private void NextFrame()
    {
        gameManager.ResetAllPins();
        Debug.Log("Frame " + (currentFrame + 1) + " done. Total so far: " + CalculateTotalScore());

        currentFrame++;
        currentThrow = 1;
    }

    public int CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in frameScores)
        {
            totalScore += score;
        }
        return totalScore;
    }

    public int GetCurrentFrame()
    {
        return currentFrame;
    }

    public int[] GetFrameTotals()
    {
        int[] totals = new int[frameScores.Length];
        int runningTotal = 0;
        for (int i = 0; i < frameScores.Length; i++)
        {
            runningTotal += frameScores[i];
            totals[i] = runningTotal;
        }
        return totals;
    }
}