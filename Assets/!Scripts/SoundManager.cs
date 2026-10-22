// Gives access to Unity's core types like MonoBehaviour, AudioSource and AudioClip
using UnityEngine;

// Plays every sound effect in the game, picked by name
public class SoundManager : MonoBehaviour
{
    // Speaker for ball sounds
    [SerializeField] private AudioSource ballSource;
    // Speaker for pin sounds
    [SerializeField] private AudioSource pinSource;
    // Speaker for UI sounds
    [SerializeField] private AudioSource uiSource;

    // Sound when the ball is thrown
    [SerializeField] private AudioClip throwClip;
    // Sound when the pins are reset
    [SerializeField] private AudioClip resetClip;
    // Sound when the game ends
    [SerializeField] private AudioClip gameOverClip;

    // Sound when the ball hits the pins
    [SerializeField] private AudioClip pinHitClip;

    // Plays the sound with this name; public so other scripts can call it
    public void PlaySound(string soundName)
    {
        // Pick a case based on the name that was passed in
        switch (soundName)
        {
            // The ball was thrown
            case "Throw":
                // Play the throw clip once on the ball speaker
                ballSource.PlayOneShot(throwClip);
                // Stop here; skip the other cases
                break;
            // The pins are being reset
            case "Reset":
                // Play the reset clip once on the pin speaker
                pinSource.PlayOneShot(resetClip);
                // Stop here; skip the other cases
                break;
            // The game is over
            case "GameOver":
                // Play the game over clip once on the UI speaker
                uiSource.PlayOneShot(gameOverClip);
                // Stop here; skip the other cases
                break;

            // The ball hit the pins
            case "PinHit":
                // Play the pin hit clip once on the pin speaker
                pinSource.PlayOneShot(pinHitClip);
                // Stop here; skip the other cases
                break;


            // The name matched none of the cases above
            default:
                // Warn in the Console so a typo in the name is easy to find
                Debug.LogWarning("No sound called " + soundName);
                // Stop here
                break;
        }
    }
}