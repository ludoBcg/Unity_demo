/*********************************************************************************************************************
 *
 * 3_TargetBehavior.cs
 * 
 * Unity_demo
 * Scene 3_playground
 * 
 * Ludovic Blache
 *
 *********************************************************************************************************************/

using UnityEngine;

// Represents the target, i.e., the jack, in game
// Assigned to jack prefab
public class TargetBehavior : MonoBehaviour
{
    private Rigidbody targetRb;

    private float epsilon = 0.001f;


    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // checks if the ball is still moving or not
    public bool Stopped()
    {
        if (Mathf.Abs(-targetRb.velocity.x) < epsilon
            && Mathf.Abs(-targetRb.velocity.y) < epsilon
            && Mathf.Abs(-targetRb.velocity.z) < epsilon)
        {
            return true;
        }
        return false;
    }

    // re-initialize target position
    public void ReInit()
    {
        targetRb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        transform.position = new Vector3(0.0f, 0.187f, 5.0f);
    }

}