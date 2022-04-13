using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    /* This is Script Control "GameObject where this script is attached"
        movement and changing its direction 
    */

    // Speed of The "GameObject where this script is attached" 
    public float speed = 8.0f;

    // Speed increasing variable that make movement more faster
    public float speedMultiplier = 1.0f;

    // The initial direction of The "GameObject where this script is attached"
    public Vector2 initialDirection;

    // The Walls Layer which have been marked to `Obstacle` Layer
    // in project setting we define `Obstacle` Layer to not make collision with it self
    // or with pellet and nodes
    public LayerMask obstacleLayer;

    // The rigid body component of "GameObject where this script is attached"
    public new Rigidbody2D rigidbody { get; private set; }

    // The variable that store direction input for "GameObject where this script is attached" movement
    public Vector2 direction { get; private set; }

    // The variable that store direction input for "GameObject where this script is attached" Next movement
    public Vector2 nextDirection { get; private set; }

    // The initial position for "GameObject where this script is attached"
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        /* Here We assign the value of GameObject's rigidbody -where this script is attached-
            and starting position for it 
        */
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startingPosition = this.transform.position;
    }

    private void Start()
    {
        // Let the speed, direction, next direction and position back to their initial state
        // and then activate "GameObject where this script is attached"
        ResetState();
    }

    public void ResetState()
    {
        /* 
            This function set things back to their initial state to when starting new round
            it include every thing that has a direct or indirect relation to movement of the 
            "GameObject where this script is attached"
         */
        this.speedMultiplier = 1.0f;    // 1.0f means there is no speed increasing
        this.direction = this.initialDirection; // starting direction >> eg. right for pacman, up for ghost
        this.nextDirection = Vector2.zero;  // next direction is set back to zero
        this.transform.position = this.startingPosition; // starting position
        this.rigidbody.isKinematic = false; // let the Gameobject pass through walls without collision set to false
        this.enabled = true; // turn movement script back on
    }

    private void Update()
    {
        /* Here We just change the direction. movement instruction occurs at fixed update function */

        // make sure that the GameObject can move at that direction first or store the value
        if (this.nextDirection != Vector2.zero)
        {
            // changing the direction to next direction
            SetDirection(this.nextDirection);
        }
    }

    private void FixedUpdate()
    {
        // the relative postion of the GameObject
        Vector2 position = this.transform.position;

        // translation variable which hold direction and speed of where we want to move 
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;

        // This method is used to move the player postion by changing it's position
        this.rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        /* 
            this function assign the direction value for Game object where he want to move 
            .. eg see pacman scripts where this function is used
         */

        // this condition runs if the direction is not blocked by wall and checking that using the function Occupied
        if (forced || !Occupied(direction))
        {
            // when the street is open to change direction
            // let our direction equals the direction argument get it from user later set next direction to zero
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        }
        else
        {
            // when the street is closed so we can't change direction
            // let the next direction to be our inputed direction since the game object is not occupied
            this.nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        /* this function get the direction from the user and then check if it's blocked or not
            if it's blocked then it will return true 
            if it's not blocked by wall it will return false
         */

        // using the box cast to accurately check for the space between walls so pacman dosen't turn in the walls
        // or to prevent him from getting into the wall by turning on early
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);

        // check if the collider (obstacle layer) is being hitted if yes then it will return true or it will be null 
        // means no collision happen with (obstacle layer) so it well return false
        return hit.collider != null;
    }
}
