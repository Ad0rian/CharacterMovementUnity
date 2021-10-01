using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    // Use this for initialization
    private Rigidbody2D rb;
    public UnityEngine.Events.UnityEvent m_MyEvent;
    public float speed ;
    void Start () {
         rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {

        rb.velocity = new Vector2(speed,rb.velocity.y);


    }

        void OnTriggerEnter2D(Collider2D player){
            if (player.tag.Equals("Player")){
            m_MyEvent.Invoke();
        }
    }
}
