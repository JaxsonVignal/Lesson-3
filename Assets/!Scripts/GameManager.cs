using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ScoreManager scoreManager;

    //add ref 
    [SerializeField] private UIManager uiManager;


    [SerializeField] private Pin[] pins;
    [SerializeField] private float settleDelay = 2f;

    public void SetNextThrow()
    {
        Invoke(nameof(ProcessThrow), settleDelay);
    }

    private void ProcessThrow()
    {
        int fallen = CountFallenPins();

        //call to the score manager and set the frame score based on the values of fallen pins this frame
        scoreManager.SetFrameScore(fallen);

        //tracks the current frame we are in 
        int currentFrame = scoreManager.GetCurrentFrame();
        //array that stores the total poiont values for each frame 
        int[] frameTotals = scoreManager.GetFrameTotals();

        //loops through the array up to the currrent frame and sets the totals based on our score values determined in our score manager script 
        for (int i = 0; i < currentFrame; i++)
        {
            uiManager.SetFrameTotal(i, frameTotals[i]);
        }


        //once we reach the last frame of the game we want to show the game over screen with our final values and return out of the method so we dont start another throw. 
        if (currentFrame >= frameTotals.Length)
        {
            uiManager.ShowGameOver(scoreManager.CalculateTotalScore());
            return;
        }

        playerController.StartThrow();
    }

    public int CountFallenPins()
    {
        int fallenCount = 0;
        foreach (Pin pin in pins)
        {
            if (pin.gameObject.activeSelf && pin.HasFallen())
            {
                fallenCount++;
                pin.gameObject.SetActive(false);
            }
        }
        return fallenCount;
    }

    public void ResetAllPins()
    {
        foreach (Pin pin in pins)
        {
            pin.ResetPin();
        }
    }
}