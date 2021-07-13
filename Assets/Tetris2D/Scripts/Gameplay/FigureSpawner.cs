using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tetris2D
{
    public class FigureSpawner : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Prefab of one tile
        /// </summary>
        [SerializeField]
        private GameObject tile;

        /// <summary>
        /// Container for tetris figure
        /// </summary>
        [SerializeField]
        private GameObject figuresContainer;

        /// <summary>
        /// Getter/setter for GameObject's tag with which is connected FigureSpawner
        /// </summary>
        public static string Tag { get; private set; }

        /// <summary>
        /// Tile ID
        /// </summary>
        private static int tileId = 0;

        /// <summary>
        /// Base for tetris figure
        /// </summary>
        private GameObject _figureBase;

        /// <summary>
        /// Types of all tetris figures
        /// </summary>
        private enum FigureTypes
        {
            O,
            I,
            S,
            Z,
            L,
            J,
            T,
            End
        }

        // available rotations for figures
        private readonly int[] _figureRotations = new int[] {90, 180, 270, 0};
        private readonly int[] _IFigureRotations = new int[] {90, 270, 0};

        #endregion

        #region LifeCycles

        private void Awake()
        {
            Tag = this.gameObject.tag;
        }

        private void Start()
        {
            SpawnFigure();
        }

        #endregion
        
        #region PublicMethods

        /// <summary>
        /// Spawn figure in the current GameObject position
        /// </summary>
        public void SpawnFigure()
        {
            _figureBase = new GameObject() {name = "Figure"};
            _figureBase.transform.parent = figuresContainer.transform;

            _figureBase = CreateFigure(_figureBase, GetRandomFigureType());
            _figureBase.AddComponent<Figure>().Parent = figuresContainer.transform;
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Get random tetris figure type
        /// </summary>
        /// <returns> type of tetris figure </returns>
        private FigureTypes GetRandomFigureType()
        {
            int between = Random.Range(0, ((int)FigureTypes.End));
            return (FigureTypes)between;
        }

        /// <summary>
        /// Create a figure from prefab tile
        /// </summary>
        /// <param name="baseObject">Base object to create figure</param>
        /// <param name="type">Type of tetris figure</param>
        /// <returns>Tetris figure</returns>
        private GameObject CreateFigure(GameObject baseObject, FigureTypes type)
        {
            Color randColor = Random.ColorHSV(0.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f);
            // Random rotation for base figure
            // one exception for I figure, without 180 rotation to don't across top border
            _figureBase.transform.Rotate(
                Vector3.forward,
                type == FigureTypes.I
                    ? _IFigureRotations[Random.Range(0, _IFigureRotations.Length)]
                    : _figureRotations[Random.Range(0, _figureRotations.Length)]
            );
            foreach (Vector3 position in GetTilePositions(type))
            {
                tile.GetComponent<SpriteRenderer>().color = randColor;
                // transform world position to local, cause figure was rotated
                GameObject newTile = Instantiate(
                    tile,
                    baseObject.transform.InverseTransformPoint(position),
                    Quaternion.identity,
                    baseObject.transform
                );
                newTile.name = $"Tile{tileId}";
                ++tileId;
            }

            baseObject.transform.position = transform.position;

            return baseObject;
        }


        /// <summary>
        /// Get tile positions relative to base object
        /// </summary>
        /// <param name="type">type of figure</param>
        /// <returns>Vector3 array with tile positions</returns>
        private IEnumerable<Vector3> GetTilePositions(FigureTypes type)
        {
            return type switch
            {
                FigureTypes.I => new Vector3[] {Vector3.zero, Vector3.up, Vector3.down, new Vector3(0.0f, -2.0f)},
                FigureTypes.J => new Vector3[] {Vector3.zero, Vector3.left, Vector3.right, new Vector3(1.0f, -1.0f)},
                FigureTypes.L => new Vector3[] {Vector3.zero, Vector3.left, Vector3.right, new Vector3(-1.0f, -1.0f)},
                FigureTypes.O => new Vector3[] {Vector3.zero, Vector3.down, new Vector3(-1.0f, -1.0f), Vector3.left},
                FigureTypes.S => new Vector3[] {Vector3.zero, Vector3.right, Vector3.down, new Vector3(-1.0f, -1.0f)},
                FigureTypes.T => new Vector3[] {Vector3.zero, Vector3.left, Vector3.right, Vector3.down},
                FigureTypes.Z => new Vector3[] {Vector3.zero, Vector3.left, Vector3.down, new Vector3(1.0f, -1.0f)},
                _ => new Vector3[] {Vector3.right, Vector3.down, new Vector3(-1.0f, -1.0f)}
            };
        }

        #endregion
    }
}