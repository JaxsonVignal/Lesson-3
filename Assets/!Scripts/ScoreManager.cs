using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;


    // var used to store our point vaule for each frame 
    private int[] frameScores = new int[10];

    // vars used to allow our system to track current frame and throw
    private int currentFrame = 0;
    private int currentThrow = 1;

    //vars used to track our score bonuses bool for spare because it only ever effects 1 throw and int for strike because it adds bonus from next two throws
    private int totalScore = 0;
    private bool hasSpareBonus = false;
    private int strikeBonusThrows = 0;


    //method to set the score for each frame int fallenPins is passed into the method to preform the calculations each frame
    public void SetFrameScore(int fallenPins)
    {
        //checking if spare bonus is true 
        if (hasSpareBonus)
        {
            //adds the fallen pins from this frame to the PREVIOUS frame score giving the spare bonus then reset spare bonus to false
            frameScores[currentFrame - 1] += fallenPins;
            hasSpareBonus = false;
        }

        //checks if player has strike bonus active 
        if (strikeBonusThrows > 0)
        {
            //adds the fallen pins from the frame to last frame 
            frameScores[currentFrame - 1] += fallenPins;
            //subtract 1 from strikeBonusThrows 
            strikeBonusThrows--;
        }

        //adds the fallen pins count from this throw to the total score for the current frame 
        frameScores[currentFrame] += fallenPins;
        Debug.Log("Frame " + (currentFrame + 1) + ", throw " + currentThrow + ": " + fallenPins + " pins");

        //checks if the player is on the first or second throw for the game if on 1st we iterate current throw and stay in the same frame if not (currentThrow = 2) we go to next frame
        if (currentThrow == 1)
        {
            //checking if the player got a strike on throw 1 by checking if all 10 pins have been knocked over 
            if (fallenPins == 10)
            {
                Debug.Log("Strike!");
                //sets the stike bonus to 2 because it needs to be applied for the next 2 throws
                strikeBonusThrows = 2;
                //because we got a strike we go straight to next frame
                NextFrame();
            }
            else
            {
                currentThrow++;
            }
        }
        else
        {
            //checking if the value of our first 2 throws are = to 10 meaning we got a spare 
            if (frameScores[currentFrame] == 10)
            {
                Debug.Log("Spare!");
                //set spare bonus to true 
                hasSpareBonus = true;
            }

            NextFrame();
        }
    }

    //simple method that iterates currentFrame (increase by 1) and resets currentThrow to 1 so we are ready to us the variablse again in the next frame
    private void NextFrame()
    {
        gameManager.ResetAllPins();
        Debug.Log("Frame " + (currentFrame + 1) + " done. Total so far: " + CalculateTotalScore());

        currentFrame++;
        currentThrow = 1;

        if (currentFrame >= 10)
        {
            Debug.Log("Game over! Final score: " + CalculateTotalScore());
            ResetScore();
        }
    }


    //method we use to calculate total score after each frame 
    public int CalculateTotalScore()
    {
        //we set totalScore to 0 so that when we calcualate score we are stating from teh same number every time
        totalScore = 0;

        //we use a for each loop to move through the entire array 
        foreach (int score in frameScores)
        {
            //for each score in the array we add the value to the current total
            totalScore += score;
        }
        //return the final value out of the method
        return totalScore;
    }

    public void ResetScore()
    {
        currentFrame = 0;
        currentThrow = 1;
        totalScore = 0;
        hasSpareBonus = false;
        strikeBonusThrows = 0;

        for (int i = 0; i < frameScores.Length; i++)
        {
            frameScores[i] = 0;
        }
    }
}