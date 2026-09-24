using UnityEngine;
using System.Collections;


public class PlayerScript : MonoBehaviour
{
    GameObject player;

    float tiltOffset;
    bool playerWalking =false;


    void Awake()
    {
        player = this.gameObject;
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
        bool isTurning = true;



        //player.transform.eulerAngles.x;

        while (playerWalking)
        {
            //WHILE player is turning to a side we run a loop that tilts the player to the side then back to the center y.
            //then we redo it with the other side.
            //we keep the cycle going until the second coroutine says the player's movement has stopped.

        }




        yield return new WaitForSeconds(0f);
    }

}
