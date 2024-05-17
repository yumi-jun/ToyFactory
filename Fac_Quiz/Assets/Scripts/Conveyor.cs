using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float speed;
    private Rigidbody rBody;
    private bool isMoving = false;
    private GameObject currentToy = null;
    [SerializeField]
    private Transform targetTransform;
    private Material mat;
    private int toyNum = 0;
    [SerializeField]
    private QuizSystem quizSystem;
    [SerializeField]
    private TMP_Text toyStateText;


    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().sharedMaterial;
        mat.SetFloat("_Speed", 0);
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 pos = rBody.position;
            rBody.position += Vector3.left * speed * Time.fixedDeltaTime;
            rBody.MovePosition(pos);
        }
    }

    public GameObject GetCurrentToy()
    {
        return currentToy;
    }

    public void SetCurrentToy(GameObject toy)
    {
        currentToy = toy;
        isMoving = true;
        mat.SetFloat("_Speed", speed);
    }

    //Àå³­°¨ È¹µæ
    public void GetToy(GameObject toy)
    {
        if (currentToy != null)
        {
            Destroy(toy);
            mat.SetFloat("_Speed", 0);
            toyNum++;
            toyStateText.text = toyNum + "/" + quizSystem.GetTargetToyNum();

            if (toy == currentToy)
            {
                currentToy = null;
                isMoving = false;

                if (quizSystem.GetIsLastQuiz())
                {
                    if (toyNum == quizSystem.GetTargetToyNum())
                        Debug.Log("¼º°ø!");
                    else
                        Debug.Log("½ÇÆÐ!");
                }
            }
        }        
    }

    public int GetToyNum()
    {
        return toyNum;
    }

}
