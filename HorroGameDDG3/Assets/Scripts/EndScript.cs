using UnityEngine;

public class EndScript : MonoBehaviour
{

    Gamemanager gm;

    private void Start()
    {
        gm = Gamemanager.Instance;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");
            gm.PlayerEscape();
        }
    }

}
