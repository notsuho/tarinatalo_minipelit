using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField] Material[] materialList;

    void Start()
    {
        if (materialList.Length == 0 || !gameObject.GetComponent<MeshRenderer>())
            return;

        gameObject.GetComponent<MeshRenderer>().material = materialList[Random.Range(0, materialList.Length)];
    }
}
