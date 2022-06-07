using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : FSMState
{
    public override void OnUpdate()
    {
        Vector3 targetPos = new Vector3((Parent as Actor).targetPosition.x, 0, (Parent as Actor).targetPosition.y);
        if (Vector3.Distance(Parent.transform.position, targetPos) > float.Epsilon)
        {
            Parent.transform.position = Vector3.MoveTowards(Parent.transform.position, targetPos, Time.deltaTime * (Parent as Actor).MoveSpeed);
        }
        else
        {
            (Parent as Actor).EnterTile((Parent as Actor).targetPosition);
            (Parent as Actor).EndTurn();
        }
    }
}
