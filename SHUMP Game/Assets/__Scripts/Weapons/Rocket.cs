﻿using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    private Transform _rocketTarget;
    public Rigidbody rocketRigidBody;
 //   static public float TIMER;
    //public static bool CHECK;
    public float turn;
    public float rocketVelocity;
    // Update is called once per frame
   private void FixedUpdate()
    {
        if (FindClosestByTag("Enemy") == null && FindClosestByTag("EnemyBoss") == null)
        {
            rocketRigidBody.velocity = transform.forward * rocketVelocity;
        }
        else
        {
            if (FindClosestByTag("Enemy") == null)
            {
                _rocketTarget = FindClosestByTag("EnemyBoss").transform;
            }

            else if (FindClosestByTag("EnemyBoss") == null)
            {
                _rocketTarget = FindClosestByTag("Enemy").transform;
            }
            else if (Vector3.Distance(rocketRigidBody.position, FindClosestByTag("EnemyBoss").transform.position) < Vector3.Distance(rocketRigidBody.position, FindClosestByTag("Enemy").transform.position))
            {
                _rocketTarget = FindClosestByTag("EnemyBoss").transform;
            }
            else
            {
                _rocketTarget = FindClosestByTag("Enemy").transform;
            }
            rocketRigidBody.velocity = transform.forward * rocketVelocity;
            var rocketTargetRotation = Quaternion.LookRotation(_rocketTarget.position - transform.position);
            rocketRigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rocketTargetRotation, turn));
        }
    }

    GameObject FindClosestByTag(string tag)
    {
        GameObject[] gameObj;
        gameObj = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gameObj)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

}

