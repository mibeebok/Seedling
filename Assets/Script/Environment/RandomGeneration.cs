using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RandomGeneration : MonoBehaviour
{
    public GameObject[] objects;
    public static HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();
    
    public bool IsGenerationComplete { get; private set; } = false;
    
    IEnumerator Start()
    {
        IsGenerationComplete = false;
        occupiedPositions = new HashSet<Vector2Int>(); 
        
        yield return new WaitUntil(() => FarmGrid.Instance != null && FarmGrid.Instance.isGridGenerated);
        
        int rand = Random.Range(0, objects.Length);
        Vector3 pos = transform.position;
        pos.z = 0;
        GameObject spawned = Instantiate(objects[rand], pos, Quaternion.identity);
        spawned.tag = "Obstacle";
        
        Vector2Int gridPos = FarmGrid.Instance.WorldToGridPosition(spawned.transform.position);
        occupiedPositions.Add(gridPos);
        
        IsGenerationComplete = true;
    }
}