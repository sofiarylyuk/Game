﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public
    public float speed = 1;
    public Vector3 moveBy;
    public bool Boss;

    //private  
    Rigidbody2D myBody = null;
    SpriteRenderer sr = null;
    Animator animator = null;

    Vector3 pointA; //move
    Vector3 pointB;
    float dir = 1; //get direction
    bool toB = true;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        
        pointA = transform.position;
        pointB = pointA + moveBy;
    }

    void FixedUpdate()
    {
        move();
    }

    public virtual void animate() { }
    public virtual void attackHero() { }

    void OnCollisionEnter2D(Collision2D coll)
    {
        attackHero();
    }

    void move()
    {
        sr = GetComponent<SpriteRenderer>();

        float value = getDirection();

        if (Mathf.Abs(value) > 0)
        {
            Vector2 vel = myBody.velocity;
            vel.x = value * speed;
            myBody.velocity = vel;
        }

        if (value < 0)
        {
            sr.flipX = false;
        }
        else if (value > 0)
        {
            sr.flipX = true;
        }
    }

    float getDirection()
    {
        Vector3 my = transform.position;

        if (isAttack())
        {
            if (my.x < Hero.lastHero.transform.position.x)
            {
                return 1;
            }
            else return -1;
        }

        else
        {

            if (toB)
            {
                if (isArrived(my, pointB) == false) dir = 1;
                else
                {
                    dir = -1;
                    toB = false;
                }
            }
            if (!toB)
            {
                if (isArrived(my, pointA) == false) dir = -1;
                else
                {
                    dir = 1;
                    toB = true;
                }
            }
        }
        return dir;
    }

    bool isArrived(Vector3 pos, Vector3 target)
    {
        pos.z = 0;
        target.z = 0;
        return Vector3.Distance(pos, target) <= 0.2f;
    }

    bool isAttack()
    {
        if (Hero.lastHero.transform.position.x > Mathf.Min(pointA.x, pointB.x)
            && Hero.lastHero.transform.position.x < Mathf.Max(pointA.x, pointB.x))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}