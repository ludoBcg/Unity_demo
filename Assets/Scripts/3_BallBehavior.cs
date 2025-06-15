/*********************************************************************************************************************
 *
 * 3_BallBehavior.cs
 * 
 * Unity_demo
 * Scene 3_playground
 * 
 * Ludovic Blache
 *
 *********************************************************************************************************************/


using UnityEngine;

// Represents a ball in game
// Assigned to ball prefab
public class BallBehavior : MonoBehaviour
{
    private Rigidbody ballRb;
    private bool wasShot = false;

    private float epsilon = 0.001f;
    private Vector2 minFieldCoords = new Vector2(-13.5f, -21.0f);
    private Vector2 maxFieldCoords = new Vector2(13.5f, 13.5f);

    private bool isPlayer = true; // flag for player's or opponent's balls


    
    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        // disable gravity at beginning
        ballRb.useGravity = false;
        wasShot = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // consider the ball has been shot when RB has force applied to it
        // do not change wasShot flag in Shoot() to avoid it been checked before animation loop
        if (ballRb.GetAccumulatedForce() != new Vector3(0.0f, 0.0f, 0.0f))
        {
            // cannot be shot anymore
            wasShot = true;
        }

        // Delete balls when thrown out of field
        if(OutOfField())
        {
            PinToWall();
        }
    }

    // to be called by player when he shoots
    public void Shoot(Vector3 _direction, float _shootingForce, float _maxOffset)
    {
        Start();
        if (!wasShot)
        {
            // enable gravity
            ballRb.useGravity = true;

            // add randomness to the shooting dir
            _direction = RandomizeDirection(_direction, _maxOffset);

            // shoot
            ballRb.AddForce(_direction * _shootingForce, ForceMode.Impulse);
        }
    }

    // Checks if the ball is still moving or not
    public bool Stopped()
    {
        if (wasShot)
        {
            if (Mathf.Abs(-ballRb.velocity.x) < epsilon
              && Mathf.Abs(-ballRb.velocity.y) < epsilon
              && Mathf.Abs(-ballRb.velocity.z) < epsilon)
            {
                return true;
            }
        }
        return false;
    }

    public void AssignToPlayer(bool _isPlayer)
    {
        isPlayer = _isPlayer;
    }

    public bool BelongsToPlayer()
    {
        return isPlayer;
    }

    // Detects whenever a ball moves out of the field
    private bool OutOfField()
    {
        if (transform.position.x <= minFieldCoords.x || transform.position.x >= maxFieldCoords.x ||
            transform.position.z <= minFieldCoords.y || transform.position.z >= maxFieldCoords.y)
            return true;

        return false;
    }

    // Prevents balls to move out of the field
    private void PinToWall()
    {
        ballRb.velocity = new Vector3(0.0f, 0.0f, 0.0f);

        transform.position = new Vector3(Mathf.Max(minFieldCoords.x + 0.25f, Mathf.Min(transform.position.x, maxFieldCoords.x - 0.25f)),
                                          0.5f,
                                          Mathf.Max(minFieldCoords.y + 0.25f, Mathf.Min(transform.position.z, maxFieldCoords.y - 0.25f)));
    }


    // Adds randomness to direction vector
    private Vector3 RandomizeDirection(Vector3 _direction, float _radius)
    {
        // build orthobase su, sv, sw, with sw being the shooting direction
        // i.e., su and sv are normalized vectors orthogonal to the direction
        Vector3 sw = _direction;
        Vector3 su;

        if (Mathf.Abs(sw.x) > 0.1f)
            su = new Vector3(0.0f, 1.0f, 0.0f);
        else
            su = new Vector3(1.0f, 0.0f, 0.0f);

        su = Vector3.Cross(su, sw);
        su.Normalize();
  
        Vector3 sv = Vector3.Cross(sw, su);

        // Create random sample direction l towards spherical light source

        // calculates maximum opening angle a_max:
        // sin(a_max) =                    radius / ||_direction||  <-- from trigonometry: sin = opposed (i.e. radius) / adjacent (i.e. ||_direction||) edges length
        //     a_max  =            arcsin( radius / ||_direction|| )
        // cos(a_max) =         sqrt(1 - ( radius / ||_direction|| )^2 )  <-- from: cos(arcsin(x)) = sqrt(1 - x^2)
        //     a_max  = arccos( sqrt(1 - ( radius / ||_direction|| )^2 ) )
        float cos_a_max = Mathf.Sqrt(1.0f - _radius * _radius / Vector3.Dot(_direction, _direction));
        
        // randomly sample opening angle a in [0; a_max]
        float rand_a = UnityEngine.Random.Range(0.0f, 1.0f);
        float cos_a = 1.0f - rand_a + rand_a * cos_a_max;
        float sin_a = Mathf.Sqrt(1.0f - cos_a * cos_a);

        // randomly sample rotation angle phi in [0; 2*PI]
        float rand_phi = UnityEngine.Random.Range(0.0f, 1.0f);
        float phi = 2.0f * Mathf.PI * rand_phi;
        
        // build random vector around direction:
        Vector3 result = su * Mathf.Cos(phi) * sin_a +
                         sv * Mathf.Sin(phi) * sin_a +
                         sw * cos_a;
        result.Normalize();

        return result;
    }
}