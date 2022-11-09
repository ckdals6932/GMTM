using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

interface IAnimal 
{
	void Move();
	void Eat();
}

class Cat : IAnimal
{
	public void Move()
	{
		Sample.s.ShowLog("Cat Move");
	}

	public void Eat()
	{
		Sample.s.ShowLog("Cat Eat");
	}
}


abstract class Animal_A
{
	public abstract void Move();
	public void Eat()
	{
		Sample.s.ShowLog("Animal_A Eat");
	}
}

class Dog_A : Animal_A
{
	public override void Move()
	{
		Sample.s.ShowLog("Dog_A Move");
	}
}

public class Animal_V
{
	public virtual void Eat()
	{ 
		Sample.s.ShowLog("Animal_V Move");
	}
}

class Cat_V : Animal_V
{
	public override void Eat()
	{
		Sample.s.ShowLog("Cat_V Eat");
	}
}
class Dog_V : Animal_V
{
	public override void Eat()
	{ 
		Sample.s.ShowLog("Dog_V Eat");
	}
}

[Serializable]
public class Subject_Data
{
    public int score = 0;
    public string subject = "";
}

[Serializable]
public class Score_Data
{
    public int score = 0;
	public string student = "";
    public List<Subject_Data> subjectList = new List<Subject_Data>();
}


