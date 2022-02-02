using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelGenerator : EditorWindow
{
    #region MemberFields
    private GameMode gameMode;
    private GameObject Box, levels;
    private Box cube;
    private Color opponentColor, playerColor;
    private int column, row, playeChance, levelNumber;
    private float width;
    #endregion MemberFields

    [MenuItem("Level Creating / Create Level")]
    public static void GenerateLevel()
    {
        GetWindow<LevelGenerator>();
    }

    #region OnGUI
    private void OnGUI()
    {
        GUILayout.Label("Level Generating", EditorStyles.boldLabel);
        Box = EditorGUILayout.ObjectField("Simple Cube", Box, typeof(GameObject), true) as GameObject;
        cube = EditorGUILayout.ObjectField("Player Cube", cube, typeof(Box), true) as Box;
        gameMode = EditorGUILayout.ObjectField("Game Mode", gameMode, typeof(GameMode), true) as GameMode;
        row = EditorGUILayout.IntField("Row", row);
        column = EditorGUILayout.IntField("Column", column);

        if (GUILayout.Button("Generate Ground"))
        {
            Generate();
        }

        if (GUILayout.Button("Change To play able Cube"))
        {
            ChangeToCube();
        }
        EditorGUILayout.Space();
        GUILayout.Label("Player details", EditorStyles.boldLabel);
        playerColor = EditorGUILayout.ColorField("Player Color", playerColor);
        playeChance = EditorGUILayout.IntField("Player Chance", playeChance);
        levelNumber = EditorGUILayout.IntField("Level Number", levelNumber);
        EditorGUILayout.Space();
        if (GUILayout.Button("Set Player Details"))
        {
            SetPlayerDetails(playerColor, playeChance, levelNumber);
        }
        EditorGUILayout.Space();

        opponentColor = EditorGUILayout.ColorField("Opponent Color", opponentColor);

        if (GUILayout.Button("Add Opponent"))
        {
            AddCOlor();
        }
    }
    #endregion OnGUI

    #region Public Method


    #endregion Public Method

    #region Private Method

    private void SetPlayerDetails(Color pl, int chance, int levelNumber)
    {
        if (chance <= 0)
        {
            Debug.Log("Please choose sth moe than zero");
            return;
        }

        GameMode temp = FindObjectOfType<GameMode>();
        if (temp)
        {
            Debug.Log("There was a game mode objects just changed color");
            temp.GetComponent<GameMode>().Setup(pl, chance, levelNumber);
        }
        else
        {
            Instantiate(gameMode, Vector3.zero, Quaternion.identity).Setup(pl, chance, levelNumber);
        }
    }

    private void Generate()
    {
        levels = new GameObject();
        levels.transform.name = "level";
        levels.tag = "Level";

        width = Box.transform.localScale.x * Box.GetComponent<BoxCollider>().size.x;
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Instantiate(Box, pos, Quaternion.identity, levels.transform);
                pos.x += width;
            }
            pos.z += width;
            pos.x = 0;
        }
    }

    private void ChangeToCube()
    {
        Vector3 pos;
        levels = GameObject.FindGameObjectWithTag("Level");

        while (Selection.gameObjects.Length > 0)
        {
            if (Selection.gameObjects[0].GetComponent<Box>())
            {
                return;
            }
            pos = Selection.gameObjects[0].transform.position;
            DestroyImmediate(Selection.gameObjects[0]);
            Instantiate(cube, pos, Quaternion.identity, levels.transform);
        }

    }

    private void AddCOlor()
    {
        GameObject temp = Selection.activeGameObject;
        if (temp.transform.CompareTag("Wall"))
        {
            Debug.Log("Please chose play able objects to change Opponent and be careful");
            return;
        }
        temp.GetComponentInParent<Box>().Setup(opponentColor, true);
    }

    #endregion Private Method
}
