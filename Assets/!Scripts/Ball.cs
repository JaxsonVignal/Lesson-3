using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gameManager;
    // Reference to SoundManager, found at start because the ball is spawned from a prefab
    private SoundManager soundManager;
    // True once the crash sound has played, so it only plays once per throw
    private bool hasHitPins = false;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        // Find the SoundManager in the scene, the same way GameManager is found
        soundManager = FindAnyObjectByType<SoundManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pin"))
        {
            Debug.Log("Ball hit a pin: " + collision.gameObject.name);

            // First pin this ball has hit
            if (!hasHitPins)
            {
                // Remember the hit so the sound does not repeat for every pin
                hasHitPins = true;
                // Play the pin crash sound
                soundManager.PlaySound("PinHit");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ball rolled through the trigger near the pins
        if (other.CompareTag("CamTrigger"))
        {
            // Ask GameManager to switch to the closeup camera
            gameManager.showCloseup();
        }

        if (other.CompareTag("Pit"))
        {
            gameManager.SetNextThrow();
            Destroy(gameObject);
        }
    }
}