using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;        //Allows the use of Lists. 

public class GameManager : MonoBehaviour
{

	public float DayStartDelay = 2f;                        //Time to wait before starting the Day, in seconds.
	public static GameManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.
	public BoardManager boardScript;                        //Store a reference to our BoardManager which will set up the level.
	public int playerCoins = 100;								// Number of Coins
	private int day = 1;                                    //Current day number, expressed in game as "Day 1".
	[HideInInspector] public bool playersTurn = true;

	private Text DayText;                                    //Text to display current Day number.
	private GameObject DayImage;                            //Image background for levelText.
	private bool doingSetup = true;                            //Boolean to check if we're setting up board, prevent Player from moving during setup.

	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();
		Debug.Log (day);
		//Call the InitGame function to initialize the first Day 
		//InitGame();
	}

	//This is called each time a scene is loaded.
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		
		//Call InitGame to initialize our day.
		InitGame();

		//Add one to our day number.
		day++;

	}

	void OnEnable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled.
			
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	//Initializes the game for each day
	void InitGame()
	{
		
		//While doingSetup is true the player can't move, prevent player from moving while title card is up.
		doingSetup = true;

		//Get a reference to our image LevelImage by finding it by name.
		DayImage = GameObject.Find("DayImage");

		//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
		DayText = GameObject.Find("DayText").GetComponent<Text>();

		//Set the text of levelText to the string "Day" and append the current day number.
		DayText.text = "Day " + day;

		//Set dayImage to active blocking player's view of the game board during setup.
		DayImage.SetActive(true);

		//Call the HideLevelImage function with a delay in seconds of dayStartDelay.
		Invoke("HideLevelImage", DayStartDelay);

		//Call the SetupScene function of the BoardManager script, pass it current day number.
		boardScript.SetupScene(day);

	}

	//Hides black image used between levels
	void HideLevelImage()
	{
		//Disable the levelImage gameObject.
		DayImage.SetActive(false);

		//Set doingSetup to false allowing player to move again.
		doingSetup = false;
	}

	public void GameOver(){

		//Set levelText to display number of levels passed and game over message
		DayText.text = "After " + day + " days, you starved.";

		//Enable black background image gameObject.
		DayImage.SetActive(true);
	
		enabled = false;
	}


	//Update is called every frame.
	void Update()
	{
		if (doingSetup)
			return;

	}
}
