using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Pin[] pins;
    [SerializeField] private float settleDelay = 2f;

    public void SetNextThrow()
    {
        //invoke allows us to call to a method with a delay
        Invoke(nameof(ProcessThrow), settleDelay);
    }

    private void ProcessThrow()
    {
        int fallen = CountFallenPins();
        scoreManager.SetFrameScore(fallen);
        playerController.StartThrow();
    }


    //method for counting fallen pins each time a throw has been done 
    public int CountFallenPins()
    {
        int fallenCount = 0;

        //iterating though the pins array and checking if each has fallen over and adding 1 to fallen count for each that has
        foreach (Pin pin in pins)
        {
            if (pin.gameObject.activeSelf && pin.HasFallen())
            {
                fallenCount++;

                //we set pin to inactive so its not in the way for our nxt shot
                pin.gameObject.SetActive(false);
            }
        }
        return fallenCount;
    }


    //method used to reset our pins when the game requires 
    public void ResetAllPins()
    {
        //loops through array of pins and resets each pin 
        foreach (Pin pin in pins)
        {
            pin.ResetPin();
        }
    }
}