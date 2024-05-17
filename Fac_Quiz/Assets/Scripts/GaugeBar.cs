using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    private Slider slider;
    public List<GameObject> toyPrefabs;
    public Transform spawnTransform;
    private float targetValue;
    public float fillSpeed;
    public GameObject conveyor;
    private int toyPrefabNum;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 0f;
        toyPrefabNum = toyPrefabs.Count;
    }

    public void Update()
    {
        if (Mathf.Approximately(slider.value, 1f))
        {
            targetValue = 0f;
            slider.value = 0f;
            MakeToy();
        }
    }

    public void IncreaseGauge(float gaugeIncrease)
    {
        targetValue = slider.value + gaugeIncrease;
        StartCoroutine(FillGauge());
    }

    public void MakeToy()
    {
        int i = Random.Range(0, toyPrefabNum);
        GameObject newToy = Instantiate(toyPrefabs[i], spawnTransform.position, spawnTransform.rotation);
        conveyor.GetComponent<Conveyor>().SetCurrentToy(newToy);
    }

    IEnumerator FillGauge()
    {
        while(slider.value < targetValue)
        {
            slider.value += fillSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
