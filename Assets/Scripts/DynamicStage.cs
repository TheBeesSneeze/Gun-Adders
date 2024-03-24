using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DynamicStage : MonoBehaviour
{
    private Animator stageAnimator;

    [Header("Debug Stuff")]
    public int Width;
    public int Height;

    public GameObject stageCube;

    // Start is called before the first frame update
    void Start()
    {
        stageAnimator = GetComponent<Animator>();
        InputEvents.Instance.RespawnStarted.AddListener(ChangeStage);
    }

    public void ChangeStage()
    {
        if(Random.value > 0.5f)
        {
            stageAnimator.SetTrigger("A");
        }
        else
        {
            stageAnimator.SetTrigger("B");
        }
        
    }

    [Button]
    public void RegenerateStage()
    {
        while(transform.childCount >0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int x = 0; x< Width; x++)
        {
            for(int y = 0; y< Height; y++)
            {
                GameObject block = Instantiate(stageCube, new Vector3(x, 0, y), Quaternion.identity, this.transform);
                //block.transform.position = position;
                block.transform.localPosition = new Vector3(x, 0, y);
                block.name = "("+x+","+y+")";
            }
        }
        
    }
}
