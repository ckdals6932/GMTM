using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateKey : MonoBehaviour
{
    public GameObject key;

    public List<Transform> keyPositions = new List<Transform>();
    public List<bool> inKey = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        //inkey 리스트 배열 크기 설정 
        for(int j = 0; j < keyPositions.Count; j++)
        {
            inKey.Add(false);
        }

        //열쇠 3개의 대한 for 문
        for(int i =0; i < 4; i++)
        {
            int Rnum =Random.Range(0, keyPositions.Count);

            if(inKey[Rnum])
            {
                i--;
                continue;
            }
            else
            {
                inKey[Rnum] = true;
                Instantiate(key, keyPositions[Rnum]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
