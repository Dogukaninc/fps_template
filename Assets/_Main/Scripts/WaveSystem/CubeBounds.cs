using UnityEngine;

[ExecuteAlways]
public class CubeBounds : MonoBehaviour
{
    public Vector3 size = new Vector3(5, 2, 5);

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.2f); // yarı saydam gövde
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.up * (size.y * 0.5f), size);

        Gizmos.color = Color.cyan; // kenarlık
        Gizmos.DrawWireCube(Vector3.up * (size.y * 0.5f), size);
    }

    /// <summary>
    /// Taban (Y=0) üzerinden world-space'te rastgele bir nokta döndürür.
    /// </summary>
    public Vector3 GetRandomGroundPoint()
    {
        float x = Random.Range(-size.x * 0.5f, size.x * 0.5f);
        float z = Random.Range(-size.z * 0.5f, size.z * 0.5f);
        Vector3 local = new Vector3(x, 0f, z);
        return transform.TransformPoint(local);
    }
}