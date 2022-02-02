using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public enum FindingNeighbourMode : short
    {
        ByRaycasting,
        ByBoxCasting,
        BySphereCasting
    }

    #region MemberFields
    public Color fillColor = Color.white;
    private bool couldCheck = true;
    public bool isChecked = false;
    [SerializeField]
    private FindingNeighbourMode findingNeighbourMode = FindingNeighbourMode.ByRaycasting;
    private Vector3[] direction = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    private  GameMode gameMode;
    //public bool justCheck ;
    #endregion MemberFields

    #region Properties

    public Color FillColor
    {
        get
        {
            return fillColor;
        }
        set
        {
            /*if (IsFilled)
            {
                return;
            }*/
            fillColor = value;

            if (this.transform.childCount <= 0)
            {
                return;
            }
            this.transform.GetChild(0).GetComponent<Cube>().Fill(FillColor);
        }
    }
    public bool isfilled = false;
    public bool Isfilled
    {
        get
        {
            return isfilled;
        }
        set
        {
            isfilled = value;
        }
    }
    
    #endregion Properties

    #region MonoBehaviour Methods

    private void Awake()
    {
        gameMode = FindObjectOfType<GameMode>();
        if (Isfilled)
        {
            this.transform.GetChild(0).GetComponent<Cube>().Fill(FillColor);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
        
    }
    
    private void LateUpdate()
    {
        if (couldCheck)
        {
            if (Isfilled && !GameMode.hasShootChane)
            {
                FillNeighbour(FillColor);
                //Raycast(FillColor);
                //RaycastNeighbour(fillColor);
                couldCheck = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (gameMode.ShootChance <= 0)
        {
            return;
        }
        Fill(gameMode.Playercol);
        gameMode.CheckSHootChance();

    }
    #endregion MonoBehaviour Methods

    #region Public Methods


    public void Fill(Color fillColor)
    {
        if (Isfilled)
        {
            return;
        }
        this.FillColor = fillColor;
        Isfilled = true;

    }
    public void FillNeighbour(Color FillColor)
    {
        switch (findingNeighbourMode)
        {
            case FindingNeighbourMode.ByRaycasting:
                Raycast(FillColor);
                break;
            case FindingNeighbourMode.ByBoxCasting:
                break;
            case FindingNeighbourMode.BySphereCasting:
                break;
        }
    }
    public void Setup(Color fill, bool fillBefore)
    {
        this.FillColor = fill;
        Isfilled = fillBefore;
    }

    #endregion Public Methods

    #region Private Mehods

    public void RaycastNeighbour(Color fillColor)
    {
        Vector3 nextPoint = this.transform.position;
        float radius = this.transform.localScale.x * this.GetComponent<BoxCollider>().size.x;
        int stepCount = 8;
        float teta = .0f;
        float angleStep = 360.0f / 8;

        for (int i = 0; i < stepCount; i++)
        {
            nextPoint.x = Mathf.Sin(teta / Mathf.Rad2Deg) * radius;
            nextPoint.z = Mathf.Cos(teta / Mathf.Rad2Deg) * radius;
            nextPoint.y = this.transform.position.y + Vector3.up.y;

            nextPoint += this.transform.position;

            RaycastHit hit;
            if (Physics.Raycast(nextPoint, Vector3.down, out hit))
            {
                Box box = hit.collider.GetComponent<Box>();

                if (box)
                {
                    box.Fill(fillColor);
                }
            }

            teta += angleStep;
        }
    }

    public void Raycast(Color fillcolor)
    {
        for (int i = 0; i < direction.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, direction[i], out hit))
            {
                Box box = hit.collider.GetComponent<Box>();

                if (box)
                {
                    box.Fill(fillcolor);
                }

            }
        }
    }

    private void FillBox(Color fillColor)
    {
        this.FillColor = fillColor;
        this.GetComponentInChildren<Cube>().Fill(gameMode.Playercol);
        this.Isfilled = true;
    }

    #endregion Private Methods
}
