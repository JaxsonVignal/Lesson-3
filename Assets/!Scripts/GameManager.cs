using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Pin[] pins;

    private int throwNumber = 1;

    public void SetNextThrow()
    {
        int fallen = CountFallenPins();
        Debug.Log("Throw " + throwNumber + ": " + fallen + " pins down");
        
                foreach (Pin pin in pins)
                    {
            pin.ResetPin();
                    }
        
                 throwNumber++;
        Debug.Log("Starting throw " + throwNumber);
        playerController.StartThrow();
    }

    public int CountFallenPins()
    {
        int fallenCount = 0;
        foreach (Pin pin in pins)
        {
           if (pin.HasFallen())
            {
                fallenCount++;
            }
        }
        return fallenCount;
    }
 }