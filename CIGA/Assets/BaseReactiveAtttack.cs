using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseReactiveAtttack : MonoBehaviour {

    public bool isReactiveMelee=false;
    public bool isReactiveRemote = false;
    public GameObject player;

    public abstract void ReactiveMeleeAttack();
    public abstract void ReactiveRemoteAttack();

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(Mathf.Abs(player.transform.position.x)- Mathf.Abs(transform.position.x))<=1.3f)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                isReactiveMelee = true;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                isReactiveRemote = true;
            }
        }

        if (isReactiveMelee)
        {
            ReactiveMeleeAttack();
        }

        if (isReactiveRemote)
        {
            ReactiveRemoteAttack();
        }
    }
}
