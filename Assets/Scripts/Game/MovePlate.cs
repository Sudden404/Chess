using System.Collections;
using System.Collections.Generic;
//using Math.Abs;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        if(attack)
        {
            //change color
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

    }
    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            if (cp.name == "white_king")
                controller.GetComponent<Game>().Winner("black");
            else if (cp.name == "black_king")
                controller.GetComponent<Game>().Winner("white");

            Destroy(cp);
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(), reference.GetComponent<Chessman>().GetYBoard());
        if (reference.GetComponent<Chessman>().GetKingRacker() && Mathf.Abs(matrixX - reference.GetComponent<Chessman>().GetXBoard()) > 1) // модуль//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            MakeRackir();
        }
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        reference.GetComponent<Chessman>().DestroyMovePlates();
        reference.GetComponent<Chessman>().SetMoved(true);
        if (!controller.GetComponent<Game>().IsGameOver() && CheckFigureOnPawnChange())
        {
            controller.GetComponent<Game>().StopGame();
            ShowChangeFigure();
        }else
            controller.GetComponent<Game>().NextTurn();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference() { return reference; }

    public bool CheckFigureOnPawnChange()
    {
        if (reference.GetComponent<Chessman>().name == "white_pawn" && reference.GetComponent<Chessman>().GetYBoard() == 7)
            return true;
        if (reference.GetComponent<Chessman>().name == "black_pawn" && reference.GetComponent<Chessman>().GetYBoard() == 0)
            return true;
        return false;
    }
    
    public void ShowChangeFigure()
    {
        if (reference.GetComponent<Chessman>().name == "black_pawn")
        {
            GameObject.Find("Queen").GetComponent<Image>().sprite = reference.GetComponent<Chessman>().black_queen;

            GameObject.Find("Bishop").GetComponent<Image>().sprite = reference.GetComponent<Chessman>().black_bishop;

            GameObject.Find("Rook").GetComponent<Image>().sprite = reference.GetComponent<Chessman>().black_rook;

            GameObject.Find("Knight").GetComponent<Image>().sprite = reference.GetComponent<Chessman>().black_knight;

        }
        if (reference.GetComponent<Chessman>().name == "white_pawn")
        {
            GameObject.Find("Queen").GetComponent<Image>().sprite = reference.GetComponent<Chessman>().white_queen;

            GameObject.Find("Bishop").GetComponent<Image>().sprite = reference.GetComponent<Chessman>().white_bishop;

            GameObject.Find("Rook").GetComponent<Image>().sprite = reference.GetComponent<Chessman>().white_rook;

            GameObject.Find("Knight").GetComponent<Image>().sprite = reference.GetComponent<Chessman>().white_knight;

        }

        GameObject.Find("Queen").GetComponent<Image>().enabled = true;
        GameObject.Find("Queen").GetComponent<Button>().enabled = true;

        GameObject.Find("Bishop").GetComponent<Image>().enabled = true;
        GameObject.Find("Bishop").GetComponent<Button>().enabled = true;

        GameObject.Find("Rook").GetComponent<Image>().enabled = true;
        GameObject.Find("Rook").GetComponent<Button>().enabled = true;

        GameObject.Find("Knight").GetComponent<Image>().enabled = true;
        GameObject.Find("Knight").GetComponent<Button>().enabled = true;

        GameObject.Find("TextSelect").GetComponent<Text>().enabled = true;
        GameObject.Find("TextFigure").GetComponent<Text>().enabled = true;
    }

    public void ChangeFigure(string NewFigure)
    {
        switch (NewFigure)
        {
            case "black_queen": reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().black_queen;
                                reference.GetComponent<SpriteRenderer>().name = NewFigure; break;
            case "white_queen": reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().white_queen;
                                reference.GetComponent<SpriteRenderer>().name = NewFigure; break;
            case "black_knight": reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().black_knight; 
                                 reference.GetComponent<SpriteRenderer>().name = NewFigure; break;
            case "white_knight": reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().white_knight; 
                                 reference.GetComponent<SpriteRenderer>().name = NewFigure; break;
            case "black_bishop": reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().black_bishop; 
                                 reference.GetComponent<SpriteRenderer>().name = NewFigure; break;
            case "white_bishop": reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().white_bishop; 
                                 reference.GetComponent<SpriteRenderer>().name = NewFigure; break;
            case "black_rook": reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().black_rook; 
                               reference.GetComponent<SpriteRenderer>().name = NewFigure; break;
            case "white_rook": reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().white_rook; 
                               reference.GetComponent<SpriteRenderer>().name = NewFigure; break;
        }
    }
    public void MakeRackir()
    {
        if(reference.GetComponent<Chessman>().GetXBoard() < matrixX)
        {
            GameObject rook = controller.GetComponent<Game>().GetPosition(7, reference.GetComponent<Chessman>().GetYBoard());
            controller.GetComponent<Game>().SetPositionEmpty(7, rook.GetComponent<Chessman>().GetYBoard());
            
            rook.GetComponent<Chessman>().SetXBoard(matrixX - 1);
            rook.GetComponent<Chessman>().SetYBoard(matrixY);
            rook.GetComponent<Chessman>().SetCoords();

            controller.GetComponent<Game>().SetPosition(rook);

            rook.GetComponent<Chessman>().SetMoved(true);
        }
        else
        {
            GameObject rook = controller.GetComponent<Game>().GetPosition(0, reference.GetComponent<Chessman>().GetYBoard());
            controller.GetComponent<Game>().SetPositionEmpty(0, rook.GetComponent<Chessman>().GetYBoard());

            rook.GetComponent<Chessman>().SetXBoard(matrixX + 1);
            rook.GetComponent<Chessman>().SetYBoard(matrixY);
            rook.GetComponent<Chessman>().SetCoords();

            controller.GetComponent<Game>().SetPosition(rook);

            rook.GetComponent<Chessman>().SetMoved(true);
        }
    }
}
