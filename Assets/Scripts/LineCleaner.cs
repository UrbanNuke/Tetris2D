using UnityEngine;

public class LineCleaner : MonoBehaviour
{
    /// <summary>
    /// Getter/setter for GameObject's tag with which is connected LineCleaner
    /// </summary>
    public static string Tag { get; private set; }
    private void Awake()
    {
        Tag = this.gameObject.tag;
    }

    /// <summary>
    /// Clear all full lines on the field
    /// </summary>
    public void ClearFullLines()
    {
        for (int y = TetrisState.GameHeight - 1; y > -1 ; --y)
        {
            if (IsFullLine(y))
            {
                DestroyLine(y);
                DropBlocksAbove(y);
            }
        }
    }

    /// <summary>
    /// Check if it's a full line
    /// </summary>
    /// <param name="y">Y coord of line</param>
    /// <returns>Result of checking</returns>
    private bool IsFullLine(int y)
    {
        for (int x = 0; x < TetrisState.GameWidth; ++x)
        {
            if (TetrisState.Grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Destroy line objects from scene
    /// </summary>
    /// <param name="y">Y coord of line</param>
    private void DestroyLine(int y)
    {
        for (int x = 0; x < TetrisState.GameWidth; ++x)
        {
            Destroy(TetrisState.Grid[x, y].gameObject);
        }
    }

    /// <summary>
    /// Drop blocks are which above on the deleted line
    /// </summary>
    /// <param name="yCoordOfDeletedLine">Y coord of deleted line</param>
    private void DropBlocksAbove(int yCoordOfDeletedLine)
    {
        for (int y = yCoordOfDeletedLine + 1; y < TetrisState.GameHeight - 1; ++y)
        {
            for (int x = 0; x < TetrisState.GameWidth; ++x)
            {
                if (TetrisState.Grid[x, y] == null) continue;
                TetrisState.Grid[x, y].position += Vector3.down;
                TetrisState.Grid[x, y - 1] = TetrisState.Grid[x, y];
                TetrisState.Grid[x, y] = null;
            }
        }
    }
}