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
    public Button[] buttons; //luu cac o
    public TMP_Text resultText; //ket qua
    public TMP_Text turnText;//luot choi
    public AudioSource buttonClickSound;  //chon nuoc
    public bool isGamePaused = false; //kiem tra game
    public AudioSource audioSource; 
    public AudioClip playerWinSound; //am thanh chien thang
    public AudioClip playerLostSound; //am thanh thua cuoc
    private void Start()
    {
        buttonClickSound.Stop(); //dung at
        ResetBoard(); //dat lai ban co luc bat dau choi 
    }

    public void ResetBoard()
    {
       //cac o -> o trong
        for (int i = 0; i < 3; i++) //hang
            for (int j = 0; j < 3; j++) //cot
                board[i, j] = '_'; //o trong

       
        foreach (Button button in buttons) //duyet cac nut chua text x o
        {
            button.interactable = true; //co the tuong tac nut 
            button.GetComponentInChildren<TMP_Text>().text = "";
        }

        resultText.text = ""; //an ket qua
        turnText.text = "Player's Turn"; //player choi truoc
    }

 
    public void PlayerMove(int index)
    {
        // xd vi tri tren ban co
        int row = index / 3;      /*vd: index = 4 -> board[1,1] */
        int col = index % 3;

        if (board[row, col] == '_') //neu o la trong
        {
            board[row, col] = 'X'; //player danh X
            buttons[index].GetComponentInChildren<TMP_Text>().text = "X"; //X hien len button
            buttons[index].interactable = false; //ko the nhan
            turnText.text = "AI's Turn"; //den luot cua ai khi xong luot player
            if (CheckGameOver()) return;
            if (!CheckGameOver()) //game chua ket thuc thi ai choi
            {
                StartCoroutine(DelayedAITurn()); 
            }
           
        }
    }

    private void AITurn() //ai tim nuoc di tot nhat
    {
        (int bestRow, int bestCol) = FindBestMove(board); //tim nuoc di theo toa do row col
        if (bestRow != -1 && bestCol != -1) //neu ban co chua day
        {
            board[bestRow, bestCol] = 'O'; //cap nhat voi dau o
            int index = bestRow * 3 + bestCol; // vd: ai chon board[1,2] -> index = 5
            buttons[index].GetComponentInChildren<TMP_Text>().text = "O"; //ky hieu
            buttons[index].interactable = false; //palyer khong the nhan vi ai da di
            turnText.text = "Player's Turn";
            CheckGameOver(); //kiem tra game ket thuc sau khi ai choi xong luot
        }
    }
    private IEnumerator DelayedAITurn()
    {
        // 2s
        yield return new WaitForSeconds(2f);

        AITurn(); //ai choi
    }

    private bool CheckGameOver()
    {
        int score = Evaluate(board);

        if (score == 10)
        {
            resultText.text = "AI Wins!";
            turnText.text = "";
            audioSource.PlayOneShot(playerLostSound); //at play thua ai
            DisableAllButtons(); //khong the choi khi da xong game
            return true; //game ket thuc
        }
        else if (score == -10)
        {
            resultText.text = "Player Wins!";
            turnText.text = "";
            audioSource.PlayOneShot(playerWinSound); //phat at player win ai
            DisableAllButtons();
            return true;
        }
        else if (!IsMovesLeft(board)) //= 0
        {
            resultText.text = "Draw!";
            turnText.text = "";
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
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])    /* x
                                                                              x
                                                                                x  */
        {                                                   
            if (board[0, 0] == 'X') return -10;
            if (board[0, 0] == 'O') return 10;
        }
        //cheo phu
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])       /* x
                                                                             x
                                                                            x    */
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
        return false;//ko con o trong
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

        // full o,khong ai thang
        if (!IsMovesLeft(board))
            return 0;

        // Neu la luot cua Max
        if (isMax) //toi da hoa
        {
            int best = int.MinValue;//chon nuoc di dcn

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == '_') //o trong
                    {
                        board[row, col] = 'O'; 
                        //lay gia tri lon nhat gdtn ht va tra ve tu minimax
                        best = Math.Max(best, Minimax(board, depth + 1, false));  //tinh toan nuoc di,false:den luot Min(player)
                        board[row, col] = '_'; 
                    }
                }
            }
            return best; //nuoc di tot nhat cua ai
        }
        else  //toi thieu hoa
        {
            int best = int.MaxValue;//chon nuoc di dtn

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == '_')
                    {
                        board[row, col] = 'X'; 
                        best = Math.Min(best, Minimax(board, depth + 1, true)); //true:den luot ai
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
