using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowPlayer : MonoBehaviour
{
    // player object to follow
    public GameObject player;

    // offset of camera's position relative to player's position
    [SerializeField] Vector3 offsetCamPos = new Vector3(0, 6, -8);
    // offset of camera's LookAt position relative to player's position
    [SerializeField] Vector3 offsetCamlookAt = new Vector3(0, 2, 0);


    // Update is called once per frame
    // use lateUpdate() instead of Update() to avoid jittering
    void LateUpdate()
    {
        // follow the player's position
        // (cam position is player position + offsetCamPos in player's space)
        transform.position = player.transform.position + player.transform.TransformDirection(offsetCamPos);

        // follow the player's forward direction
        // (cam position LookAt point is player position + offsetCamlookAt in player's space)
        transform.LookAt(player.transform.position + player.transform.TransformDirection(offsetCamlookAt));
    }
}
