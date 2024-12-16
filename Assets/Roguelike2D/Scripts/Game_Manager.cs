using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class Game_Manager : MonoBehaviour
{

    public static Game_Manager Instance { get; private set; }

    public Board_Manager board_manager;
    public Player_Controller player_controller;

    public Turn_Manager turn_manager { get; private set; }
    
    public UIDocument UIDoc;
    private Label m_Food_Label;

    private VisualElement m_GameOverPanel;
    private Label m_GameOverLabel;

    public bool GameOver = false;
    
    public int m_comida = 60;
    public int HealNumber;

    public int currentLevel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        m_Food_Label = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        
        turn_manager = new Turn_Manager();
        turn_manager.onTick += TurnoPasado;

        m_GameOverPanel = UIDoc.rootVisualElement.Q< VisualElement> ("GameOverPanel");
        m_GameOverLabel = m_GameOverPanel.Q<Label>("GameOverLabel");

    board_manager.MapGeneration();

        player_controller.Spawn(board_manager, new Vector2Int(board_manager.SpawnX, board_manager.SpawnY));
    }

    public void TurnoPasado ()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount) 
    {
        if (m_comida + amount > 100)
        {
            m_comida = 100;
        }
        else
        {
            m_comida = m_comida + amount;
        }
        
        m_Food_Label.text = "Comida: " + m_comida;


        if (m_comida <= 0) 
        {
            GameOver = true;
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverLabel.text = "Game Over!! \n\nHas avanzado a traves de " + /*currentLevel + */ " niveles";
        }


    }

    public void NewLevel() 
    {
        board_manager.Clean();
        board_manager.MapGeneration();
        player_controller.Spawn(board_manager, new Vector2Int(1,1));

        currentLevel ++;
    }

    public void StartNewGame() 
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        currentLevel = 1;
        m_comida = 60;
        board_manager.Clean();
        board_manager.MapGeneration();
        player_controller.init();
        player_controller.Spawn(board_manager,new Vector2Int(1,1));
        m_Food_Label.text = "Comida: " + m_comida;
    }
}
