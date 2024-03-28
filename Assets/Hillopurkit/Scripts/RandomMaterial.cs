using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField] Material[] materialList;

    void Start()
    {
        if (materialList.Length == 0)
        {
            Debug.Log("Jam material list is empty.");
            return;
        }
        
        if(!gameObject.GetComponent<MeshRenderer>())
        {
            Debug.Log("Game object is missing a mesh renderer");
            return;
        }

        gameObject.GetComponent<MeshRenderer>().material = materialList[Random.Range(0, materialList.Length)];
    }
}
