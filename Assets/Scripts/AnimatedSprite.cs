using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    /* This Script is used to Animate Pacman by just changing it's sprite continuosly */
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;
    public float animationTime = 0.125f;
    public int animationFrame { get; private set; }

    public bool loop = true;
    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        /*
            this method is like fixed update because it runs repeatedly 
            it runs the function specified in the first parameter but never
            start running before the time has reached in the second paramter 
            this time is used once to tell when the function start
            then this function repeat it self in the time specified in the 
            third parameter.
         */
        InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
    }

    private void Advance()
    {
        // this condition check if the sprite render component is active if not then it retrun or stop
        if (!this.spriteRenderer.enabled)
        {
            return;
        }

        // increament the animation frame or sprite number that will be animated 
        // I think this is how animation in unity work :) XD
        this.animationFrame++;

        // let the animation frame back to zero if we reach the maximum length of sprites in the sprite list 
        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            this.animationFrame = 0;
        }

        // checking again if the animation Frame within the sprites list range 
        if (this.animationFrame >= 0 && this.animationFrame < this.sprites.Length)
        {
            // changing the property sprite - look in the editor - at the sprite render component 
            this.spriteRenderer.sprite = this.sprites[this.animationFrame];
        }
    }

    public void Restart()
    {
        /*
            I don't know what this function do here yet but it dosen't matter 
            cause it dosen't has a reference in the code .
        */
        this.animationFrame = -1;

        Advance();
    }
}
