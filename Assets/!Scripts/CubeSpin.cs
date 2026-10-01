using UnityEngine;

public class CubeSpin : MonoBehaviour
{


    private float speed = 1f;


    private bool isRunning = true;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool current = anim.GetBool("Spinning");
            anim.SetBool("Spinning", !current);



            if(speed > 0f)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            if(speed < 0f && isRunning)
            {
                //do stuff
            }

            if (speed < 0f || isRunning)
            {
                //do stuff
            }


        }
    }
}