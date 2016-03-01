using UnityEngine;
using System.Collections;

public class Checker : MonoBehaviour {

    public int Column = 8;
    public int Row = 8;

    public GameObject redGem;
    public GameObject blueGem;
    public GameObject greenGem;
    public GameObject yellowGem;
    public GameObject whiteGem;
    public GameObject greyGem;
    public GameObject brownGem;

    // Store gem GameObject
    GameObject[] gems;
    // Store gem color
    GemColor[] gemsColor;

    // set current column and row number
    int nCo = 0;
    int nRo = 0;

    // set position of vector
    float posX = -7.15f;
    float posY = 4.05f;
    float posZ = 0;

    Vector3 pos;

    bool moveable = false;

    public enum GemColor
    {
        red,
        blue,
        green,
        yellow,
        white,
        grey,
        brown
    };

    // create a begin board
    void Awake()
    {
        gems = new GameObject[Column * Row];
        gemsColor = new GemColor[Column * Row];
        
        for (int i = 0; i < gems.Length; i++ )
        {
            int color = Random.Range(1, 7);
            
            pos = new Vector3(posX, posY, posZ);
           
            if (color == 1)
            {
                gems[i] = Instantiate(redGem, pos, Quaternion.identity) as GameObject;
                gemsColor[i] = GemColor.red;
            }
            else if (color == 2)
            {
                gems[i] = Instantiate(blueGem, pos, Quaternion.identity) as GameObject;
                gemsColor[i] = GemColor.blue;
            }
            else if (color == 3)
            {
                gems[i] = Instantiate(greenGem, pos, Quaternion.identity) as GameObject;
                gemsColor[i] = GemColor.green;
            }
            else if (color == 4)
            {
                gems[i] = Instantiate(yellowGem, pos, Quaternion.identity) as GameObject;
                gemsColor[i] = GemColor.yellow;
            }
            else if (color == 5)
            {
                gems[i] = Instantiate(whiteGem, pos, Quaternion.identity) as GameObject;
                gemsColor[i] = GemColor.white;
            }
            else if (color == 6)
            {
                gems[i] = Instantiate(greyGem, pos, Quaternion.identity) as GameObject;
                gemsColor[i] = GemColor.grey;
            }
            else if (color == 7)
            {
                gems[i] = Instantiate(brownGem, pos, Quaternion.identity) as GameObject;
                gemsColor[i] = GemColor.brown;
            }

            // move to next position in column and increase current column number
            if ((i+1) % Column != 0 || i == 0)
            {
                nCo++;

                posX += 1.3f;
            }
            // if can divide by Column that mean already finish that row, also not count 0
            else if((i+1)  % Column == 0)
            {
                nRo++;

                if (nRo >= Row)
                {
                    nRo = 0;

                    posX = -7.15f;
                    posY = 4.05f;
                }
                else
                {
                    posX = -7.15f;
                    posY -= 1.14f;
                }

                nCo = 0;
            }
        }    
    }

    void Update()
    {
        checkMatch();
    }

    void checkMatch()
    {
        for (int i = 0; i < gems.Length; i++)
        {
            if (nCo < Column - 2)
            {
                if (gemsColor[i] == gemsColor[i + 1] && gemsColor[i] == gemsColor[i + 2])
                {
                    Destroy(gems[i].gameObject);
                    Destroy(gems[i + 1].gameObject);
                    Destroy(gems[i + 2].gameObject);
                    setGemColor(i,posX,posY,posZ);
                    setGemColor(i + 1,posX + 1.3f,posY,posZ);
                    setGemColor(i + 2,posX + (2 * 1.3f),posY,posZ);
                }
            }
            else if (nCo < Column - 3)
            {
                if (gemsColor[i] == gemsColor[i + 1] && gemsColor[i] == gemsColor[i + 2] && gemsColor[i] == gemsColor[i + 3])
                {
                    Destroy(gems[i].gameObject);
                    Destroy(gems[i + 1].gameObject);
                    Destroy(gems[i + 2].gameObject);
                    Destroy(gems[i + 3].gameObject);
                    setGemColor(i, posX, posY, posZ);
                    setGemColor(i + 1, posX + 1.3f, posY, posZ);
                    setGemColor(i + 2, posX + (2 * 1.3f), posY, posZ);
                    setGemColor(i + 3, posX + (3 * 1.3f), posY, posZ);
                }
            }
            else if (nCo < Column - 4)
            {
                if (gemsColor[i] == gemsColor[i + 1] && gemsColor[i] == gemsColor[i + 2] && gemsColor[i] == gemsColor[i + 3] && gemsColor[i] == gemsColor[i + 4])
                {
                    Destroy(gems[i].gameObject);
                    Destroy(gems[i + 1].gameObject);
                    Destroy(gems[i + 2].gameObject);
                    Destroy(gems[i + 3].gameObject);
                    Destroy(gems[i + 4].gameObject);
                    setGemColor(i, posX, posY, posZ);
                    setGemColor(i + 1, posX + 1.3f, posY, posZ);
                    setGemColor(i + 2, posX + (2 * 1.3f), posY, posZ);
                    setGemColor(i + 3, posX + (3 * 1.3f), posY, posZ);
                    setGemColor(i + 4, posX + (4 * 1.3f), posY, posZ);
                }
            }
            
            if (nRo < Row - 2)
            {
                if (gemsColor[i] == gemsColor[i + Column] && gemsColor[i] == gemsColor[i + (2 * Column)])
                {
                    Destroy(gems[i].gameObject);
                    Destroy(gems[i + Column].gameObject);
                    Destroy(gems[i + (2 * Column)].gameObject);
                    setGemColor(i, posX, posY, posZ);
                    setGemColor(i + Column, posX, posY - 1.14f, posZ);
                    setGemColor(i + (2 * Column), posX, posY - (2 * 1.14f), posZ);
                }
            }
            else if (nRo < Row - 3)
            {
                if (gemsColor[i] == gemsColor[i + Column] && gemsColor[i] == gemsColor[i + (2 * Column)] && gemsColor[i] == gemsColor[i + (3 * Column)])
                {
                    Destroy(gems[i].gameObject);
                    Destroy(gems[i + Column].gameObject);
                    Destroy(gems[i + (2 * Column)].gameObject);
                    Destroy(gems[i + (3 * Column)].gameObject);
                    setGemColor(i, posX, posY, posZ);
                    setGemColor(i + Column, posX, posY - 1.14f, posZ);
                    setGemColor(i + (2 * Column), posX, posY - (2 * 1.14f), posZ);
                    setGemColor(i + (3 * Column), posX, posY - (3 * 1.14f), posZ);
                }
            }
            else if (nRo < Row - 4)
            {
                if (gemsColor[i] == gemsColor[i + Column] && gemsColor[i] == gemsColor[i + (2 * Column)] && gemsColor[i] == gemsColor[i + (3 * Column)] && gemsColor[i] == gemsColor[i + (4 * Column)])
                {
                    Destroy(gems[i].gameObject);
                    Destroy(gems[i + Column].gameObject);
                    Destroy(gems[i + (2 * Column)].gameObject);
                    Destroy(gems[i + (3 * Column)].gameObject);
                    Destroy(gems[i + (4 * Column)].gameObject);
                    setGemColor(i, posX, posY, posZ);
                    setGemColor(i + Column, posX, posY - 1.14f, posZ);
                    setGemColor(i + (2 * Column), posX, posY - (2 * 1.14f), posZ);
                    setGemColor(i + (3 * Column), posX, posY - (3 * 1.14f), posZ);
                    setGemColor(i + (4 * Column), posX, posY - (4 * 1.14f), posZ);
                }
            }

            // move to next position in column and increase current column number
            if ((i + 1) % Column != 0 || i == 0)
            {
                nCo++;

                posX += 1.3f;
            }
            // if can divide by Column that mean already finish that row, also not count 0
            else if ((i + 1) % Column == 0)
            {
                nRo++;

                if (nRo >= Row)
                {
                    nRo = 0;

                    posX = -7.15f;
                    posY = 4.05f;
                }
                else
                {
                    posX = -7.15f;
                    posY -= 1.14f;
                }

                nCo = 0;
            }
        }
    }

    void setGemColor(int i,float newPosX,float newPosY,float newPosZ)
    {
        int color = Random.Range(1, 7);

        pos = new Vector3(newPosX, newPosY, newPosZ);

        if (color == 1)
        {
            gems[i] = Instantiate(redGem, pos, Quaternion.identity) as GameObject;
            gemsColor[i] = GemColor.red;
        }
        else if (color == 2)
        {
            gems[i] = Instantiate(blueGem, pos, Quaternion.identity) as GameObject;
            gemsColor[i] = GemColor.blue;
        }
        else if (color == 3)
        {
            gems[i] = Instantiate(greenGem, pos, Quaternion.identity) as GameObject;
            gemsColor[i] = GemColor.green;
        }
        else if (color == 4)
        {
            gems[i] = Instantiate(yellowGem, pos, Quaternion.identity) as GameObject;
            gemsColor[i] = GemColor.yellow;
        }
        else if (color == 5)
        {
            gems[i] = Instantiate(whiteGem, pos, Quaternion.identity) as GameObject;
            gemsColor[i] = GemColor.white;
        }
        else if (color == 6)
        {
            gems[i] = Instantiate(greyGem, pos, Quaternion.identity) as GameObject;
            gemsColor[i] = GemColor.grey;
        }
        else if (color == 7)
        {
            gems[i] = Instantiate(brownGem, pos, Quaternion.identity) as GameObject;
            gemsColor[i] = GemColor.brown;
        }
    }

    void checkMoveable()
    {
        for (int i = 0; i < gems.Length; i++)
        {

        }
    }
}
