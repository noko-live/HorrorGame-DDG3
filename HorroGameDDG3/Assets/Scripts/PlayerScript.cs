using UnityEngine;
using System.Collections;


public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;

    public GameObject player;

    float tiltOffset;
    public bool playerWalking =false;
    float fowardLeanAmount = 70f;
    Coroutine walkRoutine;
    [SerializeField] float turnDuration = 0.45f;
    [SerializeField] float leanSpeedDuration = 0.25f;
    bool animLock = false;
    [SerializeField] private float speed = 12f;
    AudioSource audioSource;
    public AudioClip FootStepSounds;

    Gamemanager gm;

    public float sideTurnAmount = 20f;
    bool clickLock = false;

    // ran into a problem where the scale of the level is alot smaller than how fast the player is moving
    //i wanted to have value that changes all of that at once;
    [SerializeField] private float mod = 0f;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //turnDuration *= (mod / 2);
        leanSpeedDuration *= mod;
        speed *= mod;

        audioSource = GetComponent<AudioSource>();
        gm = Gamemanager.Instance;
    }


    private void Update()
    {

        if (!clickLock)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerWalking = true;
                Walk();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerWalking = false;
            StopFootsteps();
            //walkRoutine = null;
        }

        if (playerWalking && !clickLock)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }


    }

    [ContextMenu("Test")]
    void Walk()
    {
        if(walkRoutine == null)
        {
            PlayFootstep();
            walkRoutine = StartCoroutine(CameraWalkAnim());
        }
    }


    
    IEnumerator CameraWalkAnim()
    {
        //we're not actually tiltng the camrea, but instead the player: to manipulate the cinemachine to move with us.

        // Tilt camera forward first
        // Rotate camera using lerp with low duration, left then right, about once each
        // Tilt camera back to original view
        // Get original eulerangle and save it so we can revert it
        // 

        Vector3 startingRotation = player.transform.eulerAngles;

        Debug.Log("Coroutine ran");

        //Code to turn camera
        Vector3 startRotation = player.transform.eulerAngles;
        Vector3 targetRotation = startRotation;

        //Debug.Log(startRotation + " , " + targetRotation);


        float duration = leanSpeedDuration;
        float elapsed = 0f;

        targetRotation = new Vector3(fowardLeanAmount, player.transform.eulerAngles.y, player.transform.eulerAngles.z);

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float currentT = Mathf.Clamp01(elapsed / duration);

            Vector3 currentEuler = Vector3.Lerp(startRotation, targetRotation, currentT);
            player.transform.rotation = Quaternion.Euler(currentEuler);

            yield return null;
        }

        Vector3 leanedStart = targetRotation;
        
        while (playerWalking)
        {

            if (!gm.getKeyStatus())
            {
                if (targetRotation.y == -sideTurnAmount)
                {
                    //Means player is facing left, so we change the target rotation to the right
                    leanedStart = targetRotation;
                    targetRotation = new Vector3(player.transform.eulerAngles.x, sideTurnAmount, player.transform.eulerAngles.z);
                }
                else
                {
                    //redo the opposite
                    leanedStart = targetRotation;
                    targetRotation = new Vector3(player.transform.eulerAngles.x, -sideTurnAmount, player.transform.eulerAngles.z);
                }
            }
            else
            {
                if (targetRotation.y == -sideTurnAmount + 180f)
                {
                    //Means player is facing left, so we change the target rotation to the right
                    leanedStart = targetRotation;
                    targetRotation = new Vector3(player.transform.eulerAngles.x, sideTurnAmount + 180f, player.transform.eulerAngles.z);
                }
                else
                {
                    //redo the opposite
                    leanedStart = targetRotation;
                    targetRotation = new Vector3(player.transform.eulerAngles.x, -sideTurnAmount + 180f, player.transform.eulerAngles.z);
                }
            }

            elapsed = 0f;
            duration = turnDuration;


            while (elapsed < duration && playerWalking)
            {
                elapsed += Time.deltaTime;

                float currentT = Mathf.Clamp01(elapsed / duration);

                Vector3 currentEuler = Vector3.Lerp(leanedStart, targetRotation, currentT);
                player.transform.rotation = Quaternion.Euler(currentEuler);

                yield return null;
            }

        }

        duration = leanSpeedDuration;
        elapsed = 0f;

        targetRotation = startingRotation;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float currentT = Mathf.Clamp01(elapsed / duration);

            Vector3 currentEuler = Vector3.Lerp(leanedStart, targetRotation, currentT);
            player.transform.rotation = Quaternion.Euler(currentEuler);

            yield return null;
        }

        yield return new WaitForSeconds(.15f);
        walkRoutine = null;
    }


    public void FlipPlayer()
    {
        StartCoroutine(_FlipPlayer());
    }

    IEnumerator _FlipPlayer()
    {
        Vector3 startingRotation = transform.eulerAngles;

        Debug.Log("Coroutine ran");

        Vector3 startRotation = transform.eulerAngles;
        Vector3 targetRotation = startRotation;



        float duration = .5f;
        float elapsed = 0f;

        targetRotation = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float currentT = Mathf.Clamp01(elapsed / duration);

            Vector3 currentEuler = Vector3.Lerp(startRotation, targetRotation, currentT);
            transform.rotation = Quaternion.Euler(currentEuler);

            yield return null;
        }


        Vector3 finalRotation = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);

        transform.rotation = Quaternion.Euler(finalRotation);

    }

    public void ClearWalkRoutine()
    {
        walkRoutine = null;
    }

    public bool isPlayerWalking()
    {
        return playerWalking;
    }


    void PlayFootstep()
    {
        //audioSource.clip = FootStepSounds;
        audioSource.PlayOneShot(FootStepSounds);
    }
    public void StopFootsteps()
    {
        audioSource.Stop();
    }

    public void setClickLock(bool i)
    {
        clickLock = i;
    }

}
