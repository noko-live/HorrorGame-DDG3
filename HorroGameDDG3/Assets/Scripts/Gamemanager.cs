using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;
    bool hasKey = false;
    [SerializeField] bool enemyWatching = false;
    
    PlayerScript playerScript;
    GameObject player;
    public GameObject keyObject;
    public GameObject MonsterFace;

    [SerializeField] float detectionTimer = 0f;
    float maxDetectionTime = 12f;

    bool isPlayerDead = false;
    public bool gameActive = false;

    public AudioSource audioSource;
    public List<AudioClip> GiggleClips;
    public List<AudioClip> PeekClips;
    public AudioClip keySound;
    public AudioClip runningWoodSound;
    public AudioClip doorOpenClose;

    public TMP_Text textUI;
    public GameObject panelUI;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = PlayerScript.Instance.gameObject;
        hasKey = false;
        playerScript = PlayerScript.Instance;
        detectionTimer = 0f;
        textUI.gameObject.SetActive(false);
        panelUI.gameObject.SetActive(false);
        gameActive = true;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(EnemyLookOutTimer());
    }

    public void GiveKey(){hasKey = true;}
    public bool getKeyStatus() { return hasKey; }


    private void Update()
    {
        if (enemyWatching && playerScript.isPlayerWalking())
        {

            detectionTimer += Time.deltaTime * 10;

            if (detectionTimer >= maxDetectionTime)
            {
                if (isPlayerDead == false)
                {
                    PlayerSpotted();
                }
            }
        }
    }

    void PeakABoo()
    {
        enemyWatching = true;
        PlayPeekSound();

    }

    void CloseEyes()
    {
        enemyWatching = false;
        PlayGiggle();
    }

    IEnumerator EnemyLookOutTimer()
    {
        while (gameActive)
        {
            //Cooldown
            float randomTime = Random.Range(3f, 8f);
            yield return new WaitForSeconds(randomTime);
            Debug.Log(" Cooldown for " + randomTime + " seconds.");

            PeakABoo();

            randomTime = Random.Range(2f, 8f);
            yield return new WaitForSeconds(randomTime);
            Debug.Log("Eyes open for " + randomTime + " seconds.");

            CloseEyes();
            Debug.Log("Eyes closed");
            detectionTimer = 0f;
        }
    }

    public void PlayerSpotted()
    {
        Debug.Log("Player dead.");
        StartCoroutine(_PlayerSpotted());
    }

    IEnumerator _PlayerSpotted()
    {
        isPlayerDead = true;
        MonsterFace.SetActive(false);


        playerScript.setClickLock(true);
        playerScript.StopFootsteps();
        playerScript.playerWalking = false;

        PlayRunSound();

        yield return new WaitForSeconds(3.2f);


        textUI.gameObject.SetActive(true);
        panelUI.gameObject.SetActive(true);

        textUI.text = "im sorry";
        Time.timeScale = 0;

        yield return new WaitForSeconds(0f);
    }


    public void PlayerEscape()
    {
        playerScript.setClickLock(true);
        playerScript.StopFootsteps();
        playerScript.playerWalking = false;

        textUI.gameObject.SetActive(true);
        panelUI.gameObject.SetActive(true);

        PlayDoorSound();

        textUI.text = "freedom.";
        Time.timeScale = 0; 

    }

    void PlayDoorSound()
    {
        audioSource.PlayOneShot(doorOpenClose);
    }
    void PlayGiggle()
    {
        audioSource.PlayOneShot(GiggleClips[Random.Range(0, GiggleClips.Count)]);
    }

    void PlayPeekSound()
    {
        audioSource.PlayOneShot(PeekClips[Random.Range(0, PeekClips.Count)]);
    }
    void PlayRunSound()
    {
        audioSource.PlayOneShot(runningWoodSound);
    }

    public void CollectKeyRoutine()
    {
        StartCoroutine(_CollectKeyRoutine());
    }

    IEnumerator _CollectKeyRoutine()
    {
        playerScript.setClickLock(true);
        playerScript.StopFootsteps();
        playerScript.playerWalking = false;
        playerScript.ClearWalkRoutine();

        yield return new WaitForSeconds(1f);
        keyObject.SetActive(false);
        GiveKey();
        audioSource.PlayOneShot(keySound);



        yield return new WaitForSeconds(2f);
        playerScript.FlipPlayer();
        yield return new WaitForSeconds(2f);

        playerScript.setClickLock(false);

        yield return new WaitForSeconds(0f);

    }



}
