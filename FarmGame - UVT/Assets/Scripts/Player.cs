using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;        // To use SceneManager
using UnityEngine.UI;

//Player inherits from MovingObject, base class for objects that can move.
public class Player : MovingObject
{
	public float restartDayDelay = 1f;        		//Delay time in seconds to restart Day.
	public int floorDamage = 1;                    //How much damage a player does to the floor when iteracting with it.
	public Text CoinsText;


	private Animator animator;                    //Used to store a reference to the Player's animator component.
	private int coins;                            //Used to store player total of coins during the Day.


	//Start overrides the Start function of MovingObject
	protected override void Start ()
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();

		//Get the current total of coins stored in GameManager.instance between levels.
		coins = GameManager.instance.playerCoins;

		CoinsText.text = "Coins: " + coins;

		//Call the Start function of the MovingObject base class.
		base.Start ();
	}


	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable ()
	{
		//When Player object is disabled, store the current local coins total in the GameManager so it can be re-loaded in the next Day.
		GameManager.instance.playerCoins = coins;

	}

	private void Update ()
	{

		int horizontal = 0;      //Used to store the horizontal move direction.
		int vertical = 0;        //Used to store the vertical move direction.


		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		animator.SetFloat ("Horizontal", horizontal);


		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int) (Input.GetAxisRaw ("Vertical"));
		animator.SetFloat ("Vertical", vertical);

		//Check if moving horizontally, if so set vertical to zero.
		if(horizontal != 0)
		{
			vertical = 0;
		}

		//Check if we have a non-zero value for horizontal or vertical
		if(horizontal != 0 || vertical != 0)
		{
			//Call AttemptMove passing in the generic parameter Floor, since that is what Player may interact with if they encounter one (by attacking it)
			//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			AttemptMove<Floor> (horizontal, vertical);
		}

	}

	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove <T> (xDir, yDir);

		//Hit allows us to reference the result of the Linecast done in Move.
		RaycastHit2D hit;

		//If Move returns true, meaning Player was able to move into an empty space.
		if (Move (xDir, yDir, out hit)) {

			//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.


		} else {
			animator.SetFloat ("Horizontal", 0);
			animator.SetFloat ("Vertical", 0); // Verificar a rotacao do player -> urgente
		}

		// Check if the game has ended.
		CheckIfGameOver ();
	}


	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove <T> (T component)
	{
		//Set hitFloor to equal the component passed in as a parameter.
		Floor hitFloor = component as Floor;

		//Call the DamageWall function of the Wall we are hitting.
		hitFloor.DamageFloor (floorDamage);

		//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
		//animator.SetTrigger ("playerChop");
	}


	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if(other.tag == "Exit")
		{
			//Invoke the Restart function to start the next Day with a delay of restartDayDelay (default 1 second).
			Invoke ("Restart", restartDayDelay);

			//Disable the player object since level is over.
			enabled = false;
		}
	}


	//Restart reloads the scene when called.
	private void Restart ()
	{
		//Load the last scene loaded, in this case Main, the only scene in the game.
		SceneManager.LoadScene (0); // Check
	}

	//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
	private void CheckIfGameOver ()
	{
		//Check if food point total is less than or equal to zero.
		if (coins < 0) 
		{
			//Call the GameOver function of GameManager.
			GameManager.instance.GameOver ();
		}
	}
}
