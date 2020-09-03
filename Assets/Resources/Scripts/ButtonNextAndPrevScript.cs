using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNextAndPrevScript : MonoBehaviour
{
    private static GameObject buttonNext, buttonPrev;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickNext()
    {
        //Debug.Log("PressedNext");
        buttonNext = gameObject;
        KeyValuePair<int, int> pair = SelectPhoto.nextPage();
        if (pair.Value > 1) 
        {
            if(buttonPrev != null)
                buttonPrev.SetActive(true);
            if (pair.Value == pair.Key)
                buttonNext.SetActive(false);
        }
    }

    public void clickPrev()
    {
        buttonPrev = gameObject;
        //Debug.Log("PressedPrev");
        if (SelectPhoto.prevPage() == 1)
        {
            buttonPrev.SetActive(false);
        }
        if (buttonNext != null)
            buttonNext.SetActive(true);
    }

    public void ripetiRilevamento()
    {
        SelectPhoto.ripetiProcessoRilevamento();
    }

    public void back()
    {
        DetectGender.back();
    }

    public void detect()
    {
        DetectGender.detect();
    }
}
