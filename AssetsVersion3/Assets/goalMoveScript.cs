using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalMoveScript : MonoBehaviour
{
    public float moveSpeed = 4;
    public float deadZone = -20;

    public Deplacement player_1;
    public Deplacement player_2;

    public LogicScript logic;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        player_1 = GameObject.FindGameObjectWithTag("Character1").GetComponent<Deplacement>();
        player_2 = GameObject.FindGameObjectWithTag("Character2").GetComponent<Deplacement>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.left * moveSpeed) * Time.deltaTime;
        if (transform.position.x < deadZone) {
            Debug.Log("Goal Deleted");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character1") && player_1.hasTheBall){
            logic.addScore1();
        } else if (collision.gameObject.CompareTag("Character2") && player_2.hasTheBall){
            logic.addScore2();
        }
        Destroy(gameObject);
    }
}
