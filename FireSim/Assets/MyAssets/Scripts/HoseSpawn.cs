/// <summary>
/// This script is deprecated, switch to using the Obi Rope Asset
/// </summary>

using UnityEngine;

public class HoseSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject partPrefab, parentObject;

    [SerializeField]
    [Range(1, 1000)]
    private int length = 1;

    [SerializeField]
    private float partDistance = 0.21f;

    [SerializeField]
    bool reset, spawn, snapFirst, snapLast;

    private void Update()
    {
        if(reset)
        {
            foreach(GameObject temp in GameObject.FindGameObjectsWithTag("Hose"))
            {
                Destroy(temp);
            }

            reset = false;
        }

        if(spawn)
        {
            Spawn();

            spawn = false;
        }
    }

    public void Spawn()
    {
        int count = (int)(length / partDistance);

        for(int i = 0; i < count; i++)
        {
            GameObject temp;

            temp = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y + partDistance * (i+1), transform.position.z), Quaternion.identity, parentObject.transform);
            temp.transform.eulerAngles = new Vector3(180, 0, 0);

            temp.name = parentObject.transform.childCount.ToString();

            if(i == 0)
            {
                Destroy(temp.GetComponent<CharacterJoint>());
            }
            else
            {
                temp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }
        }
    }
}
