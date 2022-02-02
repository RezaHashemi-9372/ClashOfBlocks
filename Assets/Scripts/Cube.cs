using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    #region MemberFields
    private bool isChecked = true;
    private GameMode gameMode;
    #endregion MemberFields

    #region MonoBehaviour Method
    private void Awake()
    {
        gameMode = GameObject.FindObjectOfType<GameMode>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    #endregion MonoBhaviour Method

    #region PublicMethods

    public void Fill(Color fillColor)
     {
        if (!isChecked)
        {
            return;
        }
        this.GetComponent<MeshRenderer>().material.color = fillColor;
        
        Animator animator = this.GetComponent<Animator>();

        if (animator)
        {
            animator.SetBool("IsFilled", true);
            gameMode = FindObjectOfType<GameMode>();
            gameMode.CheckFilledBox();
        }
    }

    #endregion PublicMethods

    #region Private Methods

    #endregion Private Methods

}
