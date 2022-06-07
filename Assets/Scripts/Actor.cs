using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MapObject
{

    public Vector2Int targetPosition;

    protected int maxHealth;
    public int MaxHealth { get { return maxHealth; } }

    protected int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }


    public float MoveSpeed = 5f;
    public int InitialHealth;

    public IdleState IdleState = new IdleState();
    public MovingState MovingState = new MovingState();
    public DeadState DeadState = new DeadState();
    public AttackingState AttackState = new AttackingState();

    public Actor attackTarget;
    public float attackDuration = 0.5f;
    public float attackStartTime;


    protected virtual void Awake()
    {
        IdleState.Parent = this;
        MovingState.Parent = this;
        AttackState.Parent = this;
        DeadState.Parent = this;

        AddState(IdleState);
        AddState(MovingState);
        AddState(AttackState);
        AddState(DeadState);
    }

    public void SetPosition(Vector2Int _position)
    {
        TilePosition = _position;
        transform.position = new Vector3(TilePosition.x, 0, TilePosition.y);
    }

    public virtual bool CanMoveToTile(Vector2Int _position)
    {

        if (CurrentState != IdleState || _position.x < 0 || _position.y < 0
        || _position.x >= DungeonController.Instance.CurrentRoom.Size.x
        || _position.y >= DungeonController.Instance.CurrentRoom.Size.y)
        {
            return false;
        }

        return true;
    }


    public virtual void BeginMove(Vector2 _direction)
    {
        Vector2Int direction = new Vector2Int((int)_direction.x, (int)_direction.y);
        Vector2Int position = TilePosition + direction;

        if (!CanMoveToTile(position))
        {
            return;
        }

        Tile tile = DungeonController.Instance.GetTile(position);
        if (tile != null)
        {
            if (tile.IsPassable())
            {
                for (int i = 0; i < tile.MapObjects.Count; i++)
                {
                    if (tile.MapObjects[i].GetType() == typeof(Door))
                    {
                        GoToState(MovingState);
                        targetPosition = position;
                    }
                }

                if (CurrentState == IdleState)
                {
                    GoToState(MovingState);
                    targetPosition = position;
                }
            }
            else
            {
                // handle what is blocking movement
                for (int i = 0; i < tile.MapObjects.Count; i++)
                {
                    if (tile.MapObjects[i].GetType() == typeof(Monster))
                    {
                        attackStartTime = Time.time;
                        GoToState(AttackState);
                        attackTarget = tile.MapObjects[i] as Monster;
                    }
                }
            }
        }
    }

    public virtual void BeginAttack(Vector2 _direction)
    {
        Vector2Int direction = new Vector2Int((int)_direction.x, (int)_direction.y);
        Vector2Int position = TilePosition + direction;

        if (!CanMoveToTile(position))
        {
            return;
        }

        bool hasPlayer = false;
        for (int i = 0; i < DungeonController.Instance.GetTile(position).MapObjects.Count; i++)
        {
            if (DungeonController.Instance.GetTile(position).MapObjects[i].GetType() == typeof(Player))
            {
                hasPlayer = true;
            }
        }
        if (!hasPlayer)
        {
            return;
        }

        GameController.Instance.Player.TakeDamage(GetAttackDamage());
    }

    public virtual void EndTurn()
    {
        GoToState(IdleState);
    }

    public virtual int GetAttackDamage()
    {
        return 5;
    }

    public virtual void TakeDamage(int _amount)
    {
        int totalDamage = _amount;
        if (_amount >= currentHealth)
        {
            totalDamage = currentHealth;
        }
        currentHealth -= totalDamage;

        UIController.Instance.ShowDamageTag(transform.position, totalDamage.ToString());
        if (currentHealth <= 0)
        {
            OnKill();
        }
    }

    public virtual void OnKill()
    {
        DungeonController.Instance.GetTile(TilePosition).MapObjects.Remove(this);
        GoToState(DeadState);
        Destroy(gameObject);
    }

    public virtual void EnterTile(Vector2Int _tilePosition)
    {
        DungeonController.Instance.GetTile(_tilePosition).MapObjects.Remove(this);
        TilePosition = targetPosition;
        DungeonController.Instance.GetTile(_tilePosition).MapObjects.Add(this);
    }
}
