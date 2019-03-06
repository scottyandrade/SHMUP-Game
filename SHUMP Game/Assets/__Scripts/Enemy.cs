﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   //setting speed
    public float speed = 10f;
    

    private BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();//when an enemy is instantiated, a BoundsCheck component is created and we can access variables from it
    }

    public Vector3 pos
    {
        get => transform.position;//getting the position of the object
        set => transform.position = value;//setting the position of the object
    }
    // Update is called once per frame
    void Update()
    {
        Move();

        if(bndCheck!=null && !bndCheck.isOnScreen)
        {
            if (pos.y < bndCheck.camHeight - bndCheck.radius)
            {
                Destroy(gameObject);//destroying the game object once it has left the screen
            }
        }
    }

    public virtual void Move()//creating a virtual void method to be used the enemy child classes
    {
        

    }
}