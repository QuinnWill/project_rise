using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public GameObject SpaceShip;

    public GameObject CurvePoint;

    public Material StartEndMaterial;

    [Range(0,1)]
    public float Speed = 0.2f;

    GameObject _ship;

    GameObject _startPosition;
    GameObject _midPosition;
    GameObject _endPosition;

    float _startTime;

    // Start is called before the first frame update
    void Start()
    {
        _ship = Instantiate(SpaceShip, transform);

        _startPosition = Instantiate(CurvePoint, transform);
        _startPosition.name = "StartPosition";

        _midPosition = Instantiate(CurvePoint, transform);
        _midPosition.name = "MidPosition";
        _midPosition.transform.rotation = Random.rotation;

        _endPosition = Instantiate(CurvePoint, transform);
        _endPosition.name = "EndPosition";
        _endPosition.transform.rotation = Random.rotation;

        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_ship)
        {
            Destroy(gameObject);
        }
        float deltaTime = Time.time - _startTime;
        Quaternion Q1, Q2;
        //SpaceShip.transform.rotation = Quaternion.Euler(Vector3.Lerp(_startPosition.transform.rotation.eulerAngles, _endPosition.transform.rotation.eulerAngles, deltaTime * Speed));
        
        Q1 = Quaternion.Slerp(_startPosition.transform.rotation, _midPosition.transform.rotation, deltaTime * Speed);

        Q2 = Quaternion.Slerp(_midPosition.transform.rotation, _endPosition.transform.rotation, deltaTime * Speed);

        _ship.transform.rotation = Quaternion.Slerp(Q1, Q2, deltaTime * Speed);

        if (deltaTime * Speed > 1)
        {
            _startTime = Time.time;
        }
    }
}
