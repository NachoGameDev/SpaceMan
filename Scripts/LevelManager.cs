using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager singleton;
    public List<scrLevelBlock> allTheLevelBlocks = new List<scrLevelBlock>();
    public List<scrLevelBlock> currentLevelBlocks = new List<scrLevelBlock>();
    public Transform levelStartPosition;

    void Awake()
    {
        if(singleton == null){
            singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateInitialBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLevelBlock(){
        int randomIdx = Random.Range(0,allTheLevelBlocks.Count);

        scrLevelBlock block;

        Vector3 spawnPosition = Vector3.zero;

        if(currentLevelBlocks.Count == 0){
            block = Instantiate(allTheLevelBlocks[0]);
            spawnPosition = levelStartPosition.position;
        }
        else{
            block = Instantiate(allTheLevelBlocks[randomIdx]);
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count-1].EndPoint.position;
        }

        //todos los bloques quedan como hijos de este manager
        block.transform.SetParent(this.transform,false);

        Vector3 correction = new Vector3(
            spawnPosition.x - block.startPoint.position.x,
            spawnPosition.y - block.startPoint.position.y,
            0
        );

        block.transform.position = correction;

        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock(){
        scrLevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    public void RemoveAllLevelBlocks(){
        while(currentLevelBlocks.Count > 0 ){
            RemoveLevelBlock();
        }
    }

    public void GenerateInitialBlocks(){
        for (int i = 0; i < 3; i++){
            AddLevelBlock();
        }
    }
}
