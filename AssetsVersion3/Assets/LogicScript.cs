using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int score1;
    public int score2;
    public Text scoreText;

    //[ContextMenu("INCREASE SCORE 1")]
    public void addScore1(){
        score1+=1;
        scoreText.text = score1.ToString() + "-" + score2.ToString(); 
    }

    public void addScore2(){
        score2+=1;
        scoreText.text = score1.ToString() + "-" + score2.ToString(); 
    }
}
