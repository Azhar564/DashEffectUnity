using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAI : MonoBehaviour
{
    public Transform player;
    public CharacterController controller;
    public float speed;
    public Animator anim;
    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        IntoPlayer();
    }

    void IntoPlayer(){
        float dis = Vector3.Distance(transform.position, player.position);
        int someValue = Random.Range(0,2)*2-1;
        var move = transform.forward * Time.deltaTime * speed;
        if (move.magnitude > 1.2) move = move * 2/3;
        if(dis<2)
        {
            transform.LookAt(player);
            controller.Move(move);
        }
        float overallSpeed = controller.velocity.magnitude;

        var Move = overallSpeed > 0;
        var Idle = overallSpeed == 0;
        
        anim.SetBool("Run", true ? Move : Idle);
    }

    void RotatePlayer(){

        float dis = Vector3.Distance(transform.position, player.position);
        int someValue = Random.Range(0,2)*2-1;
        var move = transform.forward * Time.deltaTime * speed;
        if (move.magnitude > 1.2) move = move * 2/3;
        if(dis>1)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime);

            controller.Move(move);
        }
        float overallSpeed = controller.velocity.magnitude;

        var Move = overallSpeed > 0;
        var Idle = overallSpeed == 0;
        
        anim.SetBool("Run", true ? Move : Idle);
    }
}
