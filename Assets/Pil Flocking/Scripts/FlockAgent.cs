using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    public bool dead = false;
    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    public List<Transform> wallPoints = new List<Transform>();

    //public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }



    public void Initialise(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.tag == "Hazard")
        {
            dead = true;
        }
        else if (collision.tag == "Food")
        {
            collision.GetComponent<Food>().Eat();
        }
        */

        switch (collision.tag)
        {
            case "Hazard":
                dead = true;
                break;
            case "Food":
                collision.GetComponent<Food>().Eat();
                break;
            case "Goal":
                /*if (LevelManager.instance.WinConditionMet())
                {
                    collision.GetComponent<Goal>().DisableGoal();
                    LevelManager.instance.NextLevel();
                }*/
                break;
            default:

                break;
        }
    }
}
