using UnityEngine;

public class ChargeEffectScript : MonoBehaviour
{
    public LancerScript lancerscript;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = lancerscript.transform.position.x + offset.x;
        transform.position = newPosition;
    }

    public void SetLancerScript(LancerScript lancer)
    {
        lancerscript = lancer;
    }
}
