using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityHelp.UI.InfiniteScroll;

public class TestInfiniteManager : MonoBehaviour
{
    [SerializeField] private InfiniteScrollView infiniteScrollView;
    private string removeIndex = "0";
    private string snapIndex = "0";

    public Text heightInstrument;
    public float baseCellHeight = 20;
    public string myName = "HowTungTung";
    private string speaker = "Tester";
    private string message = "In a recent blog post we introduced the concept of Scriptable Render Pipelines. In short, SRP allow developers to control how Unity renders a frame in C#. We will release two built-in render pipelines with Unity 2018.1: the Lightweight Pipeline and HD Pipeline. In this article we¡¯re going to focus on the Lightweight Pipeline or LWRP.";

    private void Awake()
    {
        infiniteScrollView.onCellSelected += OnCellSelected;
    }

    private void OnCellSelected(InfiniteCell selectedCell)
    {
        Debug.Log("On Cell Selected " + selectedCell.CellData.index);
    }

    private void OnGUI()
    {
        if (infiniteScrollView != null)
        {
            if (GUILayout.Button("Add 1000 Cell Horizontal"))
            {
                for (int i = 0; i < 1000; i++)
                {
                    infiniteScrollView.Add(new InfiniteCellData(new Vector2(50, 0)));
                }
                infiniteScrollView.Refresh();
            }
            if (GUILayout.Button("Add Horizontal"))
            {
                var data = new InfiniteCellData(new Vector2(50, 0));
                infiniteScrollView.Add(data);
                infiniteScrollView.Refresh();
                infiniteScrollView.SnapLast(0.1f);
            }
            if (GUILayout.Button("Add 1000 Cell Vertical"))
            {
                for (int i = 0; i < 1000; i++)
                {
                    infiniteScrollView.Add(new InfiniteCellData(new Vector2(100, 100)));
                }
                infiniteScrollView.Refresh();
            }
            if (GUILayout.Button("Add Vertical"))
            {
                var data = new InfiniteCellData(new Vector2(100, 100));
                infiniteScrollView.Add(data);
                infiniteScrollView.Refresh();
                infiniteScrollView.SnapLast(0.1f);
            }
            GUILayout.Label("Remove Index");
            removeIndex = GUILayout.TextField(removeIndex);
            if (GUILayout.Button("Remove"))
            {
                infiniteScrollView.Remove(int.Parse(removeIndex));
            }
            GUILayout.Label("Snap Index");
            snapIndex = GUILayout.TextField(snapIndex);
            if (GUILayout.Button("Snap"))
            {
                infiniteScrollView.Snap(int.Parse(snapIndex), 0.1f);
            }

            GUILayout.Label("Speaker");
            speaker = GUILayout.TextField(speaker);
            GUILayout.Label("Message");
            message = GUILayout.TextArea(message, GUILayout.MaxWidth(300), GUILayout.MaxHeight(100));
            if (GUILayout.Button("Add"))
            {
                AddChatData(new ChatCellData(speaker, message, false));
            }
        }
    }

    public void OnSubmit(string input)
    {
        AddChatData(new ChatCellData(myName, input, true));
    }

    private void AddChatData(ChatCellData chatCellData)
    {
        heightInstrument.text = chatCellData.message;
        var infiniteData = new InfiniteCellData(new Vector2(0, heightInstrument.preferredHeight + baseCellHeight), chatCellData);
        infiniteScrollView.Add(infiniteData);
        infiniteScrollView.Refresh();
        infiniteScrollView.SnapLast(0.1f);
    }
}
