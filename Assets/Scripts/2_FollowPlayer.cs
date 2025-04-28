using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // player object to follow
    public GameObject player;

    // offset of camera's position relative to player's position
    [SerializeField] Vector3 offset = new Vector3(0, 6, -8);


    // Update is called once per frame
    // use lateUpdate() instead of Update() to avoid jittering
    void LateUpdate()
    {
        // follow the player's position
        transform.position = player.transform.position + offset;
    }
}
