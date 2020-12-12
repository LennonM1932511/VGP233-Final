using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossNPC : MonoBehaviour
{
    [SerializeField]
    Animator animator = null;
    NavMeshAgent agent;

    [SerializeField]
    float detectionRange = 15.0f;
    [SerializeField]
    float fightRange = 1.0f;
    [SerializeField]
    private float moveSpeed = 10.0f;
    bool isDead = false;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        Vector3 playerPosition = target.position;
        float distanceToPlayer = Vector3.Distance(playerPosition, transform.position);
        if (distanceToPlayer <= fightRange)
        {
            Attack(target);
        }
        else if (distanceToPlayer <= detectionRange)
        {
            Follow(target);
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        if (!animator.GetAnimatorTransitionInfo(0).IsName("Idle"))
            animator.SetTrigger("isIdle");
        agent.isStopped = true;
    }

    private void Follow(Transform targetTransform)
    {
        agent.transform.LookAt(targetTransform);
        agent.isStopped = false;
        agent.SetDestination(targetTransform.position);

        if (!animator.GetAnimatorTransitionInfo(0).IsName("BossRun"))
            animator.SetTrigger("isRunning");
    }

    private void Attack(Transform targetTransform)
    {
        agent.isStopped = true;
        agent.transform.LookAt(targetTransform);
        if (!animator.GetAnimatorTransitionInfo(0).IsName("BossAttack"))
            animator.SetTrigger("isAttacking");
    }

    public void DeathAnimation()
    {
        isDead = true;
        agent.isStopped = true;
        if (!animator.GetAnimatorTransitionInfo(0).IsName("BossDying"))
            animator.SetTrigger("isDead");
    }
}
