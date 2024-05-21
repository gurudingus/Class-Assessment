using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.txt";

    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("Our current Directory is: " + currentDirectory);

        LoadScoresFromFile();
    }

    void Update()
    {
    }

    public void LoadScoresFromFile()
    {
        //checks that the file exists
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists == true)
        {
            Debug.Log("Found high score file " + scoreFileName);
        }
        else
        {
            Debug.Log("The file " + scoreFileName +
                     " does not exists. No scores will be loaded.", this);
            return;
        }

        scores = new int[scores.Length];

        StreamReader fileReader = new StreamReader(currentDirectory +
                                             "\\" + scoreFileName);

        int scoreCount = 0;

        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            string fileLine = fileReader.ReadLine();

            int readScore = -1;

            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                scores[scoreCount] = readScore;
            }
            else
            {
                Debug.Log("Invalid line in scores file at " + scoreCount +
                         ", using default value.", this);
                scores[scoreCount] = 0;
            }
            scoreCount++;
        }

        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
    }

    public void SaveScoresToFile()
    {   // creates a StreamWriter for the file path
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\"
                                                  + scoreFileName);
        //Writes the lines to the file
        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }

        //close the Stream
        fileWriter.Close();

        //write a log message
        Debug.Log("High scores written to " + scoreFileName);
    }

    public void AddScore(int newScore)
    {
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length; i++) 
        {
            if (scores[i] < newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }

        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore +
                     " not high enough for high scores list.", this);
            return;
        }

        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - i];
        }

        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore +
                 " entered into high scores at position " + desiredIndex, this);
    }
}
