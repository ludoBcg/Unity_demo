/*********************************************************************************************************************
 *
 * 3_CameraController.cs
 * 
 * Unity_demo
 * Scene 3_playground
 * 
 * Ludovic Blache
 *
 *********************************************************************************************************************/

using UnityEngine;

// Main camera control for shooting direction
public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100.0f;     // Rotation speed for A/D/W/S keys in degrees per second

    public GameObject player;
    private Player playerScript;
    private Vector3 viewDir = Vector3.zero;

    private Vector3 initPos = new Vector3(0.0f, 10.0f, -15.11f );
    private Quaternion initRot;

    private bool hasGameStarted = false; // flag if game has started or not

    // Start is called before the first frame update
    void Start()
    {
        viewDir = Vector3.forward;

        initPos = new Vector3(0.0f, 10.0f, -15.11f);
        initRot = Quaternion.Euler(30.0f, 0.0f, 0.0f);

        playerScript = player.GetComponent<Player>();

        hasGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasGameStarted)
        {
            // Rotation with A/D keys
            float rotHorizontal = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            //transform.Rotate(0, rotHorizontal, 0, Space.World); // Rotate around the global Y axis
            transform.RotateAround(playerScript.playerPos, Vector3.up, rotHorizontal);

            // Rotation with W/S keys
            float rotVertical = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
            //transform.Rotate(-rotVertical, 0, 0, Space.Self); // Rotate around the global X axis
            if ((rotVertical < 0.0f && transform.rotation.eulerAngles.x > 80.0f && transform.rotation.eulerAngles.x < 278.0f) ||
                 (rotVertical > 0.0f && transform.rotation.eulerAngles.x > 82.0f && transform.rotation.eulerAngles.x < 280.0f))
            {
                rotVertical = 0.0f;
            }

            transform.RotateAround(playerScript.playerPos, -transform.right, rotVertical);

            playerScript.RotateArrow(rotHorizontal, rotVertical);

            // send view direction to player, to be used as shooting direction
            viewDir = transform.forward;
            playerScript.SetShootingDir(viewDir);
        }
    }

    // re-initialize camera orientation
    public void ReInit()
    {
        transform.position = initPos;
        transform.rotation = initRot;
        playerScript.ReInitArrow();
    }

    public void GamePaused(bool _hasStarted)
    {
        hasGameStarted = _hasStarted;
    }

}
