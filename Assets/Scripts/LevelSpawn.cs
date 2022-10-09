using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawn : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    private Vector3 lastSpawnedWall;
    private int lastX;
    private bool spawnUpwards = false;
    private int objsThisWay;
    [SerializeField] private float maxUp = -0.05f;
    [SerializeField] private float maxDown = -4.95f;
    [SerializeField] private CopterMovement player;

    private void Awake()
    {
        maxDown = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + 0.05f;
        maxUp = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y- 5.05f; 
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnUpwards = false;
        //spawnWalls through the whole screen
        SpawnWalls(new Vector3(15, -2.5f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.transform.position.x+12 > lastX)
        {
            //If we've spawned enough objects
            if (objsThisWay < 1)
            {
                objsThisWay = Random.Range(5, 25);
                //flip SpawnUpwards
                spawnUpwards = !spawnUpwards;
            }

            //Randomly determine how much we are going up or down
            float vertPos = Random.Range(1, 10);
            if (!spawnUpwards) { vertPos *= -1f; }

            lastX = Mathf.RoundToInt(lastSpawnedWall.x);
            SpawnWalls(new Vector3(lastX+1, lastSpawnedWall.y + (vertPos*0.1f), 0));
            objsThisWay--;
        }
    }

    private void SpawnWalls(Vector3 pos)
    {
        player.AddPoints(1);
        if (pos.y > maxUp || pos.y < maxDown)
        {
            if (pos.y > maxUp)
                pos.y = maxUp;
            else if (pos.y < maxDown)
                pos.y = maxDown;
        }

        GameObject wallBit = Instantiate(wallPrefab, pos, Quaternion.identity);
        lastSpawnedWall = wallBit.transform.position;
        StartCoroutine(DestroyerOfWalls(wallBit,7));
    }

    private IEnumerator DestroyerOfWalls(GameObject spawnedObj, float time)
    {
        yield return new WaitForSeconds(time);
        if (player.gameGoing)
            Destroy(spawnedObj, 7);
    }
    
}
