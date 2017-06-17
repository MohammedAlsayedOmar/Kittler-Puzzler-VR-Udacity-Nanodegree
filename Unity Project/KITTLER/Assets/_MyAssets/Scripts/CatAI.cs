using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CatAI : MonoBehaviour {

    public Transform[] wayPoints;
    public Vector2 waitTime;

    public static bool FOOOD = false;

    NavMeshAgent agent;
    Animator anim;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartCoroutine(MoveRandomly(randomNumber()));
    }

    void Update () {
		if (FOOOD)
        {
            FOOOD = false;
            StopAllCoroutines();
            StartCoroutine(EATALLTHEFOOD());
        }

        anim.SetBool("Switch", agent.velocity != Vector3.zero);
	}

    IEnumerator MoveRandomly(float time)
    {
        int index = randomIndex();
        agent.SetDestination(wayPoints[index].position);
        yield return new WaitForSeconds(time);


        StartCoroutine(MoveRandomly(randomNumber()));
    }

    float randomNumber()    {   return Random.Range(waitTime.x, waitTime.y);    }

    int randomIndex()    {  return Random.Range(1, wayPoints.Length - 1);    }

    IEnumerator EATALLTHEFOOD()
    {
        agent.SetDestination(wayPoints[0].position);
        yield return new WaitForSeconds(7f);
        GameLogic.plateHasFood = false;
        GameLogic.completed = true;
        GameLogic.setText("THANKS", "FOOD WAS GREAT");

        StartCoroutine(MoveRandomly(randomNumber()));
    }
}
