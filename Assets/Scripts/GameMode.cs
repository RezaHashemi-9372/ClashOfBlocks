using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    #region MemberFields
    [SerializeField]
    private TextMesh percentageText;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private GameObject pnlWin;
    [SerializeField]
    private GameObject pnlLose;

    public static bool hasShootChane = true;
    public int shootChance = 0;
    public int levelNumber = 0;
    private int Checker = 0;
    private int savedShootChance = 0;

    private Box[] allObjects;
    private int[] colorBoxCounter;
    public Color playercol = Color.white;
    private List<Color> colors = new List<Color>();


    #endregion MemberFields

    #region Properties

    public int ShootChance
    {
        get
        {
            return shootChance;
        }

        set
        {
            shootChance = value;
        }
    }
    public Color Playercol
    {
        get
        {
            return playercol;
        }
        set
        {
            playercol = value;
        }
    }

    #endregion Properties

    #region MonoBehaviour Methods
    private void Awake()
    {
        savedShootChance = shootChance;
        pnlWin.SetActive(false);
        pnlLose.SetActive(false);
        allObjects = GameObject.FindObjectsOfType<Box>();
    }

    private void Start()
    {
        levelText.text = string.Format("{0}", levelNumber);
    }
    #endregion MonoBehaviour Methods

    #region PublicMethods

    public void CheckSHootChance()
    {
        ShootChance -= 1;
        if (ShootChance <= 0)
        {
            hasShootChane = false;
        }
    }


    public void Setup(Color pl, int shtChance, int levelNumber)
    {
        Playercol = pl;
        ShootChance = shtChance;
        this.levelNumber = levelNumber;
    }

    public void CheckFilledBox()
    {
        Checker += 1;
        allObjects = FindObjectsOfType<Box>();
        if (Checker >= allObjects.Length )
        {
            CheckWinner();
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        shootChance = savedShootChance;
        hasShootChane = true;
    }

    public void CheckWinner()
    {
        colors = SetColors(allObjects);
        colorBoxCounter = new int[colors.Count];

        for (int i = 0; i < allObjects.Length; i++)
        {
            for (int j = 0; j < colors.Count; j++)
            {
                if (allObjects[i].FillColor == colors[j])
                {
                    colorBoxCounter[j]++;
                }
            }
        }

        SetMaxNumColor();
    }

    #endregion PublicMethods

    #region PrivateMethods
    private void SetMaxNumColor()
    {
        int maxNumber = 0;
        Color maxColor = new Color();
        for (int i = 0; i < colors.Count; i++)
        {
            if (colorBoxCounter[i] > maxNumber)
            {
                maxNumber = colorBoxCounter[i];
                maxColor = colors[i];
            }
        }

        SetPercentage(maxColor, maxNumber);
    }

    private void SetPercentage(Color maxCol, int maxNum)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            bool temFlag = true;
            int k = 0;
            while (temFlag)
            {
                if (allObjects[k].FillColor == colors[i])
                {
                    Vector3 pos = allObjects[k].transform.position;
                    pos.x -= 1;
                    //pos.y += .5f;
                    CreateText(pos, colorBoxCounter[i], maxNum);
                    temFlag = false;
                }
                k++;
            }
        }

        if (maxCol == Playercol)
        {
            Invoke("ShowWinPanel", 4.0f);
        }
        else
        {
            Invoke("ShowLosePanel", 4.0f);
        }
    }

    private void ShowLosePanel()
    {
        pnlLose.SetActive(true);
    }
    private void ShowWinPanel()
    {
        pnlWin.SetActive(true);
    }
    private void CreateText(Vector3 position, int coloredBoxNumber, int maxnumber)
    {
        float percentUnit = 100.0f / allObjects.Length;
        TextMesh tempText = Instantiate(percentageText, position, Quaternion.Euler(70.0f, 0, 0));
        tempText.text = string.Format("{0}%", (percentUnit * coloredBoxNumber).ToString("F1"));
        if (coloredBoxNumber == maxnumber)
        {
            tempText.fontSize = 17;
            tempText.color = Color.black;
        }
        else
        {
            tempText.fontSize = 12;
            tempText.color = Color.black;
        }
    }
    private List<Color> SetColors(Box[] allBox)
    {
        List<Color> tempColorList = new List<Color>();

        for (int i = 0; i < allBox.Length; i++)
        {
            if (!tempColorList.Contains(allBox[i].FillColor))
            {
                tempColorList.Add(allBox[i].FillColor);
            }
        }

        return tempColorList;
    }

    #endregion PrivateMethods
}
