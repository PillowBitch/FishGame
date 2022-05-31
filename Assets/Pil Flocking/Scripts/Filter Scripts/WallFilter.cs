using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Wall Filter")]

public class WallFilter : ContextFilter

{

    public LayerMask mask;

    public float angleOffsetChange;

    public float radius;

    public GameObject pointPrefab;

    public GameObject folder;



    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)

    {

        Collider2D[] wallCheck = Physics2D.OverlapCircleAll(agent.transform.position, radius, mask);

        if (wallCheck.Length == 0)

        {

            foreach (Transform point in agent.wallPoints)

            {

                Destroy(point.gameObject);

            }

            agent.wallPoints.Clear();

            return new List<Transform>();

        }

        else

        {

            return CreateWallPoints(agent);

        }



    }



    List<Transform> CreateWallPoints(FlockAgent agent)

    {

        float angle = 0;

        for (int i = 0; i < 360 / angleOffsetChange; i++)

        {

            float x = Mathf.Cos(angle);

            float y = Mathf.Sin(angle);

            angle += 2 * Mathf.PI / (360 / angleOffsetChange);



            Vector3 circlePos = new Vector3(agent.transform.position.x + (100 * x), (agent.transform.position.y + (100 * y)));

            RaycastHit2D circleCheck = Physics2D.Linecast(agent.transform.position, circlePos, mask);



            if (Vector2.SqrMagnitude(circleCheck.point - (Vector2)agent.transform.position) < Mathf.Pow(radius, 2))

            {

                GameObject point = Instantiate(pointPrefab, circleCheck.point, Quaternion.identity);

                point.transform.parent = GameObject.FindGameObjectWithTag("PointFolder").transform;

                agent.wallPoints.Insert(0, point.transform);

            }

            if (agent.wallPoints.Count > 360 / angleOffsetChange)

            {

                Destroy(agent.wallPoints[agent.wallPoints.Count - 1].gameObject);

                agent.wallPoints.RemoveAt(agent.wallPoints.Count - 1);

            }



        }



        return agent.wallPoints;

    }

}
