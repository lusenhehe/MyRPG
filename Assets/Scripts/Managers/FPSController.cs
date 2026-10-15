using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float updateInterval = 1f;

    private float _timer;
    private float _averageFramerate;
    private TextMeshProUGUI text => GetComponent<TextMeshProUGUI>();
    void Update()
    {
        float smoothDeltaTime = Time.deltaTime;
        _timer = _timer <= 0 ? updateInterval : _timer -= smoothDeltaTime;

        if(_timer <= 0){
            _averageFramerate = 1 / smoothDeltaTime;
            _averageFramerate = (int)_averageFramerate;
            text.text = "FPS:" + _averageFramerate.ToString();
        } 
    }
}
