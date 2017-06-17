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

    void Start()
    {
        part1 = GameObject.Find("Part1").GetComponent<Text>();
        part2 = GameObject.Find("Part2").GetComponent<Text>();

        if (!part1 || !part2)
            Debug.LogError("WORDS NOT FOUND");
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
        //player.transform.position = wayPoints[1].position;
        Doors[0].SetTrigger("openDoor");
        //iTween.MoveTo(player.gameObject,
        //    iTween.Hash(
        //        "position", wayPoints[1].position,
        //        "time",2,
        //        "easetype","linear"
        //        ));
        iTween.MoveTo(player.gameObject,
           iTween.Hash(
               "path", iTweenPath.GetPath("StartPath"),
               "time", 2,
               "easetype", "linear"
               ));


    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    public void PuzzleWon()
    {
        //player.transform.position = wayPoints[2].position;
        Doors[1].SetTrigger("openDoor");
        //iTween.MoveTo(player.gameObject,
        //    iTween.Hash(
        //        "position", wayPoints[2].position,
        //        "time", 2,
        //        "easetype", "linear"
        //        ));
        iTween.MoveTo(player.gameObject,
        iTween.Hash(
       "path", iTweenPath.GetPath("WonPath"),
       "time", 3,
       "easetype", "linear"
       ));
    }

    public void OnClickTakeButton()
    {
        hasFood = Selected;
        Selected = 0;

    }

    public void OnClickRestartButton()
    {
        //player.transform.position = wayPoints[0].position;
        Doors[0].SetTrigger("closeDoor");
        Doors[1].SetTrigger("closeDoor");
        iTween.MoveTo(player.gameObject,
        iTween.Hash(
         "path", iTweenPath.GetPath("RestartPath"),
           "time", 5,
           "easetype", "linear"
          ));
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
}
