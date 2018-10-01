//2017 Jordan Black

//Description: A kind of messenger for connected local clients. Also has some command functionality.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour {

    public string consoleTextTag = "ConsoleText";
    public GameObject consoleTextPrefab;
    public float textVisibleForTime = 5f;

    public GameObject lobbyPanel;
    public InputField consoleInputField;

    private List<GameObject> consoleTextObjects = new List<GameObject>();
    private int activeConsoleObjects;

    private NetworkLobby lobby;


    private void Start()
    {
        lobby = GetComponent<NetworkLobby>();

        consoleTextObjects.AddRange(GameObject.FindGameObjectsWithTag(consoleTextTag));
        activeConsoleObjects = consoleTextObjects.Count;

        consoleInputField.onEndEdit.AddListener(delegate { CreateConsoleText(consoleInputField.text); });

        StartCoroutine(FadeOut(consoleTextObjects[0]));
    }

    public void CreateConsoleText(string msg)
    {
        if(consoleInputField.text != "" && Input.GetButtonDown("Submit"))
        {

            GameObject g = Instantiate(consoleTextPrefab);
            g.GetComponent<Text>().text = msg;
            g.transform.SetParent(lobbyPanel.transform);
            g.transform.position = consoleTextObjects[0].transform.position + Vector3.down * 30 * activeConsoleObjects;

            consoleTextObjects.Add(g);
            activeConsoleObjects++;

            ParseText(msg);

            consoleInputField.text = "";

            StartCoroutine(FadeOut(g));
        }
    }

    void ParseText(string s)
    {
        //s = s.ToLower();
        //s = s.Trim();
        if (s == "/load")
        {
            Debug.Log("Beginning service...");
            lobby.BeginService();
            CreateConsoleText("Loading network services...");
        }

        if (s == "/stop")
        {
            Debug.Log("Stopping service...");
            lobby.StopService();
        }

        if(s == "/connect")
        {
            Debug.Log("Connecting...");
            lobby.JoinLobby();
        }

        if (s == "Hello")
        {
            lobby.CmdSendConsoleMessage(s);
        }
    }

    IEnumerator FadeOut(GameObject g)
    {
        Text t = g.GetComponent<Text>();
        Color c = t.color;
        float alphaLevel = c.a;

        yield return new WaitForSeconds(textVisibleForTime);

        while (alphaLevel > 0f)
        {
            alphaLevel -= (0.01f);

            t.color = new Color(c.r, c.g, c.b, alphaLevel);

            yield return new WaitForEndOfFrame();
        }

        if (alphaLevel < 0f)
        {
            alphaLevel = 0f;
            t.color = new Color(c.r, c.g, c.b, alphaLevel);
        }

        g.SetActive(false);

        if (activeConsoleObjects > 1)
        {
            StartCoroutine(MoveTextUp());

            //consoleTextObjects.RemoveAt(0);
            //consoleTextObjects.TrimExcess();
            
        }

        activeConsoleObjects -= 1;
        yield return null;

    }

    IEnumerator MoveTextUp()
    {
        for(int i = 1; i < consoleTextObjects.Count; i++)
        {
            consoleTextObjects[i].transform.position += Vector3.up * 30;
        }


        yield return null;
    }

}
