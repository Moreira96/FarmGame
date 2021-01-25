using UnityEngine;
using System;
using System.Collections.Generic;         //Allows us to use Lists.
using Random = UnityEngine.Random;         //Tells Random to use the Unity Engine random number generator.


	public class BoardManager : MonoBehaviour
	{
		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count
		{
			public int minimum;             //Minimum value for our Count class.
			public int maximum;             //Maximum value for our Count class.


			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}


		public int columns = 8;                                         //Number of columns in our game board.
		public int rows = 8;                                            //Number of rows in our game board.
		public Count wallCount = new Count (5, 9);                        //Lower and upper limit for our random number of walls per level.
		public Count foodCount = new Count (1, 5);                        //Lower and upper limit for our random number of food items per level.
		public GameObject exit;                                            //Prefab to spawn for exit.
		public GameObject[] floorTiles;                                    //Array of floor prefabs.
		public GameObject[] outerWallTiles;                                //Array of outer tile prefabs.

		private Transform boardHolder;                                    //A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();    //A list of possible locations to place tiles.

		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();

			//Loop through x axis (columns).
			for(float x = 1; x <= columns+1; x++)
			{
				//Within each column, loop through y axis (rows).
				for(float y = 1; y <= rows+1; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}


		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;

			GameObject toInstantiate = null;

			int aux = 0;

			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(float x = -0.5f; x <= columns + 1; x ++)
			{
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for (float y = -0.5f; y <= rows + 1; y++) {
				//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
				if (x == -0.5f || x == columns + 0.5f || y == -0.5f || y == rows + 0.5f) {
					if (x == -0.5f || x == columns + 0.5f) {
						toInstantiate = outerWallTiles [1];
						aux = 1;
					}
					if (y == -0.5f || y == rows + 0.5f) {
						toInstantiate = outerWallTiles [0];
						aux = 0;
					}
				
					
					if (aux == 1) {
						if (x == columns + 0.5f)
							x += 1f;
						x -= 0.25f;
						y += 0.5f;
					}

					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject			
					GameObject instance = 
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					if (aux == 1) {
						if (x == columns + 1.25f)
							x -= 1f;
							x += 0.25f;
							y -= 0.5f;
						}
							aux = 0;
				
							//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
							instance.transform.SetParent (boardHolder);
						}
					}
				}
			}

	void BoardGrassSetup ()
	{
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject ("Board").transform;

		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(float x = -0.5f; x <= columns + 1; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(float y = -0.5f; y <= rows + 1; y++)
			{
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];

				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject			
				GameObject instance = 
					Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent (boardHolder);
			}
		}
	}
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{

			//Creates the outer walls
			BoardSetup ();

			//Creates the floor
			BoardGrassSetup ();

			//Reset our list of gridpositions.
			InitialiseList ();

			//Instantiate the exit tile in the upper right hand corner of game board
			Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		}
	}