using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;
using MiniJSON;
using System.Linq;

/// <summary>
/// This script handles:
///     - HFRankOrders_(string source_sentence): the post request to the API: given an user input score each action in the robot list.
///     - ProcessResult(string result): given a string result, convert it to an array of floats and find the max score and max score index.
///     - Start(): to build the JSON correctly, we need to make a string array of all robot sentences;
/// </summary>
public class HuggingFaceAPI : MonoBehaviour
{
    [Header("HuggingFace Model URL")]
    public string model_url;

    [Header("HuggingFace Key API")]
    public string hf_api_key; // DO NOT SHARE YOUR PROJECT IF YOU DEFINED YOUR API KEY

    [HideInInspector]
    public string source_sentence; // User input text

    [HideInInspector]
    public List<string> sentences; // Robot list of sentences (actions)

    [HideInInspector]
    public float maxScore; // Value of the action with the highest score

    [HideInInspector]
    public int maxScoreIndex; // Index of the action with the highest score

    [Header("Jammo Behavior")]
    public JammoBehavior jammo; // A reference to Jammo Behavior Script

    void Start()
    {
        // To prepare the JSON, we take all the sentences candidates
        foreach (JammoBehavior.Actions actions in jammo.actionsList)
        {
            sentences.Add(actions.sentence);
        }
    }

    /// <summary>
    /// Given a user input text and a set of sentences candidates, call HF model to score each of them.
    /// The JSON looks like this:
    /// 
    /// {
    /// "inputs": {
    ///     "source_sentence": "That is a happy person",
    ///     "sentences": [
    ///         "That is a happy dog",
    ///         "That is a very happy person",
    ///         "Today is a sunny day"
    ///         ]
    ///         },
    /// }
    /// </summary>
    /// <param name="source_sentence">user input sentence</param>
    public void HFRankOrders_(string source_sentence)
    {
        StartCoroutine(HFScore(source_sentence));
    }

    public IEnumerator HFScore(string prompt)
    {
        // Form the JSON
        var form = new Dictionary<string, object>();
        var attributes = new Dictionary<string, object>();
        attributes["source_sentence"] = prompt;
        attributes["sentences"] = sentences;
        form["inputs"] = attributes;

        var json = Json.Serialize(form);
        Debug.Log("JSON" + json);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

        // Make the web request
        UnityWebRequest request = UnityWebRequest.Put(model_url, bytes);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + hf_api_key);
        request.method = "POST"; // Hack to send POST to server instead of PUT

        yield return request.SendWebRequest();

        // If the request return an error set the error on console.
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.data);
            yield return request.error;
        }
        else
        {
            JSONNode data = request.downloadHandler.text;
            // Process the result
            yield return ProcessResult(data);
        }
    }

    /// <summary>
    /// We receive a string like "[0.7777, 0.19, 0.01]", we need to process this data to transform it to an array of floats
    /// [0.77, 0.19, 0.01] to be able to perform operations on it.
    /// </summary>
    /// <param name="result">json return from API call</param>
    private IEnumerator ProcessResult(string result)
    {
        // The data is a score for each possible sentence candidate
        // But, it looks something like this "[0.7777, 0.19, 0.01]"
        // First, we need to remove [ and ]
        string cleanedResult = result.Replace("[", "");
        cleanedResult = cleanedResult.Replace("]", "");

        // Then, we need to split each element of the array and convert to float
        string[] splitArray = cleanedResult.Split(char.Parse(","));
        float[] myFloat = splitArray.Select(float.Parse).ToArray();

        // Now that we have a array of floats, we can find the max score and the index of it.
        // We don't need to return these 2 variables, we'll access them directly since they are public.
        maxScore = myFloat.Max();
        maxScoreIndex = myFloat.ToList().IndexOf(maxScore);

        yield return null;
    }     
}
