using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : FSMState
{
    public override void OnUpdate()
    {
        Vector3 attackPos = new Vector3((Parent as Actor).attackTarget.TilePosition.x, 0, (Parent as Actor).attackTarget.TilePosition.y);
        float t = (Time.time - (Parent as Actor).attackStartTime) / (Parent as Actor).attackDuration;
        Vector3 attackDir = attackPos - Parent.transform.position;
        Parent.transform.position = DungeonController.Instance.CurrentRoom.Tiles[(Parent as Actor).TilePosition.x, (Parent as Actor).TilePosition.y].gameObject.transform.position + attackDir * Mathf.PingPong(t, 0.5f);
        if (t > 1f)
        {
            int attackDamage = (Parent as Actor).GetAttackDamage();
            (Parent as Actor).attackTarget.TakeDamage(attackDamage);
            (Parent as Actor).EndTurn();
        }
    }
}
