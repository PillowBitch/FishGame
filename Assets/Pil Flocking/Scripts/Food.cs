using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Food : MonoBehaviour
{
    [Header("Food Colour")]
    public Color colour = Color.white;

    [Header("Particles")]
    public ParticleSystem ps;
    public float particleCount = 10f;

    float defParticleCount;

    ParticleSystem.EmissionModule psEmission;

    [Header("Food Logic")]
    public float foodCount = 100;
    public float eatRateFactor = 1;

    public float eaten = 0f;

    public float radius = 2;

    float defFoodCount;

    public float soundCountDown = 0.1f;
    float soundCountDownDef;
    [Header("Collider")]
    public CircleCollider2D hitBox;

    bool isDead;


    public void Start()
    {
        LevelManager.instance.AddToList(this);

        Debug.Log(this + " is in list.");

        if (hitBox == null)
            hitBox = GetComponent<CircleCollider2D>();
        if (ps == null)
            ps = GetComponentInChildren<ParticleSystem>();

        hitBox.radius = radius;
        ParticleSystem.ShapeModule psShape = ps.shape;
        psShape.radius = radius;

        defFoodCount = foodCount;

        defParticleCount = particleCount;

        psEmission = ps.emission;

        soundCountDownDef = soundCountDown;
        soundCountDown = 0;
    }

    public void Update()
    {
        if(GameManager.instance.gameState == GameState.Play)
            Eat();
        if (soundCountDown > 0)
            soundCountDown -= Time.deltaTime;
    }

    public void Eat()
    {
        Collider2D[] eating = Physics2D.OverlapCircleAll((Vector2)transform.position, radius);
        //Debug.Log("Number of agents eating: " + (eating.Length - 1));
        int previousFood = (int)foodCount;
        if(foodCount > 0)
        {
            foodCount -= (eating.Length - 1) * eatRateFactor * Time.deltaTime;
            eaten = defFoodCount - foodCount;
            particleCount = (defParticleCount * (foodCount / defFoodCount));
            psEmission.rateOverTime = particleCount;
        }
        else if (!isDead)
        {
            isDead = true;
            foodCount = 0;
            eaten = defFoodCount;
            particleCount = 0;
            psEmission.rateOverTime = 0;
            //LevelManager.instance.AddFood(defFoodCount);
        }
        if ((int)foodCount != previousFood && soundCountDown <= 0)
        {
            AudioController.PlayAudio("Pop");
            soundCountDown = soundCountDownDef;
        }

    }


    void OnDrawGizmosSelected()
    {
        hitBox.radius = radius;
        ParticleSystem.MainModule psMain = ps.main;
        ParticleSystem.ShapeModule psShape = ps.shape;
        psEmission = ps.emission;
        psShape.radius = radius;
        psEmission.rateOverTime = particleCount;
        psMain.startColor = colour;


        



        //Debug.Log("ps.shape.radius = " + ps.shape.radius);
        //Debug.Log("psShape.radius = " + psShape.radius);
    }

}
