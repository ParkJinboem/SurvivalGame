using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : WeakAnimal
{
    protected override void Update()
    {
        base.Update();
        if(viewAngle.View() && !isDead)
        {
            Run(viewAngle.GetTargetPos());
        }
    }
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }
    private void RandomAction()
    {
        RandomSound();
        int random = Random.Range(0, 4); //´ë±â, Ç®µè³¢, µÎ¸®¹ø, °È±â
        if (random == 0)
        {
            Wait();
        }
        else if (random == 1)
        {
            Eat();
        }
        else if (random == 2)
        {
            Peek();
        }
        else if (random == 3)
        {
            TryWalk();
        }
    }

    private void Wait()
    {
        currentTime = waitTime;
    }

    private void Eat()
    {
        anim.SetTrigger("Eat");
        currentTime = waitTime;
    }

    private void Peek()
    {
        anim.SetTrigger("Peek");
        currentTime = waitTime;
    }

}
