using UnityEngine;
using System.Collections;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;
    bool hasKey = false;
    [SerializeField] bool enemyWatching = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        hasKey = false;

        StartCoroutine(EnemyLookOutTimer());
    }

    public void GiveKey(){hasKey = true;}


    void PeakABoo()
    {
        enemyWatching = true;
        // Eye/Watching logic
    }


    IEnumerator EnemyLookOutTimer()
    {
        //Cooldown
        float randomTime = Random.Range(3f,8f);
        yield return new WaitForSeconds(randomTime);
        Debug.Log(" Cooldown for " + randomTime + " seconds.");

        PeakABoo();

        randomTime = Random.Range(2f,8f);
        yield return new WaitForSeconds(randomTime);
        Debug.Log("Eyes open for " + randomTime + " seconds.");

        enemyWatching = false;
        Debug.Log("Eyes closed");
    }

    public void PlayerSpotted()
    {
        Debug.Log("Player dead.");
    }


}
