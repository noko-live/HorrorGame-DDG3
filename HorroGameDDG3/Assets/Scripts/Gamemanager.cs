using UnityEngine;
using System.Collections;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;
    bool hasKey = false;
    [SerializeField] bool enemyWatching = false;
    PlayerScript playerScript;

    [SerializeField] float detectionTimer = 0f;
    float maxDetectionTime = 3f;

    bool isPlayerDead = false;
    public bool gameActive = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        hasKey = false;
        playerScript = PlayerScript.Instance;
        detectionTimer = 0f;


        gameActive = true;
        StartCoroutine(EnemyLookOutTimer());
    }

    public void GiveKey(){hasKey = true;}


    private void Update()
    {
        if (enemyWatching && playerScript.isPlayerWalking())
        {

            detectionTimer += Time.deltaTime * 10;

            if (detectionTimer >= maxDetectionTime)
            {
                PlayerSpotted();
            }
        }
    }

    void PeakABoo()
    {
        enemyWatching = true;
    }

    void CloseEyes()
    {
        enemyWatching = false;

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
        isPlayerDead = true;
        Debug.Log("Player dead.");
    }


}
