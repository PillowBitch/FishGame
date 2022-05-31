using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay in Radius")]
public class StayInRadiusBehaviour : FlockBehaviour
{
    public bool targeting = false;
    public Vector2 center;
    public float radius = 15f;

    public void SetCenter()
    {
        center = Vector2.zero;
    }

    public void SetCenter(Vector2 c)
    {
        center = c;
    }

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (targeting && Input.GetMouseButton(0) && GameManager.instance.gameState == GameState.Play)
        {
            //center = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            center = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude / radius;
        if(t < 0.9f)
        {
            return Vector2.zero;
        }

        return centerOffset * t * t;
    }
} 
