using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AllScoreController : MonoBehaviour
{

    public TextMeshProUGUI[] highScoreEntries;
    public RandomUserNameContainer randomUsernames;

    private List<string> usernameList;

    private void Awake()
    {
        usernameList = randomUsernames.userNames.ToList();
    }

    public string PopRandomUsername()
    {
        int index = Random.Range(0, usernameList.Count);
        string username = usernameList[index];
        usernameList.RemoveAt(index);

        return username;

    }

}
