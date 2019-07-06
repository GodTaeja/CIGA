using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTestGOEvent : BaseReactiveAtttack
{
 

    public override void ReactiveMeleeAttack()
    {
           Vector3 pos= player.transform.localScale.x < 0 ? transform.position =
            new Vector3(transform.position.x - 1, transform.position.y, transform.position.z) 
            : transform.position = new Vector3(transform.position.x + 1, transform.position.y, 
            transform.position.z);
        isReactiveMelee = false;
    }

    public override void ReactiveRemoteAttack()
    {
        Debug.Log("remote");
        isReactiveRemote = false;
    }
}
