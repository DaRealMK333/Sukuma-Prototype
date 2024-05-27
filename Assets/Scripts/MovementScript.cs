using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveBeadsUp(Rigidbody2D Bead)
    {
        Bead.velocity += new Vector2(0, 1);
    }

    public void MoveBeadDown(Rigidbody2D Bead)
    {
        Bead.velocity += new Vector2(0, -1);
    }

    public void MoveBeadRight(Rigidbody2D Bead)
    {
        Bead.velocity += new Vector2(1, 0);
    }

    public void MoveBeadLeft(Rigidbody2D Bead)
    {
        Bead.velocity += new Vector2(-1, 0);
    }


}