using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TicTacToe : MonoBehaviour
{
    private char[,] board = new char[3, 3]; 
    public Button[] buttons; 
    public TMP_Text resultText;
    public AudioSource buttonClickSound;
    public bool isGamePaused = false;
    private void Start()
    {
        buttonClickSound.Stop();
        ResetBoard();
    }

    public void ResetBoard()
    {
       
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                board[i, j] = '_';

       
        foreach (Button button in buttons)
        {
            button.interactable = true;
            button.GetComponentInChildren<TMP_Text>().text = ""; 
        }

        resultText.text = "";
    }

 
    public void PlayerMove(int index)
    {
        int row = index / 3;
        int col = index % 3;

        if (board[row, col] == '_')
        {
            board[row, col] = 'O'; 
            buttons[index].GetComponentInChildren<TMP_Text>().text = "X"; // Sử dụng TMP_Text
            buttons[index].interactable = false;

            if (CheckGameOver()) return; 

            AITurn(); 
        }
    }

    private void AITurn()
    {
        (int bestRow, int bestCol) = FindBestMove(board);
        if (bestRow != -1 && bestCol != -1)
        {
            board[bestRow, bestCol] = 'X'; 
            int index = bestRow * 3 + bestCol;
            buttons[index].GetComponentInChildren<TMP_Text>().text = "O"; 
            buttons[index].interactable = false;

            CheckGameOver();
        }
    }

    private bool CheckGameOver()
    {
        int score = Evaluate(board);

        if (score == 10)
        {
            resultText.text = "AI Wins!"; 
            DisableAllButtons();
            return true;
        }
        else if (score == -10)
        {
            resultText.text = "Player Wins!"; 
            DisableAllButtons();
            return true;
        }
        else if (!IsMovesLeft(board))
        {
            resultText.text = "Draw!";
            return true;
        }

        return false;
    }

    private void DisableAllButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
    private int Evaluate(char[,] board)
    {
       
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
            {
                if (board[row, 0] == 'X') return +10; // AI win
                if (board[row, 0] == 'O') return -10; // player win
            }
        }

       
        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == board[1, col] && board[1, col] == board[2, col])
            {
                if (board[0, col] == 'X') return +10;
                if (board[0, col] == 'O') return -10;
            }
        }

       
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == 'X') return +10;
            if (board[0, 0] == 'O') return -10;
        }
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == 'X') return +10;
            if (board[0, 2] == 'O') return -10;
        }

       
        return 0;
    }
    private bool IsMovesLeft(char[,] board)
    {
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                if (board[row, col] == '_') 
                    return true;
        return false;
    }
    private int Minimax(char[,] board, int depth, bool isMax)
    {
        int score = Evaluate(board);

        // if max win
        if (score == 10)
            return score - depth;

        // if min win
        if (score == -10)
            return score + depth;

        // Nếu không còn nước đi
        if (!IsMovesLeft(board))
            return 0;

        // Nếu là lượt của Max
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
    private (int, int) FindBestMove(char[,] board)
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
}
