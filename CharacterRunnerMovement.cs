using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunnerMovement : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent beforeDead_MyEvent;
    public UnityEngine.Events.UnityEvent afterDead_MyEvent;
    private Rigidbody2D rb;
    public Animator animator;
    public Animator animatorClone;
    public float speed;
    public float jumpForce;
    private float moveInput;
    public AudioSource audioCharacter;
    public AudioClip Run;
    public AudioClip Jump;
    public AudioClip Crouch;
    public AudioClip Dead;

    private bool isGrounded;
    private bool isCrouched;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float jumpTimeCounter;
    public float jumpTime;
    private float crouchTimeCounter;
    public float crouchTime;
    private bool isJumping;
    private bool canFall;
    private bool hasEntered;
    public ParticleSystem crouchDust;
    public ParticleSystem jumpDust;
    private float counterTime;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

/*
    public void SavePlayer ()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        
        SaveData data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
        
    }
    */

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed,rb.velocity.y);
        animator.SetFloat("inAir", rb.velocity.y);
        animator.SetBool("inGround", isGrounded);
        animator.SetBool("isCrouch", isCrouched);
        animatorClone.SetFloat("inAir", rb.velocity.y);
        animatorClone.SetBool("inGround", isGrounded);
        animatorClone.SetBool("isCrouch", isCrouched);   
    }
        void Update()
    {   
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);


        if (isGrounded == true && Input.GetButtonDown("Crouch") && hasEntered == false && isJumping==false)
        {
            audioCharacter.clip=Crouch;
            audioCharacter.loop = false;
            audioCharacter.Play();
           isCrouched = true;
           crouchTimeCounter = crouchTime;
        }else if (Input.GetButtonDown("Crouch") && isGrounded == false){ 
            rb.velocity += Vector2.down * Mathf.Abs(-10);   
            if(canFall == true  && rb.velocity.y > 0)
            {
            canFall= false;
            }
            isCrouched = true;
            crouchTimeCounter = crouchTime;
        }
		if (Input.GetButton("Crouch") && isCrouched == true && hasEntered == false)
		{   
            if(isGrounded == true){
            if(!crouchDust.isPlaying)
            {
                if(audioCharacter.clip != Crouch){
                audioCharacter.clip=Crouch;
                audioCharacter.loop = false;
                audioCharacter.Play();    
                }
                crouchDust.Play();
            }


			if(crouchTimeCounter >0){
				
				crouchTimeCounter -= Time.deltaTime;
			}
			else {
                audioCharacter.clip=Run;
                audioCharacter.loop = true;
                audioCharacter.Play();
               if(crouchDust.isPlaying) crouchDust.Stop();
				isCrouched = false;
			}
        }
		}

		
		if (Input.GetButtonUp("Crouch") && hasEntered == false )
		{
            audioCharacter.clip=Run;
            audioCharacter.loop = true;
            audioCharacter.Play();
            if(crouchDust.isPlaying) crouchDust.Stop();
			isCrouched = false;
           
		}

        if (isGrounded == true && Input.GetButtonDown("Jump") && hasEntered == false)
		{
            audioCharacter.clip=Jump;
            audioCharacter.loop = false;
            audioCharacter.Play();
            if(!jumpDust.isPlaying) jumpDust.Play();
            if(crouchDust.isPlaying) crouchDust.Stop();           
            isJumping = true;
            canFall = false;
            
			// Add a vertical force to the player.
            crouchTimeCounter = crouchTime;
			jumpTimeCounter = jumpTime;
			rb.velocity = Vector2.up * jumpForce;


		}
		if (Input.GetButton("Jump") && isJumping == true && hasEntered == false)
		{   
            //if (Input.GetButton("Crouch"))jumpTimeCounter = 0;
			if(jumpTimeCounter >0){
                               
				rb.velocity = Vector2.up * jumpForce;
				jumpTimeCounter -= Time.deltaTime;
			}
			else {
                if(jumpDust.isPlaying) jumpDust.Stop();
				isJumping = false;
                audioCharacter.clip=Run;
                audioCharacter.loop = true;
                audioCharacter.Play();
			}
		}
		if (Input.GetButtonUp("Jump") && hasEntered == false)
		{
            audioCharacter.clip=Run;
            audioCharacter.loop = true;
            audioCharacter.Play();
            if(jumpDust.isPlaying) jumpDust.Stop();
			isJumping = false;
            canFall = true;
 
		}
	}

    private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == ("Enemies") && !hasEntered) {
            audioCharacter.clip=Dead;
            audioCharacter.loop = false;
            audioCharacter.Play();
            hasEntered = true;
            isCrouched = false;
            //rb.velocity = new Vector2(0,rb.velocity.y);
            animator.SetBool("isDead", true);
            animatorClone.SetBool("isDead", true);
            beforeDead_MyEvent.Invoke();

		}
	}
        public void DeadEnded(string deadName)
        { 
            audioCharacter.clip=Run;
            audioCharacter.loop = true;
            audioCharacter.Play();
            isCrouched = false;
            isJumping = false;
            isGrounded =true;
            if(deadName =="respawn"){
            hasEntered = false;
            afterDead_MyEvent.Invoke();
            //Destroy(gameObject);
            LevelManager.instance.Respawn();
            animator.SetBool("isDead", false);
            animatorClone.SetBool("isDead", false);
            deadName= "nothing";
            }

        }
    
}
