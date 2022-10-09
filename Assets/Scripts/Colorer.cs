using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        float val = Random.Range(0.2f, .3f);
        sr.color = new Color(val,val,val);
    }
}
