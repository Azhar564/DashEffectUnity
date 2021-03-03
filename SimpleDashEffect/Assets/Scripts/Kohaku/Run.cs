using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Run : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    public float speed;
    public Transform enemy;

    [Header("Dash Edit")]
    public float dashSpeed;
    public float dashTime;
    public ParticleSystem[] particle;
    [Header("Clone Edit")]
    public SkinnedMeshRenderer[] skinnedMeshes;
    public Material mat;

    private float x, z;
    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        
        Move();
        AttackPoDash();
    }

    void Move(){
        Vector3 move = rotateToMove(x,z);
        Vector3 getMove = move;
        controller.Move(move * Time.deltaTime * speed);

        var Move = x != 0 || z != 0;
        var Idle = z == 0 && x == 0;

        // if (Idle) transform.LookAt(enemy);
        // else if (Move) transform.rotation = Quaternion.LookRotation(move);
        if (Move) {
            getMove = move;
            transform.rotation = Quaternion.LookRotation(getMove); 
        }
        anim.SetBool("Idle", true ? Idle : Move);
        anim.SetBool("Run", true ? Move : Idle);
    }

    void AttackPoDash(){
        
        var click = Input.GetMouseButtonDown(0);
        Vector3 move = rotateToMove(x,z);

        if(click && move.magnitude == 0) anim.SetTrigger("Attack");

        //dash
        
        else if(click) {
            anim.SetTrigger("Attack");
            DashEffect();
            transform.GetChild(0).gameObject.SetActive(false);
            foreach (var item in particle) item.Play();
        }
    }

    IEnumerator Dash(){
        float startTime = Time.time;
        while(Time.time < startTime + dashTime){
            controller.Move(rotateToMove(x,z) * dashSpeed * Time.deltaTime);
            
            yield return null;
        }
        if (Time.time > startTime + dashTime)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            foreach (var item in particle) item.Stop();
        }
    }
    

    Vector3 rotateToMove(float x, float z){
        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        var move = forward * z + right * x;
        if(move.magnitude > 1.2f) move = move * 2/3;
        return move;
    }

    void DashEffect(){
        StartCoroutine(Dash());
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
        Destroy(clone.GetComponent<CharacterController>());
        Destroy(clone.GetComponent<Run>());
        Destroy(clone.GetComponentInChildren<Animator>());
        Destroy(clone.GetComponentInChildren<FUnit.SpringManager>());
        foreach (var item in clone.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            item.material = mat;
            item.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(()=>Destroy(clone));
            
        }
    }
}
