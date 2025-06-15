/*********************************************************************************************************************
 *
 * 3_FollowTarget.cs
 * 
 * Unity_demo
 * Scene 3_playground
 * 
 * Ludovic Blache
 *
 *********************************************************************************************************************/

using UnityEngine;

// Secondary camera control for target tracking
public class FollowTarget : MonoBehaviour
{
    // target object to follow
    public GameObject target;

    // offset of camera's position relative to target's position
    [SerializeField] Vector3 offset = new Vector3(0.0f, 8.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // LateUpdate is called after Update() has finished
    void LateUpdate()
    {
        // follow the target's position
        transform.position = target.transform.position + offset;
    }
}
