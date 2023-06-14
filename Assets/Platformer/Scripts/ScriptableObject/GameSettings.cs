using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Game Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private float _speed = 5f;

    public float Speed => _speed;

    public float JumpForce => _speed * 5f;

    public float HurtForce => 3f * _speed;
}