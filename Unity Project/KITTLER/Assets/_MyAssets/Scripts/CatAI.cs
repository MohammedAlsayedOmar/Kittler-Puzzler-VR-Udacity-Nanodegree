using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CatAI : MonoBehaviour {

    public Transform[] wayPoints;
    public Vector2 waitTime;

    public static bool FOOOD = false;

    bool isEating;
    bool isPlaying = false;
    public AudioClip eating;
    public AudioClip[] mewos;

    GvrAudioSource audioSrc;
    NavMeshAgent agent;
    Animator anim;

	void Start () {
        audioSrc = GetComponent<GvrAudioSource>();
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

        if (isEating && agent.velocity == Vector3.zero && !isPlaying)
        {
            isPlaying = true;
            audioSrc.clip = eating;
            audioSrc.Play();
        }

        anim.SetBool("Switch", agent.velocity != Vector3.zero);
	}

    IEnumerator MoveRandomly(float time)
    {
        //audioSrc.PlayOneShot(mewos[Random.Range(0, mewos.Length - 1)]);
        int index = randomIndex();
        agent.SetDestination(wayPoints[index].position);
        yield return new WaitForSeconds(time);


        StartCoroutine(MoveRandomly(randomNumber()));
    }

    float randomNumber()    {   return Random.Range(waitTime.x, waitTime.y);    }

    int randomIndex()    {  return Random.Range(1, wayPoints.Length - 1);    }

    IEnumerator EATALLTHEFOOD()
    {
        isPlaying = false;
        isEating = true;
        agent.SetDestination(wayPoints[0].position);
        yield return new WaitForSeconds(7f);
        isPlaying = false;
        isEating = false;
        Debug.Log("EATING NOW FALSE");
        audioSrc.Stop();
        GameLogic.plateHasFood = false;
        GameLogic.completed = true;
        GameLogic.setText("THANKS", "YUMMYYY!");

        StartCoroutine(MoveRandomly(randomNumber()));
    }
}
