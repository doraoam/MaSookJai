using UnityEngine;
using System.Collections;

public class Checker : MonoBehaviour {

    public RaycastHit hit;
    public Ray ray;

    int Column = 8;
    int Row = 8;

    GameObject board;

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
    bool destroyable = false;

    GameObject selectGem;
    GameObject targetGem;

    bool selectGemEmpty;
    bool targetGemEmpty;

    int selectArray;
    int targetArray;

    Vector3 selectGemPosition;
    Vector3 targetGemPosition;

    // All color of gem
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

    // create a begin board until the new board can moveable
    void Awake()
    {
        gems = new GameObject[Column * Row];
        gemsColor = new GemColor[Column * Row];

        selectGemEmpty = true;
        targetGemEmpty = true;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        board = new GameObject("Board");

        createABoard();

        destroyable = true;

        while (destroyable)
        {
            checkMatch();
        }

        while (!moveable)
        {
            checkMoveable();
        }
    }

    // create a board
    void createABoard()
    {
        for (int i = 0; i < gems.Length; i++)
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

            gems[i].gameObject.layer = LayerMask.NameToLayer("Gem");
            gems[i].gameObject.transform.SetParent(board.transform);

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

    // always check if current board moveable or not if not create a new board 
    void Update()
    {
        // if moveable == true
        // if player move --> checkMatch()
        // else (not moveable) --> call a new board

        if (!moveable)
        {
            createABoard();

            destroyable = true;

            while (destroyable)
            {
                checkMatch();
            }

            while (!moveable)
            {
                checkMoveable();
            }
            Debug.Log("work");
        }
      
        selection();
    }

    // control gem selection
    void selection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Gem")))
            {
                if (selectGemEmpty)
                {
                    selectGem = hit.collider.gameObject;
                    selectArray = findPositionInArray(selectGem.transform.position.x, selectGem.transform.position.y);
                    selectGemPosition = hit.transform.position;
                    selectGemEmpty = false;
                    Debug.Log(gemsColor[selectArray]);
                }
                else if (targetGemEmpty)
                {
                    targetGem = hit.collider.gameObject;
                    targetArray = findPositionInArray(targetGem.transform.position.x, targetGem.transform.position.y);
                    targetGemPosition = hit.transform.position;
                    targetGemEmpty = false;
                    Debug.Log(gemsColor[targetArray]);

                    if (checkGivenMatch(selectArray, targetArray))
                    {
                        Vector3 temp = selectGemPosition;
                        selectGemPosition = targetGemPosition;
                        targetGemPosition = temp;

                        checkMatch();
                        Debug.Log("pass");
                    }
                    else
                    {
                        // error not allow this move
                        Debug.Log("error");
                    }

                    selectGemEmpty = true;
                    targetGemEmpty = true;
                }
                //else if (!selectGemEmpty && !targetGemEmpty)
                //{
                //    if (checkGivenMatch(selectArray, targetArray))
                //    {
                //        Vector3 temp = selectGemPosition;
                //        selectGemPosition = targetGemPosition;
                //        targetGemPosition = temp;

                //        checkMatch();
                //        Debug.Log("pass");
                //    }
                //    else
                //    {
                //        // error not allow this move
                //        Debug.Log("error");
                //    }

                //    selectGemEmpty = true;
                //    targetGemEmpty = true;
                //}
            }
        }
    }

    // Find position in array
    int findPositionInArray(float x, float y)
    {
        int arrayPos = 0;
        float fakeX = -7.15f;
        
        if (y <= 4.04 && y > 2.9)
        {
            arrayPos = 0;
        }
        else if (y <= 2.9 && y > 1.76)
        {
            arrayPos = 8;
        }
        else if (y <= 1.76 && y > 0.62)
        {
            arrayPos = 16;
        }
        else if (y <= 0.62 && y > -0.5)
        {
            arrayPos = 24;
        }
        else if (y <= -0.5 && y > -1.64)
        {
            arrayPos = 32;
        }
        else if (y <= -1.64 && y > -2.78)
        {
            arrayPos = 40;
        }
        else if (y <= -2.78 && y > -3.92)
        {
            arrayPos = 48;
        }
        else if (y <= -3.92)
        {
            arrayPos = 56;
        }
        
        while (fakeX != x && fakeX < 2)
        {
            arrayPos++;

            if (fakeX >= 0.6)
            {
                fakeX += 1.4f;
            }
            else
            {
                fakeX += 1.3f;
            }
        }

        return arrayPos;
    }

    /* check if moved gem match to another or not
    also replace i of origin checkMatch with arrayPoint
    + tell can move or not */
    bool checkGivenMatch(int arrayPoint,int targetPoint)
    {
        if (targetPoint - arrayPoint == 1)
        {
            if(targetPoint%Column < Column - 2)
            {
                if (gemsColor[arrayPoint] == gemsColor[targetPoint + 1] && gemsColor[arrayPoint] == gemsColor[targetPoint + 2])
                {
                    return true;
                }
            }
        }
        else if (arrayPoint - targetPoint == 1)
        {
            if (targetPoint % Column >= 2)
            {
                if (gemsColor[arrayPoint] == gemsColor[targetPoint - 1] && gemsColor[arrayPoint] == gemsColor[targetPoint - 2])
                {
                    return true;
                }
            }
        }
        else if (arrayPoint - targetPoint == Row)
        {
            if (targetPoint >= Row * 2)
            {
                if (gemsColor[arrayPoint] == gemsColor[targetPoint - Row] && gemsColor[arrayPoint] == gemsColor[targetPoint - (Row * 2)])
                {
                    return true;
                }
            }
        }
        else if (targetPoint - arrayPoint == Row)
        {
            if (targetPoint < Row * (Row - 2))
            {
                if (gemsColor[arrayPoint] == gemsColor[targetPoint + Row] && gemsColor[arrayPoint] == gemsColor[targetPoint + (Row * 2)])
                {
                    return true;
                }
            }
        }

        return false;
    }

    void checkMatch()
    {
        int count = 0;

        for (int i = 0; i < gems.Length; i++)
        {
            if (nCo < Column - 2)
            {
                if (gemsColor[i] == gemsColor[i + 1] && gemsColor[i] == gemsColor[i + 2])
                {
                    Destroy(gems[i].gameObject);
                    Destroy(gems[i + 1].gameObject);
                    Destroy(gems[i + 2].gameObject);
                    setGemColor(i, posX, posY, posZ);
                    setGemColor(i + 1, posX + 1.3f, posY, posZ);
                    setGemColor(i + 2, posX + (2 * 1.3f), posY, posZ);

                    count++;
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

                    count++;
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

                    count++;
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

                    count++;
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

                    count++;
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

                    count++;
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

            if (count == 0 && i == gems.Length - 1)
            {
                destroyable = false;
            }
        }
    }

    // create a new gem with given position and random color also save in given position in array
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

        gems[i].gameObject.layer = LayerMask.NameToLayer("Gem");
        gems[i].gameObject.transform.SetParent(board.transform);
    }

    // check that current board can move or not
    void checkMoveable()
    {
        for (int i = 0; i < gems.Length; i++)
        {
            if (nCo < Column - 3)
            {
                if (gemsColor[i] == gemsColor[i + 2] && gemsColor[i] == gemsColor[i + 3])
                {
                    moveable = true;
                }
            }

            if (nCo > 3)
            {
                if (gemsColor[i] == gemsColor[i - 2] && gemsColor[i] == gemsColor[i - 3])
                {
                    moveable = true;
                }
            }

            if (nRo < Row - 3)
            {
                if(gemsColor[i] == gemsColor[i + (2 * Column)] && gemsColor[i] == gemsColor[i + (3 * Column)])
                {
                    moveable = true;
                }
            }

            if(nRo > 3)
            {
                if(gemsColor[i] == gemsColor[i - (2 * Column)] && gemsColor[i] == gemsColor[i - (3 * Column)])
                {
                    moveable = true;
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
}
