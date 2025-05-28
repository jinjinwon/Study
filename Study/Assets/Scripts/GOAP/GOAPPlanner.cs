using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    public Queue<GOAPAction> Plan(GameObject agent,HashSet<GOAPAction> actions,Dictionary<string, object> worldState,Dictionary<string, object> goal)
    {
        var leaves = new List<Node>();
        var start = new Node(null, 0f, worldState, null);
        bool success = BuildGraph(agent, start, leaves, actions, goal);
        if (!success) return null;

        // ���� ��� leaf ����
        Node cheapest = null;
        foreach (var leaf in leaves)
        {
            if (cheapest == null || leaf.runningCost < cheapest.runningCost)
                cheapest = leaf;
        }

        // ��� ������
        var result = new List<GOAPAction>();
        var n = cheapest;
        while (n != null && n.action != null)
        {
            result.Insert(0, n.action);
            n = n.parent;
        }
        return new Queue<GOAPAction>(result);
    }

    private bool BuildGraph(GameObject agent,Node parent,List<Node> leaves,HashSet<GOAPAction> actions,Dictionary<string, object> goal)
    {
        bool foundOne = false;
        foreach (var action in actions)
        {
            // ���� Preconditions�� �˻�
            if (!InState(action.preconditions, parent.state))
                continue;

            // �ùķ��̼ǿ� ���� ����
            var currentState = new Dictionary<string, object>(parent.state);

            // effects Ű�� ���� EnemyCount�� ���� ���� �ùķ��̼�
            foreach (var e in action.effects)
            {
                if (e.Key == "EnemyCount" && parent.state.ContainsKey("EnemyCount"))
                {
                    int prev = (int)parent.state["EnemyCount"];
                    currentState["EnemyCount"] = prev - 1;
                }
                else
                {
                    currentState[e.Key] = e.Value;
                }
            }

            // �� ��� ����
            var node = new Node(parent, parent.runningCost + action.cost, currentState, action);
            if (InState(goal, currentState))
            {
                leaves.Add(node);
                foundOne = true;
            }
            else
            {
                if (BuildGraph(agent, node, leaves, actions, goal))
                    foundOne = true;
            }
        }
        return foundOne;
    }


    private bool InState(Dictionary<string, object> test, Dictionary<string, object> state)
    {
        foreach (var t in test)
        {
            if (!state.ContainsKey(t.Key) || !state[t.Key].Equals(t.Value))
                return false;
        }
        return true;
    }

    // Ž�� Ʈ�� ���
    private class Node
    {
        public Node parent;
        public float runningCost;
        public Dictionary<string, object> state;
        public GOAPAction action;
        public Node(Node p, float cost, Dictionary<string, object> s, GOAPAction a)
        {
            parent = p;
            runningCost = cost;
            state = s;
            action = a;
        }
    }
}
