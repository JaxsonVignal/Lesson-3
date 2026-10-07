using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //test

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private Rigidbody ballRb;

    [SerializeField] float rangeX = .5f;

    [SerializeField] private Transform arrowVisual;
    [SerializeField] private Animator animator;

    private Vector3 ballOffset;

    private bool wasBallThrown = false;

    private bool isAiming = false;

    [SerializeField]private GameObject[] ballPrefabs;
    [SerializeField] private Transform spawnPos;

    private GameObject currentBall;

    private void Start()
    {
        spawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (wasBallThrown) 
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                spawnBall();
            }

            return; 
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAiming)
            {
                startAiming();
            }

            else
            {
                throwBall();
                return;
            }
        }

        if (!isAiming)
        {
            moveArrow();
        }

        followArrow();
    }

    void moveArrow()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -rangeX, rangeX);
        transform.position = pos;
    }

    void followArrow()
    {
        currentBall.transform.position = transform.position + ballOffset;
    }

    void startAiming()
    {
        isAiming = true;
        animator.SetBool("Aiming", true);
    }

    void throwBall()
    {
        wasBallThrown = true;
        isAiming = false;

        Vector3 throwDirection = arrowVisual.up;

        throwDirection.y = 0f;
        throwDirection.Normalize();

        animator.SetBool("Aiming", false);

        ballRb.isKinematic = false;

        ballRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }

    void spawnBall()
    {
        if(currentBall != null)
        {
            Destroy(currentBall);
        }

        int index = Random.Range(0, ballPrefabs.Length);

        currentBall = Instantiate(ballPrefabs[index], spawnPos.position, Quaternion.identity);

        ballRb = currentBall.GetComponent<Rigidbody>();
        ballRb.isKinematic = true;

        ballOffset = currentBall.transform.position - transform.position;

        wasBallThrown = false;
    }

    public void StartThrow()
   {
        spawnBall();
   }
}
