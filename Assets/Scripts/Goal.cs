using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Collider2D goalCollider;
    public Collider2D GoalCollider { get { return goalCollider; } }

    public void DisableGoal()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && LevelManager.instance.WinConditionMet())
        {
            this.gameObject.SetActive(false);
            LevelManager.instance.NextLevel();
        }
    }
}
