using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class TicTacToe : MonoBehaviour
{
    private char[,] board = new char[3, 3]; //3x3
    public Button[] buttons; 
    public TMP_Text resultText;
    public AudioSource buttonClickSound;
    public bool isGamePaused = false;
    private void Start()
    {
        buttonClickSound.Stop();
        ResetBoard(); //dat lai ban co luc bat dau choi 
    }

    public void ResetBoard()
    {
       //khoi tao cac o
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                board[i, j] = '_';

       
        foreach (Button button in buttons)
        {
            button.interactable = true; //co the tuong tac nut 
            button.GetComponentInChildren<TMP_Text>().text = ""; //van ban trong
        }

        resultText.text = "";
    }

 
    public void PlayerMove(int index)
    {
        int row = index / 3;
        int col = index % 3;

        if (board[row, col] == '_')
        {
            board[row, col] = 'X'; 
            buttons[index].GetComponentInChildren<TMP_Text>().text = "X"; // Sử dụng TMP_Text
            buttons[index].interactable = false;

            if (CheckGameOver()) return; 

            AITurn();  //ai choi luot tiep theo neu chua het tro choi
        }
    }

    private void AITurn() //ai tim nuoc di tot nhat
    {
        (int bestRow, int bestCol) = FindBestMove(board); //tim nuoc di theo toa do row col
        if (bestRow != -1 && bestCol != -1)
        {
            board[bestRow, bestCol] = 'O'; //cap nhat voi dau o
            int index = bestRow * 3 + bestCol;
            buttons[index].GetComponentInChildren<TMP_Text>().text = "O"; //ky hieu
            buttons[index].interactable = false;

            CheckGameOver(); //kiem tra game ket thuc sau khi ai choi xong luot
        }
    }

    private bool CheckGameOver()
    {
        int score = Evaluate(board);

        if (score == 10)
        {
            resultText.text = "AI Wins!"; 
            DisableAllButtons(); //khong the choi khi da xong game
            return true; //game ket thuc
        }
        else if (score == -10)
        {
            resultText.text = "Player Wins!"; 
            DisableAllButtons();
            return true;
        }
        else if (!IsMovesLeft(board)) //= 0
        {
            resultText.text = "Draw!";
            return true;
        }

        return false; //game chua ket thuc
    }

    private void DisableAllButtons() //ko the nhan
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
    private int Evaluate(char[,] board) //xem xet bang ai thang ai thua
    {
       //kiem tra hang
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
            {
                if (board[row, 0] == 'X') return -10; // player win
                if (board[row, 0] == 'O') return 10; // ai win
            }
        }

       //cot
        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == board[1, col] && board[1, col] == board[2, col])
            {
                if (board[0, col] == 'X') return -10;
                if (board[0, col] == 'O') return 10;
            }
        }

       //cheo chinh
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == 'X') return -10;
            if (board[0, 0] == 'O') return 10;
        }
        //cheo phu
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == 'X') return -10;//player win
            if (board[0, 2] == 'O') return 10;//ai win
        }

       
        return 0;
    }
    private bool IsMovesLeft(char[,] board) //kiem tra con o trong khong
    {
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                if (board[row, col] == '_') 
                    return true;//con o trong
        return false;//ko o trong
    }
    private int Minimax(char[,] board, int depth, bool isMax)
    {
        int score = Evaluate(board); //tinh va tra diem

        // if min win
        if (score == -10)
            return score - depth;

        // if max win
        if (score == 10)
            return score + depth;

        // Neu khong con nuoc di
        if (!IsMovesLeft(board))
            return 0;

        // Neu la luot cua Max
        if (isMax)
        {
            int best = int.MinValue;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == '_')
                    {
                        board[row, col] = 'X'; 
                        best = Math.Max(best, Minimax(board, depth + 1, false));
                        board[row, col] = '_'; 
                    }
                }
            }
            return best;
        }
        else 
        {
            int best = int.MaxValue;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == '_')
                    {
                        board[row, col] = 'O'; 
                        best = Math.Min(best, Minimax(board, depth + 1, true));
                        board[row, col] = '_'; 
                    }
                }
            }
            return best;
        }
    }
    private (int, int) FindBestMove(char[,] board) //duyet o,ai choi nuoc di tot nhat,ghi lai toa do
    {
        int bestVal = int.MinValue;
        int bestRow = -1, bestCol = -1;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (board[row, col] == '_')
                {
                    board[row, col] = 'X'; 
                    int moveVal = Minimax(board, 0, false);
                    board[row, col] = '_'; 

                    if (moveVal > bestVal)
                    {
                        bestRow = row;
                        bestCol = col;
                        bestVal = moveVal;
                    }
                }
            }
        }
        return (bestRow, bestCol);
    }
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }
    public void StopGame()
    {
        isGamePaused = true;

        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
        if (buttonClickSound != null)
        {
            buttonClickSound.Pause();
        }
        resultText.text = "Paused";
    }
    public void ResumeGame()
    {
        isGamePaused = false;

       
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        
        if (buttonClickSound != null)
        {
            buttonClickSound.UnPause();
        }
        resultText.text = "";
    }
    public void LoadMenu()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
        SceneManager.LoadScene("Menu");
    }
  
}
