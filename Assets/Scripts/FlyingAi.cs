using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAi : MonoBehaviour
{
    enum FishState
    {
        Patrol = 0, Chase =1, Attack =2,Dead = 3
    }
    FishState currState;
    int wayPointsCounter;
    Rigidbody rb;
    [SerializeField] Transform[] wayPoints;
    [SerializeField] float speed;
    [SerializeField] GameObject player;
    [SerializeField] int damage;
    ZombieHealt health;
    private Animator animator;
    PlayerHealth playerHealth;
    private float attackTime =0;
    bool isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currState = FishState.Patrol;
        health = GetComponent<ZombieHealt>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            currState = FishState.Patrol;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            currState = FishState.Chase;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            currState = FishState.Attack;
        }
        ChangeState(currState);        
        if (attackTime > 0)
        {
            attackTime += Time.deltaTime;
            if (attackTime > 2)
            {
                attackTime= 0;
            }
        }
    }
    private void ChangeState(FishState state)
    {
        switch (state)
        {
            case FishState.Patrol: Patrol(); break;
            case FishState.Chase: Chase(); break;
            case FishState.Attack: Attack(); break;
            
            case FishState.Dead:
                if (!isDead)
                {
                    print("BirD DEADD");
                    WaveSpawner.EnemiesAlive--;
                    isDead= true;
                }
                Dead();
                break; 
        }
        animator.SetInteger("State", (int)state);
    }

    void Patrol()
    {
        print("patrolling");
        transform.LookAt(wayPoints[wayPointsCounter].transform.position);
        if (health.GetHealth()<=0 && !isDead)
        {            
            isDead = true;
            currState= FishState.Dead;
            //WaveSpawner.EnemiesAlive--;
        }
        print(wayPointsCounter);
        if (Vector3.Magnitude(wayPoints[wayPointsCounter].transform.position - transform.position) < 2f)
        {
            wayPointsCounter++;
            if (wayPointsCounter >= wayPoints.Length)
            {
                wayPointsCounter = 0;
            }
        }
        seeking(wayPoints[wayPointsCounter].transform.position);
        if (Vector3.Magnitude(player.transform.position - transform.position) < 10f)
        {
            currState = FishState.Chase;
        }
    }

    void Chase()
    {
        print("Chasing");
        transform.LookAt(player.transform.position);

        if (health.GetHealth() <= 0 && !isDead)
        {            
            //isDead = true; 
            currState = FishState.Dead;
            //WaveSpawner.EnemiesAlive--;
        }
        if (Vector3.Magnitude(player.transform.position - transform.position) < 10f)
        {
            seeking(player.transform.position);
        }
        if (Vector3.Magnitude(player.transform.position - transform.position) > 10f)
        {
            currState = FishState.Patrol;
        }
        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 3f)
        {
                currState = FishState.Attack;

        }
        print(player.transform.position - transform.position);
    }
    private void Attack()
    {
        transform.LookAt(player.transform.position);
        if (health.GetHealth() <= 0)
        {
            currState = FishState.Dead;
        }
        print("ATACKKKKKKKK");
        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 5f)
        {
            rb.velocity = Vector3.zero;
            if (attackTime == 0)
            {
                playerHealth.DedectHealth(damage);
                attackTime += Time.deltaTime;
            }
        }
        else if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) > 5f)
        {
            //player.GetComponent<MeshRenderer>().material.color = Color.green;
            currState = FishState.Chase;
        }
        else
        {
            currState = FishState.Patrol;
            //player.GetComponent<MeshRenderer>().material.color = Color.green;

        }
    }
    private void Dead()
    {
        rb.velocity = Vector3.zero;
        print("DIEDDD");
        animator.SetTrigger("isDead");
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 3.0f);
    }
    void seeking(Vector3 target)
    {
        var dir = target - transform.position;
        rb.velocity = dir * speed;
        //rb.AddForce(dir);
    }
    
}
