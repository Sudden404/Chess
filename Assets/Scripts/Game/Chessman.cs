//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Chessman : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;
    
    private bool kingRakir = false;

    private string player;

    private bool Moved = false;

    public Sprite black_queen, white_queen, black_knight, white_knight, black_pawn, white_pawn;
    public Sprite black_bishop, white_bishop, black_rook, white_rook, black_king, white_king;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;
        x *= 0.66f;
        y *= 0.66f;
        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard() { return xBoard; }
    public int GetYBoard() { return yBoard; }
    public void SetXBoard(int x) { xBoard = x; }
    public void SetYBoard(int y) { yBoard = y; }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player) 
        { 
            DestroyMovePlates();
            SetKingRacker(false);

            InitializeMovePlates();
        } 
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitializeMovePlates()
    {
        switch(this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                BlackPawnMovePlate(xBoard, yBoard);
                break;
            case "white_pawn":
                WhitePawnMovePlate(xBoard, yBoard);
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while(sc.PositionOnBoard(x, y) && sc.GetPosition(x,y) == null) 
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if(sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
            MovePlateAttackSpawn(x, y);
    }
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);

        rackir();
    }
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }
    public void PawnPointAttackPlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);
            if (cp != null)
                if (cp.GetComponent<Chessman>().player != player)
                    MovePlateAttackSpawn(x, y);
        }
    }
    public void PawnPointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
        }
    }

    // доделать превращение пешки
    public void BlackPawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        PawnPointAttackPlate(xBoard - 1, yBoard - 1);
        PawnPointAttackPlate(xBoard + 1, yBoard - 1);

        if (!GetMoved())//если пешка не двигалась надо проверить дополнительный ход
        {
            PawnPointMovePlate(xBoard, yBoard - 1);
            GameObject cp = sc.GetPosition(xBoard, yBoard - 1);
            if (cp == null)
            {
                PawnPointMovePlate(xBoard, yBoard - 2);
            } 
        }else
            PawnPointMovePlate(xBoard, yBoard - 1);
    }

    public void WhitePawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        PawnPointAttackPlate(xBoard - 1, yBoard + 1);
        PawnPointAttackPlate(xBoard + 1, yBoard + 1);

        if (!GetMoved())//если пешка не двигалась надо проверить дополнительный ход
        {
            PawnPointMovePlate(xBoard, yBoard + 1);
            GameObject cp = sc.GetPosition(xBoard, yBoard + 1);
            if (cp == null)
            {
                PawnPointMovePlate(xBoard, yBoard + 2);
            }
        }
        else
            PawnPointMovePlate(xBoard, yBoard + 1);
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x -= 2.3f;
        y -= 2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x -= 2.3f;
        y -= 2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
    public bool GetMoved()
    {
        return Moved;
    }
    public void SetMoved(bool value)
    {
        Moved = value;
    }

    public bool GetKingRacker()
    {
        return kingRakir;
    }
    public void SetKingRacker(bool value)
    {
        kingRakir = value;
    }

    public void NewQueenSet()
    {
        GameObject fig = GetChangePawnObj();
        if (controller.GetComponent<Game>().GetStoppedGamePlayer() == "black")
        {
            fig.GetComponent<SpriteRenderer>().sprite = black_queen;
            fig.name = "black_queen";
        }
        else
        {
            fig.GetComponent<SpriteRenderer>().sprite = white_queen;
            fig.name = "white_queen";
        }
        
        CloseChangeButtons();
    }

    public void NewRookSet()
    {
        GameObject fig = GetChangePawnObj();
        if (controller.GetComponent<Game>().GetStoppedGamePlayer() == "black")
        {

            fig.GetComponent<SpriteRenderer>().sprite = black_rook;
            fig.name = "black_rook";
        }
        else
        {
            fig.GetComponent<SpriteRenderer>().sprite = white_rook;
            fig.name = "white_rook";
        }

        CloseChangeButtons();
    }

    public void NewBishopSet()
    {
        GameObject fig = GetChangePawnObj();
        if (controller.GetComponent<Game>().GetStoppedGamePlayer() == "black")
        {

            fig.GetComponent<SpriteRenderer>().sprite = black_bishop;
            fig.name = "black_bishop";
        }
        else
        {
            fig.GetComponent<SpriteRenderer>().sprite = white_bishop;
            fig.name = "white_bishop";
        }

        CloseChangeButtons();
    }
    public void NewKnightSet()
    {
        GameObject fig = GetChangePawnObj();
        if (controller.GetComponent<Game>().GetStoppedGamePlayer() == "black")
        {

            fig.GetComponent<SpriteRenderer>().sprite = black_knight;
            fig.name = "black_knight";
        }
        else
        {
            fig.GetComponent<SpriteRenderer>().sprite = white_knight;
            fig.name = "white_knight";
        }

        CloseChangeButtons();
    }

    public GameObject GetChangePawnObj()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game sc = controller.GetComponent<Game>();
        GameObject fig = null;
        if (controller.GetComponent<Game>().GetStoppedGamePlayer() == "black")
        {
            for (int x = 0; x < 8; x++)
            {
                int y = 0;
                fig = sc.GetPosition(x, y);
                if (fig != null && fig.name == "black_pawn")
                    break;
            }
        }
        else
        {
            for (int x = 0; x < 8; x++)
            {
                int y = 7;
                fig = sc.GetPosition(x, y);
                if (fig != null && fig.name == "white_pawn")
                    break;
            }
        }
        return fig;
    }

    public void CloseChangeButtons()
    {
        GameObject.Find("Queen").GetComponent<UnityEngine.UI.Image>().enabled = false;
        GameObject.Find("Queen").GetComponent<UnityEngine.UI.Button>().enabled = false;

        GameObject.Find("Bishop").GetComponent<UnityEngine.UI.Image>().enabled = false;
        GameObject.Find("Bishop").GetComponent<UnityEngine.UI.Button>().enabled = false;

        GameObject.Find("Rook").GetComponent<UnityEngine.UI.Image>().enabled = false;
        GameObject.Find("Rook").GetComponent<UnityEngine.UI.Button>().enabled = false;

        GameObject.Find("Knight").GetComponent<UnityEngine.UI.Image>().enabled = false;
        GameObject.Find("Knight").GetComponent<UnityEngine.UI.Button>().enabled = false;

        GameObject.Find("TextSelect").GetComponent<Text>().enabled = false;
        GameObject.Find("TextFigure").GetComponent<Text>().enabled = false;

        controller.GetComponent<Game>().ContinueGame();
        controller.GetComponent<Game>().NextTurn();
    }

    public void rackir()
    {
        if (!GetMoved())
        {
            if (name == "black_king")
            {
                if (controller.GetComponent<Game>().GetPosition(7, 7).name == "black_rook" && !controller.GetComponent<Game>().GetPosition(7, 7).GetComponent<Chessman>().GetMoved() &&
                    controller.GetComponent<Game>().GetPosition(6, 7) == null && controller.GetComponent<Game>().GetPosition(5, 7) == null)
                {
                    MovePlateSpawn(6, 7);
                    SetKingRacker(true);
                }
                if (controller.GetComponent<Game>().GetPosition(0, 7).name == "black_rook" && !controller.GetComponent<Game>().GetPosition(0, 7).GetComponent<Chessman>().GetMoved() &&
                    controller.GetComponent<Game>().GetPosition(1, 7) == null && controller.GetComponent<Game>().GetPosition(2, 7) == null && controller.GetComponent<Game>().GetPosition(3, 7) == null)
                {
                    MovePlateSpawn(2, 7);
                    SetKingRacker(true);
                }
            }
            if (name == "white_king")
            {
                if (controller.GetComponent<Game>().GetPosition(7, 0).name == "white_rook" && !controller.GetComponent<Game>().GetPosition(7, 0).GetComponent<Chessman>().GetMoved() &&
                    controller.GetComponent<Game>().GetPosition(6, 0) == null && controller.GetComponent<Game>().GetPosition(5, 0) == null)
                {
                    MovePlateSpawn(6, 0);
                    SetKingRacker(true);
                }
                else
                    MovePlateSpawn(5, 5);
                if (controller.GetComponent<Game>().GetPosition(0, 0).name == "white_rook" && !controller.GetComponent<Game>().GetPosition(0, 0).GetComponent<Chessman>().GetMoved() &&
                    controller.GetComponent<Game>().GetPosition(1, 0) == null && controller.GetComponent<Game>().GetPosition(2, 0) == null && controller.GetComponent<Game>().GetPosition(3, 0) == null)
                {
                    MovePlateSpawn(2, 0);
                    SetKingRacker(true);
                }
            }
        }
    }
}
