using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLogic : MonoBehaviour {

    public Transform player;
    public Animator[] Doors;

    public GameObject[] Food;

    public static Text part1, part2;

    int hasFood = 0;
    int Selected = 0;
    public static bool plateHasFood = false;
    public static bool completed = false;


    public GameObject eventSystem;
    public GameObject[] puzzleSpheres; //An array to hold our puzzle spheres

    public int puzzleLength = 5; //How many times we light up.  This is the difficulty factor.  The longer it is the more you have to memorize in-game.
    public float puzzleSpeed = 1f; //How many seconds between puzzle display pulses
    private int[] puzzleOrder; //For now let's have 5 orbs

    private int currentDisplayIndex = 0; //Temporary variable for storing the index when displaying the pattern
    public bool currentlyDisplayingPattern = true;
    public bool playerWon = false;

    private int currentSolveIndex = 0; //Temporary variable for storing the index that the player is solving for in the pattern.

    public AudioClip buzz;
    public AudioClip winSound;

    int wonSecondTime = 0;

    void Start()
    {
        part1 = GameObject.Find("Part1").GetComponent<Text>();
        part2 = GameObject.Find("Part2").GetComponent<Text>();

        if (!part1 || !part2)
            Debug.LogError("WORDS NOT FOUND");

        puzzleOrder = new int[puzzleLength]; //Set the size of our array to the declared puzzle length
        generatePuzzleSequence(); //Generate the puzzle sequence for this playthrough.  
    }

    public void OnClickPlate()
    {
        if (plateHasFood)
            return;

        Food[0].SetActive(false);
        Food[1].SetActive(false);
        Food[2].SetActive(false);

        if (hasFood == 1) //Fish
        {
            Food[0].SetActive(true);
            plateHasFood = true;
            CatAI.FOOOD = true;
        }
        else if (hasFood == 2) //Milk
        {
            Food[1].SetActive(true);
            plateHasFood = true;
            CatAI.FOOOD = true;
        }
        else
        {
            if (!plateHasFood)
            {
                Food[2].SetActive(true);
                setText("NO FOOD", "SHAME ON YOU");
            }
        }

        hasFood = 0;
    }

    public void OnClickStartButton()
    {
       
        Doors[0].SetTrigger("openDoor");

        iTween.MoveTo(player.gameObject,
           iTween.Hash(
               "path", iTweenPath.GetPath("StartPath"),
               "time", 2,
               "easetype", "linear"
               ));

        eventSystem.SetActive(false);
        CancelInvoke("displayPattern");
        InvokeRepeating("displayPattern", 3, puzzleSpeed); //Start running through the displaypattern function
        currentSolveIndex = 0; //Set our puzzle index at 0
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    public void PuzzleWon()
    {      
        player.gameObject.GetComponent<GvrAudioSource>().PlayOneShot(winSound);
        Doors[1].SetTrigger("openDoor");

        iTween.MoveTo(player.gameObject,
        iTween.Hash(
        "path", iTweenPath.GetPath("WonPath"),
        "time", 3,
        "easetype", "linear",
        "oncomplete", "finishingFlourish",
        "oncompletetarget", this.gameObject
       ));
    }

    public void OnClickTakeButton()
    {
        hasFood = Selected;
        Selected = 0;

    }

    public void OnClickRestartButton()
    {
        StartCoroutine(closeDoors());
        iTween.MoveTo(player.gameObject,
        iTween.Hash(
        "path", iTweenPath.GetPath("RestartPath"),
        "time", 5,
        "easetype", "linear",
        "oncomplete", "resetGame",
        "oncompletetarget", this.gameObject
          ));
    }

    IEnumerator closeDoors()
    {
        Doors[1].SetTrigger("closeDoor");
        yield return new WaitForSeconds(3f);
        Doors[0].SetTrigger("closeDoor");
    }

    public void SelectFish()
    {
        Selected = 1;
    }

    public void SelectMilk()
    {
        Selected = 2;
    }

    public static void setText(string p1, string p2)
    {
        part1.text = p1;
        part2.text = p2;
    }

    void Update()
    {
        if (!plateHasFood && (Food[0].activeSelf || Food[1].activeSelf))
        {
            Food[0].SetActive(false);
            Food[1].SetActive(false);
        }

        if (completed)
        {
            Food[2].SetActive(true);
            completed = false;
        }
    }

    // Provided Functions
    public void playerSelection(GameObject sphere)
    {
        if (playerWon != true)
        { //If the player hasn't won yet
            int selectedIndex = 0;
            //Get the index of the selected object
            for (int i = 0; i < puzzleSpheres.Length; i++)
            { //Go through the puzzlespheres array
                if (puzzleSpheres[i] == sphere)
                { //If the object we have matches this index, we're good
                    Debug.Log("Looks like we hit sphere: " + i);
                    selectedIndex = i;
                }
            }
            solutionCheck(selectedIndex);//Check if it's correct
        }
    }

    public void solutionCheck(int playerSelectionIndex)
    { //We check whether or not the passed index matches the solution index
        if (playerSelectionIndex == puzzleOrder[currentSolveIndex])
        { //Check if the index of the object the player passed is the same as the current solve index in our solution array
            currentSolveIndex++;
            Debug.Log("Correct!  You've solved " + currentSolveIndex + " out of " + puzzleLength);
            if (currentSolveIndex >= puzzleLength)
            {
                PuzzleWon();
            }
        }
        else
        {
            puzzleFailure();
        }

    }

    void displayPattern()
    { //Invoked repeating.
        currentlyDisplayingPattern = true; //Let us know were displaying the pattern
        eventSystem.SetActive(false); //Disable gaze input events while we are displaying the pattern.

        if (currentlyDisplayingPattern == true)
        { //If we are not finished displaying the pattern
            if (currentDisplayIndex < puzzleOrder.Length)
            { //If we haven't reached the end of the puzzle
                Debug.Log(puzzleOrder[currentDisplayIndex] + " at index: " + currentDisplayIndex);
                puzzleSpheres[puzzleOrder[currentDisplayIndex]].GetComponent<lightUp>().patternLightUp(puzzleSpeed); //Light up the sphere at the proper index.  For now we keep it lit up the same amount of time as our interval, but could adjust this to be less.
                currentDisplayIndex++; //Move one further up.
            }
            else
            {
                Debug.Log("End of puzzle display");
                currentlyDisplayingPattern = false; //Let us know were done displaying the pattern
                currentDisplayIndex = 0;
                CancelInvoke(); //Stop the pattern display.  May be better to use coroutines for this but oh well
                eventSystem.SetActive(true); //Enable gaze input when we aren't displaying the pattern.
            }
        }
    }

    public void generatePuzzleSequence()
    {
        int tempReference;
        for (int i = 0; i < puzzleLength; i++)
        { //Do this as many times as necessary for puzzle length
            tempReference = Random.Range(0, puzzleSpheres.Length); //Generate a random reference number for our puzzle spheres
            puzzleOrder[i] = tempReference; //Set the current index to our randomly generated reference number
        }
    }

    public void puzzleFailure()
    { //Do this when the player gets it wrong
        player.gameObject.GetComponent<GvrAudioSource>().PlayOneShot(buzz);

        currentSolveIndex = 0;

        eventSystem.SetActive(false);
        CancelInvoke("displayPattern");
        InvokeRepeating("displayPattern", 3, puzzleSpeed); //Start running through the displaypattern function
        currentSolveIndex = 0; //Set our puzzle index at 0
    }

    public void finishingFlourish()
    { //A nice visual flourish when the player wins     
        playerWon = true;
    }

    public void resetGame()
    {
        wonSecondTime++;
        if (wonSecondTime == 2)
        {
            wonSecondTime = 0;
            puzzleLength++;
        }
        puzzleOrder = new int[puzzleLength]; //Set the size of our array to the declared puzzle length
        playerWon = false;
        generatePuzzleSequence(); //Generate the puzzle sequence for this playthrough.  
    }


}