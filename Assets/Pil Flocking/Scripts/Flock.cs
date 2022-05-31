using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flock : MonoBehaviour
{
    [Header("Player control")]
    public bool isPlayer = false;
    public int failCount = 0;
    public GameObject editorVisualiser;
    
    [Header("Technical stuff")]
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    public StayInRadiusBehaviour chaseBehaviour;

    public Transform center;

    [Range(10, 500)]
    public int startingCount = 250;
    public float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    [Header("Vignette")]
    public Image vignette;
    public float transparencyFactor = 1;
    public float vignetteMinimum = 0;
    public float dangerRatio = 0.5f;

    [Header("Fish Counter UI")]
    public Slider fishCounter;

    // Start is called before the first frame update
    void Start()
    {
        if(editorVisualiser != null)
            editorVisualiser.gameObject.SetActive(false);

        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;



        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                (Vector2)transform.position + (Random.insideUnitCircle * startingCount * AgentDensity),
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialise(this);
            agents.Add(newAgent);
        }

        if (vignette != null)
        {
            Color c = vignette.color;
            c.a = 0;
            vignette.color = c;
        }


        if(chaseBehaviour != null)
        {
            if (center != null) 
            {
                chaseBehaviour.SetCenter((Vector2)center.position); 
            }
            else
            {
                chaseBehaviour.SetCenter();
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        //Iterate through all agents. Using for instead of foreach to be able to remove agents at specific places in the list.
        for (int i = 0; i < agents.Count; i++)
        {
            //Checks to see if the current agent is dead, if so it removes it in the else statement below.
            if (!agents[i].dead)
            {
                //Logic to calculate the agents movement based on the specific behaviours selected in the editor.
                List<Transform> context = GetNearbyObjects(agents[i]);

                Vector2 move = behaviour.CalculateMove(agents[i], context, this);
                move *= driveFactor;
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                agents[i].Move(move);
            }
            else
            {
                Destroy(agents[i].gameObject);
                agents.RemoveAt(i);

                //It will only alert the player of an agents deletion if the agent is part of the player controlled flock.
                if (isPlayer)
                {
                    AudioController.PlayAudio("Crunch");

                    Color c = vignette.color;
                    if (c.a < 0.8f)
                    {
                        c.a += 0.25f;
                    }
                    vignette.color = c;
                }

            }
        }

        
        dangerRatio = ((float)agents.Count - (float)failCount) / ((float)startingCount - (float)failCount);
        
        //Debug.Log(agents.Count + " - " + failCount + " / (" + startingCount + " - " + failCount + ") = " + dangerRatio);
        //float lol = 1 - Mathf.Clamp(dangerRatio, 0.8f, 1.0f);
        //Debug.Log(1 - dangerRatio + " Clamped: " + lol);
        if(fishCounter != null)
            fishCounter.value = dangerRatio;
        if (vignette != null) 
        {
            if (vignette.color.a > 0 && isPlayer)
            {
                Color c = vignette.color;
                c.a -= Time.deltaTime * transparencyFactor;
                vignette.color = c;
            }
        }


        if (agents.Count < failCount + 1 && isPlayer && GameManager.instance.gameState == GameState.Play)
        {
            GameManager.instance.Failure();
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);
        foreach(Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider && !c.isTrigger)
            {
                context.Add(c.transform);
            }
        }
        return context; 
    }

    private void OnDrawGizmosSelected()
    {
        editorVisualiser.transform.localScale = new Vector3(2 * (1 + (startingCount * AgentDensity)),2 * (1 + (startingCount * AgentDensity)), 1);
    }
}
