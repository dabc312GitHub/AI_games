using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace;

public class GeneratorAI_HF : MonoBehaviour
{
    // HuggingFace.API.HuggingFaceAPI HF_api;


    // Start is called before the first frame update
    void Start()
    {
        
        Query();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Query() {
        string inputText = "I'm on my way to the forest.";
        string[] candidates = {
            "The player is going to the city",
            "The player is going to the wilderness",
            "The player is wandering aimlessly"
        };
        // HuggingFace.API.HuggingFaceAPI.SentenceSimilarity(inputText, OnSuccess, OnError, candidates);
        // HuggingFaceAPI.SentenceSimilarity(inputText, OnSuccess, OnError, candidates);
    }

    // If successful, handle the result
    void OnSuccess(float[] result) {
        foreach(float value in result) {
            Debug.Log(value);
        }
    }

    // Otherwise, handle the error
    void OnError(string error) {
        Debug.LogError(error);
    }

}
