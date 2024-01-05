using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class Google : MonoBehaviour
{
    [SerializeField] private InputField input;

    public void google()
    {
        string googlehash = Backend.Utils.GetGoogleHash();
        if (!string.IsNullOrEmpty(googlehash))
            input.text = googlehash;
        else
        {
            string f = "fail";
            input.text = f;
        }
    } 
}
