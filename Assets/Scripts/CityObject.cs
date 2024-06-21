using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityObject : MonoBehaviour
{
    City city;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCity(City city)
    {
        this.city = city;
    }

    public City GetCity()
    {
        return this.city;
    }
}
