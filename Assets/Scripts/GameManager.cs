using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* This Script is used to Control Game scenes loosing winning starting and dying stuff */
    public Ghost[] ghost;
    public Pacman pacman;
    public Transform pellets;
    public int ghostMultiplier { get; private set; } = 1;
    public int Score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        // starting new game by setting the initial condition score and lives ... etc
        NewGame();
    }

    private void Update()
    {
        // getting any input from to start a new game if the player loose all his lives 
        if (lives <= 0 && Input.anyKeyDown)
        {
            // starting new game by setting the initial condition score and lives ... etc
            NewGame();
        }
    }

    private void NewGame()
    {
        /*
            this function reset the state of the player by set his score to zero 
            and his lives to 3 and then invoke the new round method which take care 
            of showing the pellets at their position and then set the player and 
            ghost position to the initial position 
        */

        // set the score to zero 
        SetScore(0);
        // set the lives to 3
        SetLives(3);
        // starting new round 
        NewRound();
    }

    private void NewRound()
    {
        /*
            this function show again all pellets that has been eaten
            and then reset the state of the ghost and the player or pacman 
        */

        // show all pellets
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        // reset the state of the ghost and the player
        ResetState();
    }

    private void ResetState()
    {
        /*
            this function loop through all ghost and set their postion the the 
            initial position and then show them or make them active
            also set pacman to it's initial position and then show him or make him active
        */
        // ResetGhostMultiplier();

        for (int i = 0; i < this.ghost.Length; i++)
        {
            this.ghost[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void GameOver()
    {
        /* this function make all ghost and pacman unactive */
        for (int i = 0; i < this.ghost.Length; i++)
        {
            this.ghost[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        /* 
            this function is used synoymously to set in the declarition but 
            we need to put more code on it so we created it
         */
        this.Score = score;
    }

    private void SetLives(int lives)
    {
        /* 
            this function is used synoymously to set in the declarition but 
            we need to put more code on it so we created it
         */
        this.lives = lives;
    }

    public void GhostEaten(Ghost ghost)
    {
        /* 
            this function add points to the score when the ghost is eated 
            the score points depend on the variable at ghost script
         */
        int points = ghost.points * ghostMultiplier;
        SetScore(this.Score + points);
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        /* 
            this function set pacman unactive when he get eated by the ghosts
         */
        
        this.pacman.DeathSequence();

        // decrementing the live by one every time pacman die
        SetLives(this.lives - 1);

        // check if the player not died completely
        if (this.lives > 0)
        {

            // this function works like coroutine but in much simpler way :)
            Invoke(nameof(ResetState), 3.0f);
        }
        else // then if he die compeletely we call game over
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(this.Score + pellet.points);

        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);

            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        // TODO: Changing Ghost State
        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);

    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
