using UnityEngine;

public class BloodBG : MonoBehaviour
{
    public GameObject[] bloodDecals;
    void OnParticleCollision() {
        GameObject bloodDecal = Instantiate(bloodDecals[Random.Range(0, bloodDecals.Length-1)], transform.position, Quaternion.identity);
        int size = Random.Range(5, 10);
        bloodDecal.transform.localScale = new Vector3(size,size,size);
    }
}
