using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class Logger : MonoBehaviour {

    private List<List<string>> rowData=new List<List<string>>();
    private List<string> curr = new List<string>();
	// Use this for initialization
	void Start ()
    {
        curr.Add("MyRacket.x");
        curr.Add("MyRacket.y");
        curr.Add("MyRacket.z");
        curr.Add("OpponentRacket.x");
        curr.Add("OpponentRacket.y");
        curr.Add("OpponentRacket.z");
        curr.Add("Ball.x");
        curr.Add("Ball.y");
        curr.Add("Ball.z");
        curr.Add("Step0");
        curr.Add("Step1");
        curr.Add("Step2");
        curr.Add("Reward");
        List<string> added = new List<string>(curr);
        rowData.Add(added);
	}
    public void AppendLog(Vector3 myRacket,Vector3 opponentRacket,Vector3 ball,float reward)
    {
        curr.Clear();
        curr.Add("" + myRacket.x);
        curr.Add("" + myRacket.y);
        curr.Add("" + myRacket.z);
        curr.Add("" + opponentRacket.x);
        curr.Add("" + opponentRacket.y);
        curr.Add("" + opponentRacket.z);
        curr.Add("" + ball.x);
        curr.Add("" + ball.y);
        curr.Add("" + ball.z);
        //No step here v
        curr.Add("");
        curr.Add("");
        curr.Add("");
        //No step here ^
        curr.Add("" + reward);
        //Debug.Log("Added");
        //foreach(string s in curr)
        //{
        //    Debug.Log(s);
        //}
        List<string> added = new List<string>(curr);
        rowData.Add(added);
    }
    public void AppendLog(Vector3 myRacket,Vector3 opponentRacket,Vector3 ball,Vector3 step,float reward)
    {
        curr.Clear();
        curr.Add("" + myRacket.x);
        curr.Add("" + myRacket.y);
        curr.Add("" + myRacket.z);
        curr.Add("" + opponentRacket.x);
        curr.Add("" + opponentRacket.y);
        curr.Add("" + opponentRacket.z);
        curr.Add("" + ball.x);
        curr.Add("" + ball.y);
        curr.Add("" + ball.z);
        //we have 3 step values so I use vector3 for ease
        curr.Add(""+step.x);
        curr.Add(""+step.y);
        curr.Add(""+step.z);
        curr.Add("" + reward);
        //Debug.Log("Added");
        //Debug.Log(curr.ToString());
        List<string> added = new List<string>(curr);
        rowData.Add(added);
    }
    private void OnApplicationQuit()
    {
        string delimeter = ",";
        StringBuilder sb = new StringBuilder();
        foreach(List<string> row in rowData)
        {
            sb.AppendLine(string.Join(delimeter, row));
        }
        string filePath = getPath();
        //Debug.Log(sb);
        //Debug.Log(new DirectoryInfo(filePath).FullName);
        //Debug.Log(new DirectoryInfo(filePath).Exists);
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
    //Dobrik: I stole it from the web, please have a look at the method below and verify it's correct
    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath + "/CSV/" +"Saved_data.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
        #else
        return Application.dataPath +"/"+"Saved_data.csv";
        #endif
    }
}
