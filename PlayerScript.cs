using UnityEngine;
using System;
/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
	/// <summary>
	/// 1 - The speed of the player
	/// </summary>
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	public int HP = 2;
	public bool dead = false;

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 365f;			// Amount of force added when the player jumps.
	
	
	
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	//private bool grounded = true;			// Whether or not the player is grounded.
	private Animator animator;
	public float startAction = 11f;
	public float chargeTime = 1f;
	public bool Punched = false;
	public bool CanJump = false;
	public bool WonRight = false;
	public bool WonLeft = false;

	//public float periodTime = 0.0f;

	private ChargeScript chargeScript;
	private AIScript aiScript;


	void Awake()
	{
		animator = GetComponent<Animator>();  
		chargeScript = GameObject.FindGameObjectWithTag ("Respawn").GetComponent<ChargeScript> ();
		aiScript = GameObject.FindGameObjectWithTag ("Finish").GetComponent<AIScript>();
	}

	void Start()
	{
		//GetComponent<ChargeScript>().enabled = false;

			
	}
	
	void Update()
	{
		if(Time.timeSinceLevelLoad > startAction)
		{
			if(aiScript.WonRight == true)
			{
				transform.Rotate(new Vector3(0,0,-90), Space.Self);
				Time.timeScale = 0.5f;
				//Get time since this is true after so many seconds display winner text or loser text
			}

			if(chargeScript.ChargedPunch == true)
			{
				//punching right
				
				if (Input.GetKeyDown (KeyCode.Mouse1)) {
					animator.SetBool ("runningLeft", false);
					animator.SetBool ("runningRight", false);
					animator.SetBool ("punchingLeft", false);
					animator.SetBool ("punchingRight", true);
					
					Punched = true;
					Debug.Log ("Punched");
					
				}
				
				//punching left
				if (Input.GetKeyDown (KeyCode.Mouse0)) {
					animator.SetBool ("runningLeft", false);
					animator.SetBool ("runningRight", false);
					animator.SetBool ("punchingLeft", true);
					
					Punched = true;
					Debug.Log ("Punched");
					
					
				}
			}
			
			//if player is with collision of ground
			if(CanJump == true)
			{
				// If the jump button is pressed and the player is grounded then the player should jump.
				if (Input.GetKeyDown (KeyCode.Space))
				{ 
					rigidbody2D.AddForce (new Vector2 (0, jumpForce));
				}
			}
			
			//if player wins
			if(WonRight == true)
			{
				animator.SetBool ("RightWin", true);
			}
			if(WonLeft == true)
			{
				animator.SetBool ("LeftWin", true);
			}
			
			
			
			
			//not punching right
			if (Input.GetKeyUp (KeyCode.Mouse1)) {
				animator.SetBool ("punchingRight", false);
			}
			
			//running right
			if (Input.GetKeyDown (KeyCode.RightArrow) && facingRight) {
				animator.SetBool ("facingLeft", false);
				animator.SetBool ("runningLeft", false);
				animator.SetBool ("punchingLeft", false);
				
				animator.SetBool ("runningRight", true);
				
			}
			//not running right
			if (Input.GetKeyUp (KeyCode.RightArrow)) {
				
				animator.SetBool ("runningRight", false);
				
			}
			
			
			//facing left
			if (Input.GetKeyDown (KeyCode.LeftArrow) && !facingRight) {
				animator.SetBool ("runningRight", false);
				animator.SetBool ("punchingRight", false);
				
				animator.SetBool ("facingLeft", true);
			}
			//running left
			
			if (Input.GetKeyDown (KeyCode.LeftArrow) && !facingRight) {
				animator.SetBool ("runningRight", false);
				animator.SetBool ("punchingRight", false);
				
				animator.SetBool ("runningLeft", true);
			}
			//not running left
			if (Input.GetKeyUp (KeyCode.LeftArrow) && !facingRight) {
				animator.SetBool ("runningLeft", false);
			}
			
			
			
			//not punching left
			if (Input.GetKeyUp (KeyCode.Mouse0)) {
				animator.SetBool ("punchingLeft", false);
			}
			// Cache the horizontal input.
			float h = Input.GetAxis("Horizontal");
			
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			//anim.SetFloat("Speed", Mathf.Abs(h));
			
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * rigidbody2D.velocity.x < maxSpeed)
				// ... add a force to the player.
				rigidbody2D.AddForce(Vector2.right * h * moveForce);
			
			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
			
			// If the input is moving the player right and the player is facing left...
			if(h > 0 && facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(h < 0 && !facingRight)
				// ... flip the player.
				Flip();
			/*
		if(jump)
		{
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
		*/
			
			
		}
		if (HP <= 0 && !dead) 
			Death ();

	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
	}
	
	//jumping works :)
	void OnCollisionEnter2D(Collision2D ground)
	{
		//ground
		if (ground.gameObject.tag == "EditorOnly") 
		{
			CanJump = true;
			Debug.Log ("On Ground");
		} 

		//works :)
		if (ground.gameObject.tag == "Finish") 
		{
			if(Punched == true)
			{
				WonRight = true; //reference this as true in ai script to do losing animation
				Debug.Log ("touching and punch is true");
			}
		}

		//if touch enemy, works, stop running animation in contact
		if(ground.gameObject.tag == "Finish")
		{
			animator.SetBool ("runningRight", false);
			animator.SetBool ("runningLeft", false);

			Debug.Log("Touched Enemy");
		}

	}
		
		
	//}
	void OnCollisionExit2D(Collision2D ground)
	{
		//ground
		if (ground.gameObject.tag == "EditorOnly") 
		{
			CanJump = false;
			Debug.Log ("In Air");
		}

		if (Punched == true) 
		{
			if (ground.gameObject.tag != "Finish") 
			{
				Debug.Log ("Missed Enemy");
				Punched = false;
			}
		}
	}
	


	void FixedUpdate()
	{

	
	
	}

	public void Hurt()
	{
				HP--;
	}

	void Death()
	{
		//if player dies, null reference exception, fix it...
		if (gameObject != null) {
						dead = true;
						Destroy (gameObject, 1);
						Application.LoadLevel("MainMenu");
						
				}

		}
	/*
	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);
			
			// If there is no clip currently playing.
			if(!audio.isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();
				
				// Play the new taunt.
				audio.clip = taunts[tauntIndex];
				audio.Play();
			}
		}
	}
	
	
	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);
		
		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}
	*/
	
	  
}
