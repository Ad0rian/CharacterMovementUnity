using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float speed ;
    void Start () {
         rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {

        rb.velocity = new Vector2(speed,rb.velocity.y);
    }
}
