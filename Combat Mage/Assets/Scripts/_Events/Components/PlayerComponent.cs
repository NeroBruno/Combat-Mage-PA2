using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : LivingEntityComponent
{
    private Player _Player;

    public Player Player
    {
        get
        {
            if (!_Player)
                _Player = GetComponent<Player>();
            if (!_Player)
                _Player = GetComponentInParent<Player>();

            return _Player;
        }
    }
}
