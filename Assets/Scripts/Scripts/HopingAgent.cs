using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class HoppingAgent : MonoBehaviour
{
    public Transform target;          // Destination point
    public float hopHeight = 1.0f;    // Maximum height of the hop
    public float hopFrequency = 2.0f; // Hops per second

    private NavMeshAgent agent;
    private float originalBaseOffset;
    private float hopTimer;
    public string DialogToSay;

    public TMP_Text dialogText;

    public List<Animation> animations;

    Animator animator;



    LeanSelectableByFinger selector;


    private bool hasRotated = false;     // Flag to prevent continuous rotation
    private Quaternion targetRotation;
    public float rotationSpeed = 10.0f;

    public List<String> sentencesForHelper;

    public List<ParticleSystem> randomEffects;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalBaseOffset = agent.baseOffset;
        agent.SetDestination(target.position);
        agent.updatePosition = false; // We'll manually update the position
        DialogToSay = "Count Stars!!";
        selector = GetComponent<LeanSelectableByFinger>();
        animator = GetComponent<Animator>();

    }


    bool sayOnce = true;
    void Update()
    {
        dialogText.transform.LookAt(Camera.main.transform);
        dialogText.transform.Rotate(0, 180, 0);
        MoveTheObject();
        if (selector.IsSelected)
        {
            if (sayOnce)
            {
                // SaySentence("You can do it");
                animator.SetTrigger("TransitionInt" + UnityEngine.Random.Range(2, 7));
                int num = UnityEngine.Random.Range(0, randomEffects.Count);
                randomEffects[num].Play();
                sayOnce = false;
            }
        }
        else
        {
            sayOnce = true;
        }


    }

    public void SetNextDestination(Transform targetposition)
    {
        agent.SetDestination(targetposition.position);
    }




    bool callOnce = true;




    void MoveTheObject()
    {
        // if (agent.pathPending)
        //     return;


        // Vector3 nextPosition = agent.nextPosition;
        // agent.Move(agent.desiredVelocity * Time.deltaTime);

        // agent.nextPosition = transform.position;

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (callOnce)
            {
                StartCoroutine(ShowText(DialogToSay,0.5f));
            }
            SmoothRotateTowardsCamera();


        }
        // else
        // {
        //     hopTimer += Time.deltaTime * hopFrequency * Mathf.PI * 2;
        //     float verticalOffset = Mathf.Abs(Mathf.Sin(hopTimer)) * hopHeight;
        //     // Update the GameObject's position
        //     nextPosition.y = agent.nextPosition.y + verticalOffset;
        //     transform.position = nextPosition;
        //     callOnce = true;
        //     hasRotated = false;
        // }

    }

    public IEnumerator ShowText(string WriteText,float timeToDisapear)
    {
        callOnce = false;
        dialogText.text = "";
        for (int i = 0; i <= WriteText.Length; i++)
        {
            dialogText.text = WriteText.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(timeToDisapear);
        dialogText.text = "";
    }




    void SmoothRotateTowardsCamera()
    {
        targetRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Check if rotation is nearly complete
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            transform.rotation = targetRotation; // Snap to the target rotation
        }
    }



    void SaySentence(String sentence)
    {
        StartCoroutine(ShowText(sentence, 2));
    }
}
