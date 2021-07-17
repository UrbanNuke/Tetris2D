using UnityEngine;

namespace Tetris2D
{
    public class Figure : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Snapshot of Time.time for falling
        /// </summary>
        private float _previousFallTime;
        
        /// <summary>
        /// Snapshot of Time.time for shifting
        /// </summary>
        private float _previousShiftTime;

        #endregion

        #region Properties

        /// <summary>
        /// Parent for tiles, when figure was grounded
        /// </summary>
        public Transform Parent { get; set; }

        #endregion

        #region LifeCycles

        private void Start()
        {
            _previousFallTime = Time.time;
        }

        private void Update()
        {
            InputHandler();
        }

        #endregion

        #region PrivateMethods

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

                if (roundedX < 0 || roundedX >= GameManager.GameWidth || roundedY < 0 || roundedY >= GameManager.GameHeight)
                {
                    return false;
                }

                if (GameManager.Instance.Grid[roundedX, roundedY] != null)
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
            if (GameManager.Instance.GameState != GameState.Play) return;
            
            if (Time.time - _previousShiftTime > GameManager.Instance.ShiftSpeed 
                && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                transform.position += Vector3.left;
                if (!IsValidMove())
                    transform.position += Vector3.right;
                _previousShiftTime = Time.time;
            }

            if (Time.time - _previousShiftTime > GameManager.Instance.ShiftSpeed 
                && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                transform.position += Vector3.right;
                if (!IsValidMove())
                    transform.position += Vector3.left;
                _previousShiftTime = Time.time;
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
            
            
            // If Time.time - previousTime > fallSpeed
            // change figure position down for 1 unit;
            if (Time.time - _previousFallTime > (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)
                ? GameManager.Instance.ForceFallSpeed
                : GameManager.Instance.FallSpeed))
            {
                transform.position += Vector3.down;
                if (!IsValidMove())
                {
                    // If we can't fall figure more, it means figure is grounded
                    // Clear lines, spawn new figure and destroy this GameObject
                    transform.position += Vector3.up;
                    AddToGrid();
                    Destroy(this.gameObject);
                    LineCleaner lineCleaner = GameObject.FindWithTag(LineCleaner.Tag).GetComponent<LineCleaner>();
                    lineCleaner.ClearFullLines();

                    if (GameManager.Instance.GameState == GameState.Play)
                    {
                        FigureSpawner spawner = GameObject.FindGameObjectWithTag(FigureSpawner.Tag).GetComponent<FigureSpawner>();
                        spawner.SpawnFigure();
                    }

                }

                _previousFallTime = Time.time;
            }
        }

        /// <summary>
        /// Add each block of figure to tetris grid.
        /// </summary>
        private void AddToGrid()
        {
            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(transform.childCount - 1);
                int roundedX = Mathf.RoundToInt(child.transform.position.x);
                int roundedY = Mathf.RoundToInt(child.transform.position.y);

                child.SetParent(Parent);
                GameManager.Instance.Grid[roundedX, roundedY] = child;
            }
        }

        #endregion
    }
}