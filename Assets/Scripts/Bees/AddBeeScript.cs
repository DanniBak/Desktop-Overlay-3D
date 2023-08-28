using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBeeScript : MonoBehaviour
{

    public GameObject bee;
    public Transform parent;
    public List<GameObject> beeList = new List<GameObject>();
    public int initialBees = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < initialBees; i++)
        {
            InstantiateNewBee();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateNewBee()
    {
        GameObject newBee = Instantiate(bee,parent.transform.position + Random.insideUnitSphere * 10, Quaternion.identity, parent);
        beeList.Add(newBee);
    }

    public void RemoveBee()
    {
        if (beeList.Count > 0)
        {
            Destroy(beeList[0]);
            beeList.RemoveAt(0);
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
