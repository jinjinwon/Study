// GOAPPlanner.cs
using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    public Queue<GOAPAction> Plan(GameObject agent, HashSet<GOAPAction> actions,
                                  Dictionary<string, object> worldState,
                                  Dictionary<string, object> goal)
    {
        var leaves = new List<Node>();
        var start = new Node(null, 0, worldState, null);
        bool success = BuildGraph(agent, start, leaves, actions, goal);
        if (!success) return null;

        // 최소 비용 leaf 선택
        Node cheapest = null;
        foreach (var leaf in leaves)
        {
            if (cheapest == null || leaf.runningCost < cheapest.runningCost)
                cheapest = leaf;
        }

        // 경로 역추적
        var result = new List<GOAPAction>();
        var n = cheapest;
        while (n != null && n.action != null)
        {
            result.Insert(0, n.action);
            n = n.parent;
        }
        return new Queue<GOAPAction>(result);
    }

    private bool BuildGraph(GameObject agent, Node parent, List<Node> leaves,
                            HashSet<GOAPAction> actions,
                            Dictionary<string, object> goal)
    {
        bool foundOne = false;
        foreach (var action in actions)
        {
            // 1) 플래닝 단계에선 씬을 보지 않는다
            int foodCount = (int)parent.state["FoodCount"];
            if (foodCount <= 0) continue;  // Food가 남아 있어야만 적용 가능

            // 2) 시뮬레이션된 상태 전이
            var currentState = new Dictionary<string, object>(parent.state);
            currentState["FoodCount"] = foodCount - 1;

            var node = new Node(parent, parent.runningCost + action.cost, currentState, action);

            // 3) 목표 달성 검사
            if (InState(goal, currentState))
            {
                leaves.Add(node);
                foundOne = true;
            }
            else
            {
                // 액션 재사용 허용 → actions 그대로 넘김
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

    private class Node
    {
        public Node parent;
        public float runningCost;
        public Dictionary<string, object> state;
        public GOAPAction action;
        public Node(Node p, float cost, Dictionary<string, object> s, GOAPAction a)
        {
            parent = p; runningCost = cost; state = s; action = a;
        }
    }
}
