using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DynamicStage : MonoBehaviour
{
    public int NumberOfStages=1; //sorry guys ive just never used a uint
    private Animator stageAnimator;
    private int currentStageNumber;

    [Header("Debug Stuff")]
    public int Width;
    public int Height;
    public bool DestroyOldStage = false;

    public GameObject stageCube;

    // Start is called before the first frame update
    void Awake()
    {
        stageAnimator = GetComponent<Animator>();
        InputEvents.Instance.RespawnStarted.AddListener(ChangeStage);
    }

    public void ChangeStage()
    {
        if(NumberOfStages > 1)
        {
            int old = currentStageNumber;
            do
            {
                currentStageNumber = Random.Range(1, NumberOfStages + 1);
            }
            while (old == currentStageNumber);
        }
        else
        {
            currentStageNumber = 1;
        }

        //currentStageNumber = Random.Range(1, NumberOfStages + 1);
        stageAnimator.SetInteger("Stage", currentStageNumber);
        stageAnimator.SetTrigger("New Stage");
        
    }

    [Button]
    public void RegenerateStage()
    {
        if(DestroyOldStage)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
        

        for (int x = 0; x< Width; x++)
        {
            for(int y = 0; y< Height; y++)
            {
                if(!DestroyOldStage)
                {
                    if(blockExists(x,y))
                    {//i love nesting
                        continue;
                    }
                }


                GameObject block = Instantiate(stageCube, new Vector3(x, 0, y), Quaternion.identity, this.transform);
                //block.transform.position = position;
                block.transform.localPosition = new Vector3(x, 0, y);
                block.name = "("+x+","+y+")";
            }
        }
        
    }

    /// <summary>
    /// shit code but its not running in runtime so sue me
    /// </summary>
    private bool blockExists(int x, int y)
    {
        GameObject test = GameObject.Find("(" + x + "," + y + ")");

        return test != null;
    }
}
