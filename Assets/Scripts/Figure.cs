using System;
using UnityEngine;


public class Figure : MonoBehaviour
{
    /// <summary>
    /// Snapshot of Time.time
    /// </summary>
    private float _previousTime;

    // Start is called before the first frame update
    private void Start()
    {
        _previousTime = Time.time;
    }
    
    private void Update()
    {
        InputHandler();

        // If Time.time - previousTime > fallSpeed
        // change figure position down for 1 unit;
        if (Time.time - _previousTime > (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)
                ? TetrisState.FallSpeed / 30 : TetrisState.FallSpeed))
        {
            transform.position += Vector3.down;
            if (!IsValidMove())
            {
                // If we can't fall figure more, it means figure is grounded
                // Spawn new figure and destroy FigureComponent
                transform.position += Vector3.up;
                FigureSpawner spawner = GameObject.FindGameObjectWithTag(FigureSpawner.Tag).GetComponent<FigureSpawner>();
                spawner.SpawnFigure();
                Destroy(GetComponent<Figure>());
            }
            _previousTime = Time.time;

        }
    }

    /// <summary>
    /// Check move for validity
    /// </summary>
    /// <returns>result of checking</returns>
    private bool IsValidMove()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            if (roundedX < 0 || roundedX >= TetrisState.GameWidth || roundedY < 0 || roundedY >= TetrisState.GameHeight)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Handle user inputs
    /// </summary>
    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
            if (!IsValidMove())
                transform.position += Vector3.right;
                        
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if (!IsValidMove())
                transform.position += Vector3.left;
                        
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.forward, 90.0f);
            if (IsValidMove())
            {
                foreach (Transform child in transform)
                {
                    child.Rotate(Vector3.forward, -90.0f);
                }   

            }
            else
            {
                transform.Rotate(Vector3.forward, -90.0f);
            }
        }
    }
}