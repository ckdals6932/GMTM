using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : KWSingleton<CoroutineTest>
{
    float fTime = 1.0f;
    bool bStart = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetSampleCoroutine1()
	{
        bStart = true;
        //StartCoroutine("SampleCoroutine1");
	}
    public void SetSampleCoroutine2()
	{
	    IEnumerator enumerator = SampleCoroutine2 ();
	    while (enumerator.MoveNext()) 
		{
		    object result = enumerator.Current;
		    Sample.s.ShowLog("Number: " + result);
	    }
	}
    public void SetSampleCoroutine3()
	{
		IEnumerator runCoroutine = SampleCoroutine3();
		while (runCoroutine.MoveNext ()) 
		{
			object result = runCoroutine.Current;

			if (result is WaitForSeconds) 
			{
				Sample.s.ShowLog("Number: " + result);
				// 원하는 초만큼 기다리는 로직 수행
				// 여기 예제에서는 1초만큼 기다리게 될 것임을 알 수 있음
			}
		}
	}
	public void SetSampleCoroutine4()
	{
		StartCoroutine(SampleCoroutine4());
	}
	IEnumerator  SampleCoroutine4()
	{
		yield return StartCoroutine(SampleCoroutine4(2.0F));
		Sample.s.ShowLog("Done " + Time.time);
	}

    // Update is called once per frame
    void Update()
    {
        if (!bStart) 
            return;

		if (fTime > 0.0f) 
        {
			fTime -= 0.2f;
            Sample.s.ShowLog(fTime.ToString());
		}       
    }

	IEnumerator SampleCoroutine1() 
    {
		while (fTime > 0.0f) 
        {
			fTime -= 0.2f;
            Sample.s.ShowLog(fTime.ToString());
			yield return new WaitForSeconds(0.2f);
		}
	}

    IEnumerator SampleCoroutine2() 
    {
	    yield return 3;
	    yield return 5;
	    yield return 8;
    }
    IEnumerator SampleCoroutine3() 
    {
	    yield return new WaitForSeconds(1.0f);
	    yield return new WaitForSeconds(1.0f);
	    yield return new WaitForSeconds(1.0f);
    }
    IEnumerator SampleCoroutine4(float waitTime) 
    {
		yield return new WaitForSeconds(waitTime);
		Sample.s.ShowLog("WaitAndPrint " + Time.time);
    }
}
