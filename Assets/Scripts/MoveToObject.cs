using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToObject : MonoBehaviour
{
    public Transform Target;
    GameObject[] Enemys;
    public float SpaceBetween;
    public enum State {Attack, Swarm, Heal};
    public State currentState;
    public float speed;
    public float health;
    public float counter; // TEST CODE TODO REMOVE
    public float NearbyEnemies;
    public float maxHealth;
    public float swarmDistance;
    public float swarmSize;
    private NavMeshAgent navMeshAgent;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        SpaceBetween = 0.5f;
        speed = 10.0f;
        health = 100.0f;
        maxHealth = 100.0f;
        currentState = State.Swarm;
        //currentState = State.Attack;
        counter = 0.0f; // TEST CODE TODO REMOVE
        NearbyEnemies = 0.0f;
        swarmDistance = 5.0f;
        swarmSize = 5.0f;
        
    }

    // Update is called once per frame
    void Update()
    {   
        //TESTING CODE
        counter++;
        if (counter % 100 == 0 && health > 0) {
            health--;
            Debug.Log(health);
        }
        //END TESTING CODE

        // The finite state machine choses what target to move towards
        switch(currentState) {
            case State.Attack: {
                Target = GameObject.FindGameObjectsWithTag("Player")[0].transform; // NOTE assumes only 1 player
                
                if (NearbyEnemies < 5.0f) {
                    currentState = State.Swarm;
                }
                if (health <= 50.0f) {
                    if (GameObject.FindGameObjectsWithTag("Healer").Length > 0) // Only switch to heal if there is a healer
                    currentState = State.Heal;
                    Debug.Log("ouch");
                }
            }
            break;

            case State.Heal: {
                GameObject[] Healers = GameObject.FindGameObjectsWithTag("Healer");
                float MinDistance = 10000000.0f; 
                foreach (GameObject go in Healers) {  // Finds the closest healer
                    float distance = Vector3.Distance(go.transform.position, this.transform.position);
                    if (distance < MinDistance) {
                        MinDistance = distance;
                        Target = go.transform;
                    }
                }
                if (health == maxHealth) {
                    currentState = State.Swarm;
                }
            }
            break;

            case State.Swarm: {
                
                GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
                float MinDistance = 10000000.0f; 
                foreach (GameObject go in Enemys) {  // Finds the closest enemy
                    if (go != gameObject) {
                        float distance = Vector3.Distance(go.transform.position, this.transform.position);
                        if (distance >= swarmDistance) {
                            if (distance < MinDistance) {
                                //Debug.Log("got here");
                                MinDistance = distance;
                                Target = go.transform;
                            }
                        }
                    }
                
                //Vector3 TargetDirectionEnemy = Target.position - transform.position;
                //TargetDirectionEnemy = Vector3.Normalize(TargetDirectionEnemy) * speed; // set the magnitude to 1 then multiply by speed
                //transform.Translate(TargetDirectionEnemy * Time.deltaTime);
                //Target = GameObject.FindGameObjectsWithTag("Player")[0].transform;

                }
                Debug.Log(NearbyEnemies);
                if (NearbyEnemies >= swarmSize) {
                    currentState = State.Attack;
                }

                if (health <= 50) {
                    if (GameObject.FindGameObjectsWithTag("Healer").Length > 0) { // Only switch to heal if there is a healer
                        currentState = State.Heal;
                    }
                }
                
            }
            break;

        } // end of switch 
        
        // Calculate how many enemies are nearby, so the FSM can decide if it needs to swarm or not
        NearbyEnemies = 0;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {  
            float distance = Vector3.Distance(go.transform.position, this.transform.position);
            if (distance < swarmDistance) {
                NearbyEnemies++;
            }
        }

        navMeshAgent.destination = Target.position;
        // Move to target
        //Vector3 TargetDirection = Target.position - transform.position;
        //TargetDirection = Vector3.Normalize(TargetDirection) * speed; // set the magnitude to 1 then multiply by speed
        //transform.Translate(TargetDirection * Time.deltaTime);
        //}


        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in Enemys) {
            if (go != gameObject) {
                float distance = Vector3.Distance(go.transform.position, this.transform.position);
                if (distance <= SpaceBetween) {
                    Vector3 EnemyDirection = transform.position - go.transform.position;
                    EnemyDirection = Vector3.Normalize(EnemyDirection) * speed;
                    transform.Translate(EnemyDirection * Time.deltaTime);
                }
            }
        }

    }
}