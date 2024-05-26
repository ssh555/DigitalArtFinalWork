using System.Collections.Generic;
using Common;
using PathSystem.NodesScript;
using UnityEngine;

namespace PathSystem
{
    public class PathView
    {
        //List<int> shortestPath;
        GameObject line;
        List<GateControllerView> physicalGates = new List<GateControllerView>();
        List<GameObject> physicalHighlightedNodes = new List<GameObject>();
        List<GameObject> physicalPath = new List<GameObject>();
        List<GameObject> physicalNode = new List<GameObject>();
        NodeControllerView nodeprefab, targetNode, teleportNode;
        // int shortestPathLength;
        IPathService pathService;
        //List<StarTypes> Stars;
        public PathView(IPathService pathService)
        {
            this.pathService = pathService;
        }
        [SerializeField] List<Node> graph = new List<Node>();
        public int DrawGraph(ScriptableGraph Graph)
        {
            nodeprefab = Graph.nodeprefab;
            targetNode = Graph.targetNode;
            teleportNode = Graph.teleportNode;
            SetGates(Graph.GatesEdge);
            line = Graph.line;
            for (int i = 0; i < Graph.Graph.Count; i++)
            {
                Node node = new Node();
                node.node = Graph.Graph[i].node;
                node.teleport = Graph.Graph[i].teleport;
                node.connections = Graph.Graph[i].GetConnections();
                graph.Add(node);

                if (graph[i].node.property == NodeProperty.TARGETNODE)
                {
                    targetNode.SetNodeID(i);
                    physicalNode.Add(GameObject.Instantiate(targetNode.gameObject, new Vector3(node.node.nodePosition.x, node.node.nodePosition.y - 0.195f, node.node.nodePosition.z), Quaternion.identity));
                }
                else if (graph[i].node.property == NodeProperty.TELEPORT)
                {
                    teleportNode.SetNodeID(i);
                    physicalNode.Add(GameObject.Instantiate(teleportNode.gameObject, new Vector3(node.node.nodePosition.x, node.node.nodePosition.y - 0.195f, node.node.nodePosition.z), Quaternion.identity));
                }
                else
                {
                    nodeprefab.SetNodeID(i);
                    physicalNode.Add(GameObject.Instantiate(nodeprefab.gameObject, new Vector3(node.node.nodePosition.x, node.node.nodePosition.y - 0.195f, node.node.nodePosition.z), Quaternion.identity));
                }
                if (graph[i].node.snipeable)
                {

                    physicalNode[physicalNode.Count - 1].GetComponent<NodeControllerView>().SetShootable();
                }
                if (node.connections[0] != -1)
                {
                    physicalPath.Add(GameObject.Instantiate(line, new Vector3(node.node.nodePosition.x, node.node.nodePosition.y - 0.195f, node.node.nodePosition.z - 2.5f), Quaternion.Euler(new Vector3(0, 90, 0))));
                }
                if (node.connections[2] != -1)
                {
                    physicalPath.Add(GameObject.Instantiate(line, new Vector3(node.node.nodePosition.x + 2.5f, node.node.nodePosition.y - 0.195f, node.node.nodePosition.z), new Quaternion(0, 0, 0, 0)));
                }
            }
            return graph.Count;

        }
        public void DrawPath(int dir, Vector3 nodePosition)
        {
            if (dir == 0)
            {
                physicalPath.Add(GameObject.Instantiate(line, new Vector3(nodePosition.x, nodePosition.y - 0.195f, nodePosition.z - 2.5f), Quaternion.Euler(new Vector3(0, 90, 0))));
            }
            if (dir == 2)
            {
                physicalPath.Add(GameObject.Instantiate(line, new Vector3(nodePosition.x + 2.5f, nodePosition.y - 0.195f, nodePosition.z), new Quaternion(0, 0, 0, 0)));
            }
        }
        public void ShowTeleportableNodes(List<int> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                physicalHighlightedNodes.Add(physicalNode[nodes[i]]);
                physicalNode[nodes[i]].GetComponent<NodeControllerView>().HighlightNode();
            }
        }
        public void DestroyPath()
        {
            graph = new List<Node>();
            for (int i = 0; i < physicalPath.Count; i++)
            {
                GameObject.DestroyImmediate(physicalPath[i]);
            }
            for (int i = 0; i < physicalNode.Count; i++)
            {
                GameObject.DestroyImmediate(physicalNode[i]);
            }
            for (int i = 0; i < physicalGates.Count; i++)
            {
                GameObject.DestroyImmediate(physicalGates[i].gameObject);
            }
            physicalGates = new List<GateControllerView>();
            physicalPath = new List<GameObject>();
            physicalNode = new List<GameObject>();
        }
        public void Unhighlightnodes()
        {
            for (int i = 0; i < physicalHighlightedNodes.Count; i++)
            {
                physicalHighlightedNodes[i].GetComponent<NodeControllerView>().UnHighlightNode();
            }
            physicalHighlightedNodes = new List<GameObject>();
        }
        public void ShowAlertedNodes(int nodeId)
        {
            Unhighlightnodes();
            physicalNode[nodeId].GetComponent<NodeControllerView>().ShowAlertedNodes();
        }
        public void ShowThrowableNodes(int nodeId)
        {
            Vector3 playerpos = graph[nodeId].node.nodePosition;
            for (int i = 0; i < graph.Count; i++)
            {
                Vector3 testpos = graph[i].node.nodePosition;
                if ((playerpos.x + 5 == testpos.x || playerpos.x - 5 == testpos.x) && playerpos.z == testpos.z)
                {
                    physicalHighlightedNodes.Add(physicalNode[i]);
                    physicalNode[i].GetComponent<NodeControllerView>().HighlightNode();
                }
                if ((playerpos.z + 5 == testpos.z || playerpos.z - 5 == testpos.z) && playerpos.x == testpos.x)
                {
                    physicalHighlightedNodes.Add(physicalNode[i]);
                    physicalNode[i].GetComponent<NodeControllerView>().HighlightNode();
                }
            }
        }
        void SetGates(List<GateData> gates)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                GateControllerView gate = GameObject.Instantiate(gates[i].gatePrefab, gates[i].position, Quaternion.identity).GetComponent<GateControllerView>();
                physicalGates.Add(gate);
                gate.gameObject.transform.LookAt(new Vector3(pathService.GetNodeLocation(gates[i].node1).x, gate.gameObject.transform.position.y, pathService.GetNodeLocation(gates[i].node1).z));
                gate.SetGate(gates[i].key, gates[i].node1, gates[i].node2);
            }
        }
        public void KeyCollected(KeyTypes key)
        {

            for (int i = 0; i < physicalGates.Count; i++)
            {
                while (i < physicalGates.Count && physicalGates[i].GetKey() == key)
                {
                    
                    physicalGates[i].Disable();
                    physicalGates.RemoveAt(i);
                }
            }
        }
    }
}