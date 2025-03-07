using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void HelloString(string str);

    [DllImport("__Internal")]
    private static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    private static extern int AddNumbers(int x, int y);

    [DllImport("__Internal")]
    private static extern string StringReturnValueFunction();

    [DllImport("__Internal")]
    private static extern void BindWebGLTexture(int texture);

    public GameObject temp;
    public TextMeshProUGUI text_tmpro;
    private Slider temp_slider;
    public void test(){
        Hello();
    }
    public void test1(){
        HelloString(text_tmpro.text);
    }
    public void test2(String value){
        text_tmpro.text = value;
    }
    // public void test2(){
    //     PrintFloatArray();
    // }
    // public void test3(){
    //     Hello();
    // }
    // Start is called before the first frame update
    private float previousFloatValue;
    void Start()
    {
        temp_slider = temp.GetComponent<Slider>();
        previousFloatValue = temp_slider.value;
        StartCoroutine(CheckFloatValueChange());
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(temp_slider.value);
        // text_tmpro.text = RandomizeString(text_tmpro.text, temp_slider.value);
    }
    IEnumerator CheckFloatValueChange()
    {
        while (true)
        {
            if (Math.Abs(temp_slider.value - previousFloatValue) > Mathf.Epsilon)
            {
                Debug.Log("test");
                text_tmpro.text = RandomizeString(text_tmpro.text, temp_slider.value);
                previousFloatValue = temp_slider.value;
            }
            yield return null; // Check every frame
        }
    }

    static string RandomizeString(string input, float floatValue)
    {
        // Convert float to an integer seed
        int seed = BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
        System.Random random = new System.Random(seed);

        // Convert string to char array and shuffle it
        char[] array = input.ToCharArray();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            char temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        return new string(array);
    }
}
