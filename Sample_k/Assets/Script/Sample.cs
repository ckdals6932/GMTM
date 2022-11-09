using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public delegate void Del(string message);

delegate int Compare(int a, int b);


public class Sample : MonoBehaviour
{
    public static Sample s;
    
    public Text logText;
    // Start is called before the first frame update
    void Start()
    {
        s = this;     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_Test()
	{
        //Interface();
        //AbstractClass();
        //VirtualFunction();
        //ArrayTest();
        //StringTest();
        //JsonTest();
        //DelegateTest();
        //DelegateTest2();
        //SavePlayerPref();
        //LoadPlayerPref();
        //CoroutineTest.instance.SetSampleCoroutine1();
        //CoroutineTest.instance.SetSampleCoroutine2();
        //CoroutineTest.instance.SetSampleCoroutine3();
        //CoroutineTest.instance.SetSampleCoroutine4();
        //RamdaTest1();
        RamdaTest2();
	}

    void Interface()
	{
         IAnimal  cat = new Cat();
	     cat.Move();
	}

    void AbstractClass()
	{
 	    Dog_A  dog = new Dog_A();
	    dog.Move();
	    dog.Eat();
	}
    void VirtualFunction()
	{
 	    Animal_V animal = new Animal_V(); 
        animal.Eat(); 
	    Cat_V  cat = new Cat_V(); 	
        cat.Eat();
 	    Dog_V dog = new Dog_V(); 	
        dog.Eat();
	    Animal_V  cat2 = new Cat_V(); 	
        cat2.Eat();
 	    Animal_V dog2 = new Dog_V(); 	
        dog2.Eat();
	}

    void ArrayTest()
	{
        int[] score = new int[5] { 10, 20, 30, 40, 50 };
        string[] subjects = new string[3] {"수학", "영어", "국어"};

        string logText = "";
        foreach (string subject in subjects)
        {
            logText += subject;    
        }
        ShowLog(logText);

        logText = "";
        for( int i = 0 ; i < subjects.Length ; i++ )
        {
            logText += subjects[i];
        }
        ShowLog(logText);

        ShowLog("Rank :" + score.Rank.ToString()); 
        ShowLog("Length : " + score.Length.ToString()); 
        Array.ForEach<int>(score, new Action<int>(ShowValue));  
        Array.Resize(ref score, score.Length + 3); 
        Array.ForEach<int>(score, new Action<int>(ShowValue));    
        int index = Array.IndexOf(score, 20);
        ShowLog("index : " + index.ToString()); 
        Array.Clear(score, 0, 5); 
        Array.ForEach<int>(score, new Action<int>(ShowValue)); 
	}

    static void ShowValue(int value) // 동작, value를 받아 그 value를 출력시킴
    {
        Sample.s.ShowLog("ShowValue : " + value.ToString());
    }
    
    void StringTest()
	{
        string value = "부천대학교 영상&게임콘텐츠과";
        int start = 6;
        int len = value.Length;
        string dept = value.Substring(start, len - start);
        ShowLog(dept);
        
        string[] words = value.Split(' ');
        foreach (string word in words)
        {
           ShowLog(word); 
        }

        int index = value.IndexOf(' ');
        dept = value.Substring(index, len - index);
        ShowLog(dept);
	}



    void JsonTest()
	{
        var sub1 = new Subject_Data
        {
            score = 10, 
            subject = "수학"    
        };
        var sub2 = new Subject_Data  
        { 
            score = 20, 
            subject = "과학"    
        };

        List<Subject_Data> subList = new List<Subject_Data>();
        subList.Add(sub1);
        subList.Add(sub2);

        var scoreData = new Score_Data 
        {  
            score = 10, 
            student = "홍길동",
            subjectList = subList 
        };

        string jsonStr = LitJson.JsonMapper.ToJson(scoreData);
        ShowLog("ToJson : " + jsonStr); 

		LitJson.JsonData jData = LitJson.JsonMapper.ToObject(jsonStr);
        ShowLog(string.Format("JSONDATA {0} , {1}", jData["score"].ToString(), 
                    jData["student"].ToString()));

        LitJson.JsonData jSubjectList = jData["subjectList"];
        for (int i = 0; i < jSubjectList.Count; i++)
        {
            ShowLog(string.Format("SUBJECT DATA {0} , {1}", jSubjectList[i]["score"].ToString(), 
                    jSubjectList[i]["subject"].ToString()));
        }

        Score_Data data2 = JsonUtility.FromJson<Score_Data>(jsonStr);
        ShowLog(string.Format("DATA2 {0} , {1}", data2.score, data2.student));

        File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data2));
        ShowLog(Application.dataPath + "/TestJson.json");

        string str2 = File.ReadAllText(Application.dataPath + "/TestJson.json"); 
        ShowLog("ReadAllText : " + str2); 

        Score_Data data4 = JsonUtility.FromJson<Score_Data>(str2);
        ShowLog(string.Format("DATA4 {0} , {1}", data2.score, data2.student));
	}

    void DelegateTest()
    {
	    Del handler = DelegateMethod1; 
	    handler += DelegateMethod2; 
	    handler += DelegateMethod3; 
	    handler ("Hello Delegate"); 
	    ShowLog("빼기");
	    handler -= DelegateMethod3; 
	    handler ("Hello Delegate"); 

    }
    void DelegateMethod1(string message) 
    { 
	    ShowLog("#1 " + message); 
    } 
    void DelegateMethod2(string message) 
    { 
	    ShowLog("#2 " + message); 
    } 
    void DelegateMethod3(string message) 
    { 
	    ShowLog("#3 " + message); 
    }

    void DelegateTest2()
    {
        int[] array = { 1, 5, 3, 10, 6 };
        DataSort(array, new Compare(AscendCompare));

        string log = string.Format("{0}, {1}, {2}, {3}, {4}", 
            array[0],array[1],array[2],array[3],array[4] );
        ShowLog(log);

        int[] array2 = { 1, 5, 3, 10, 6 };
        DataSort(array2, new Compare(DescendCompare));

        log = string.Format("{0}, {1}, {2}, {3}, {4}", 
            array2[0],array2[1],array2[2],array2[3],array2[4] );
        ShowLog(log);
    }
    
    static int AscendCompare(int a, int b)
   {
        if (a > b) return 1;
        else if (a == b) return 0;
        else return -1;
    }    
    static int DescendCompare(int a, int b)
    {
        if (a < b) return 1;
        else if (a == b) return 0;
        else return -1;
    }       
    static void DataSort(int[] DataSet, Compare comparer)
    {
        int i, j, temp = 0; 
        for(i = 0; i < DataSet.Length -1; i++)
        {
            for(j = 0; j < DataSet.Length -(i+1); j++)
			{
                if (comparer(DataSet[j], DataSet[j+1]) > 0)
	            {
                    temp = DataSet[j + 1];
                    DataSet[j + 1] = DataSet[j];
                    DataSet[j] = temp;
	            }
            }
         }
     }

    void SavePlayerPref()
    {
        PlayerPrefs.SetFloat("Volume1", 10.12f);
        PlayerPrefs.SetInt("Volume2", 12345);
        PlayerPrefs.SetString("Volume3", "vvvvv");

        ShowLog("Save PlayerPref");
    }
    void LoadPlayerPref()
    {
        float  saveData1  = PlayerPrefs.GetFloat("Volume1"); 
        int    saveData2  = PlayerPrefs.GetInt("Volume2"); 
        string saveData3 = PlayerPrefs.GetString("Volume3");

        ShowLog("Load PlayerPref");
        ShowLog(saveData1.ToString());
        ShowLog(saveData2.ToString());
        ShowLog(saveData3);
    }

    delegate void MyDelegate(string Name, int age);
    delegate string Message(string message);
    void RamdaTest1()
	{
 	    MyDelegate student = (name, age) =>
        {
 		    ShowLog(string.Format("이름 : {0}, 나이 : {1}", name, age));
        };

        student("홍길동", 27);

        Message message = (str) => { return str; };
        ShowLog(string.Format("이름 : {0}", message("홍길동")));
	}


	void RamdaTest2()
	{
        RamdaCallback(() => { OnCallBack(); });
	}

    public void OnCallBack()
    {
        ShowLog("OnCallBack");    
    }

	public void RamdaCallback( System.Action action1)
	{
		action1.Invoke();
	}



    public void ShowLog(string szMessage)
	{
        string msg = logText.text + "\r\n" + szMessage;
        logText.text = msg;
	}
}
