using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject ladderPrefab;

    public float gapSize = 0.02f;

    const int TileSize = 2;

    const int XTiles = 40;
    const int ZTiles = 40;
    private GameObject[,] world = new GameObject[XTiles, ZTiles];

    // Start is called before the first frame update
    void Start()
    {
        var (worldOffsetX, worldOffsetZ) = ((XTiles + gapSize * XTiles) / 2.0f, (ZTiles + gapSize * ZTiles) / 2.0f);
        CreateFloor(worldOffsetX, -20, worldOffsetZ);
        CreateFloor(worldOffsetX, 0, worldOffsetZ);
        CreateFloor(worldOffsetX, 20, worldOffsetZ);

        void CreateFloor(float worldOffsetX, float worldOffsetY, float worldOffsetZ )
        {
            for (int x = 0; x < XTiles; x++)
            {
                for (int z = 0; z < ZTiles; z++)
                {
                    var xScaled = x * TileSize;
                    var zScaled = z * TileSize;

                    var xGapOffset = gapSize * xScaled;
                    var zGapOffset = gapSize * zScaled;


                    float y = Mathf.PerlinNoise(xScaled / 15.0f, zScaled / 15.0f) * 3.0f - 3.0f;

                    var ladder = Random.Range(0f, 1f) < 0.005;

                    GameObject newTile = Instantiate(ladder ? ladderPrefab : tilePrefab,
                        new Vector3(
                            xScaled + xGapOffset - worldOffsetX,
                            y + worldOffsetY,
                            zScaled + zGapOffset - worldOffsetZ),
                        Quaternion.identity);
                    newTile.transform.parent = transform;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
