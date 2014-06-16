using UnityEngine;
using System.Collections;

public class ChargeScript : MonoBehaviour {

	private Animator animator;
	public float startAction = 11f;
	public float chargeTime = 1f;
	public float timer;
	public float timer2;
	public float startTime;
	public bool ChargedPunch = false;
	public bool GreenColor = false;
	public bool YellowColor = false;
	public bool RedColor = false;
	private PlayerScript playerScript;



	void Awake()
	{
		animator = GetComponent<Animator>();
		//playerScript = GetComponent<PlayerScript> ();
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();
		
	}

	// Use this for initialization
	void Start () 
	{

	}
	

	// Update is called once per frame
	void Update () {
	

		//WORKS :)

		if (Time.timeSinceLevelLoad > startAction + chargeTime) 
		{
			RedColor = true;
			if(RedColor == true)
			{
				//
				StartCoroutine ("Yellow");
			}
			if (YellowColor == true) 
			{
				playerScript.Punched = false;
				StopCoroutine("Yellow");
				StartCoroutine ("Green");
			}

			if (playerScript.Punched == true) 
			{
				StopCoroutine("Green");
				animator.SetBool ("Green", false);
				animator.SetBool ("PunchedCharge", true);
				
				ChargedPunch = false;
				RedColor = true;
				GreenColor = false;
			}

			if(GreenColor == true)
			{
				//Debug.Log ("Green, Charged");
			}
		}
	}

	void FixedUpdate()
	{

	}
	IEnumerator Yellow()
	{
		yield return new WaitForSeconds (2.0f);

		animator.SetBool ("Yellow", true);
		animator.SetBool ("PunchedCharge", false);

		ChargedPunch = false;
		YellowColor = true;
		RedColor = false;
	}
	IEnumerator Green()
	{
		yield return new WaitForSeconds (2.0f);

		animator.SetBool ("Green", true);
		animator.SetBool ("Yellow", false);

		ChargedPunch = true;
		GreenColor = true;
		YellowColor = false;
		RedColor = false;
	}
	
}
