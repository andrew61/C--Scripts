using UnityEngine;
using System.Collections;
/// <summary>
/// Player controller and behavior
/// </summary>
public class AIScript : MonoBehaviour
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
	//public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 365f;			// Amount of force added when the player jumps.
	//public AudioClip[] taunts;				// Array of clips for when the player taunts.
	//public float tauntProbability = 50f;	// Chance of a taunt happening.
	//public float tauntDelay = 1f;			// Delay for when the taunt should happen.
	
	
	//private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = true;			// Whether or not the player is grounded.
	private Animator animator;
	public float startAction = 11f;
	public float periodTime = 0.0f;
	public float chargeTime = 1f;
	public bool Punched = false;
	public bool CanJump = false;
	public bool WonRight = false;
	public bool WonLeft = false;
	public bool Green = true;
	public bool CoroutineRunning = false;
	public bool CoroutineRunning2 = false;

	
	private ChargeScriptAI chargeScriptai;
	private ChargeScript chargeScript;
	private PlayerScript playerScript;

	
	
	
	
    public Transform target;//set target from inspector instead of looking in Update
    //toward
	public float speed = 3f;
	//away
	public float speedNeg = -3f;
	
	void Awake()
	{
		// Get the animator
		animator = GetComponent<Animator>();
		chargeScriptai = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ChargeScriptAI> ();
		chargeScript = GameObject.FindGameObjectWithTag ("Respawn").GetComponent<ChargeScript> ();
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();

	}
	void Start()
	{

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
		if (ground.gameObject.tag == "Player") 
		{
			if(Punched == true)
			{
				WonRight = true; //reference this as true in ai script to do losing animation
				Debug.Log ("touching and punch is true");
			}
		}
		
		//if touch enemy, works
		if(ground.gameObject.tag == "Player")
		{
			Debug.Log("Touched Player");
		}
		
	}

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
			if (ground.gameObject.tag != "Player") 
			{
				Debug.Log ("Missed Player");
				Punched = false;
			}
		}
	}

	void Update()
	{
	if(Time.timeSinceLevelLoad > startAction)
	{
  		//rotate to look at the player
       transform.LookAt(target.position);
       transform.Rotate(new Vector3(0,-90,0),Space.Self);//correcting the original rotation
 


			/*else if ((Vector3.Distance(transform.position,target.position)>3f) && !facingRight)
			{
				transform.position += transform.forward*maxSpeed*Time.deltaTime;
				
				animator.SetBool ("punchingRight", false);
				
				animator.SetBool ("punchingLeft", true);
				
			}*/
 
		//if green
		if(chargeScriptai.ChargedPunch == true)
		{
			//move toward player
			if ((Vector3.Distance(transform.position,target.position)>3f) && facingRight)
			{//move if distance from target is greater than 1
				//transform.position += transform.forward*maxSpeed*Time.deltaTime;
				transform.Translate(new Vector3(speed* Time.deltaTime,0,0) );
					
				animator.SetBool ("punchingLeft", false);
					
				animator.SetBool ("punchingRight", false);
			}
			//if close to player
			if ((Vector3.Distance(transform.position,target.position)<3f) && facingRight)
			{//move if distance from target is less than 1
				//transform.position += transform.forward*maxSpeed*Time.deltaTime;
				transform.Translate(new Vector3(speed* Time.deltaTime,0,0) );
					
				animator.SetBool ("punchingLeft", false);
				Punched = true;
				Green = false;
				animator.SetBool ("punchingRight", true);
			}
				
		}
		
		if(chargeScriptai.YellowColor == true)
		{
			animator.SetBool ("punchingRight", false);
		}
		
		if(chargeScriptai.ChargedPunch == false)
		{
			//if close to player move away
			if ((Vector3.Distance(transform.position,target.position)<10f) && facingRight)
			{
				transform.Translate(new Vector3(speedNeg* Time.deltaTime,0,0) );
			}
		}
		if(playerScript.WonRight == true)
		{
			transform.Rotate(new Vector3(0,0,90), Space.Self);
				Time.timeScale = 0.5f;

		}
		/*
		 * If player has charged punch, and ai is not, back away
		 * If ai has charged punch and player does not, move toward
		 * If both have charged punch move toward slowly
		*/

		//if ai wins
		if(WonRight == true)
		{
			animator.SetBool ("RightWin", true);
		}
		if(WonLeft == true)
		{
			animator.SetBool ("LeftWin", true);
		}


		/*
		 * //if not green
		if(chargeScriptai.ChargedPunch == false)
		{
			//Debug.Log ("Not Green");
			StartCoroutine("WaitTime");
			
			if(CoroutineRunning == true)
			{
				StopCoroutine("WaitTime");
				StartCoroutine("WaitTime2");
				
				if(CoroutineRunning2 == true)
				{
					Debug.Log ("ai punch is false");
					animator.SetBool ("punchingRight", false);
				}
			}
		}
		*/
	}

	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

	}
	
	void Jump()
	{
		rigidbody2D.AddForce (new Vector2 (0, jumpForce));

	}
	void FixedUpdate()
	{

		if (HP <= 0 && !dead) 
			Death ();
	}

	public void Hurt()
	{
		HP--;
	}
	
	void Death()
	{
		if (gameObject != null) 
		{
			dead = true;
			
			Destroy (gameObject, 1);
			Application.LoadLevel("MainMenu");
		}

	}

	public IEnumerator WaitFor()
	{
		if (Punched == true) 
		{
			yield return new WaitForSeconds (3.0f);
			CoroutineRunning = true;

		}
	}

	public IEnumerator AfterFor()
	{
		if (CoroutineRunning == true) 
		{
			yield return new WaitForSeconds(2.0f);
			//yield return StartCoroutine ("WaitFor");
			HasPunched();
		}
	
	}


	void HasPunched()
	{
		Debug.Log ("void haspunched running");
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
