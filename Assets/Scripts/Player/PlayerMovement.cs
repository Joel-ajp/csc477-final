using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed = 5f;

    private Vector2 _movement;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        _rb.velocity = _movement * _moveSpeed;
    }
}
