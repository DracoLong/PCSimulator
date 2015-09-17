using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using MDPSimulator.View;
using System.Collections;
using MDPSimulator;
namespace MDPModel
{
    public class Robot
    {
        private volatile bool _shouldStop;
        private bool shortestPathComputed;
        public delegate void RobotMovingHandler(int x, int y);
        public delegate void RobotSendingMessage(string s);
        public event RobotMovingHandler ChangePosition;
        public event RobotSendingMessage SendingMessage;
        public double CurrentCoverage { get; set; }
        private int x, y;
        public bool isExplored = false;
        public Node StartNode { get; set; }
        public Node GoalNode { get; set; }
        public DFSNode StartDFSNode { get; set; }
        public DFSNode GoalDFSNode { get; set; }
        public int XStart { get; set; }
        public int YStart { get; set; }
        public int XGoal { get; set; }
        public int YGoal { get; set; }
        public List<Node> ShortestPath { get; set; }
        public int X
        {
            get { return x; }
            set
            {
                x = value;
                OnPositionChanged();
            }
        }
        public Stack stack { get; set; }
        public List<DFSNode> DFSNodes { get; set; }
        public List<Node> priorityNodes { get; set; }
        public int Y
        {
            get { return this.y; }
            set
            {
                this.y = value;
                OnPositionChanged();
            }
        }
        public int Dir { get; set; }

        public int Range { get; set; }

        public Map Env { get; set; }
        public Map VirtualMap { get; set; }
        public Map Memory { get; set; }

        public Robot(int x, int y, int r, char d)
        {
            this.X = x;
            this.Y = y;
            this.Range = r;
            this.Dir = d;
            //empty memory for robot
            this.Memory = new Map();
            this.XGoal = 13;
            this.YGoal = 18;
            this.XStart = 1;
            this.YStart = 1;
            this.StartNode = new Node(this.XStart, this.YStart);
            this.GoalNode = new Node(this.XGoal, this.YGoal);
            this.ShortestPath = new List<Node>();
            this.VirtualMap = new Map();
        }

        public Robot()
        {
            this.X = 1;
            this.Y = 1;
            this.Range = 1;
            this.Dir = 'U';
            this.Memory = new Map();
            this.XGoal = 13;
            this.YGoal = 18;
            this.XStart = 1;
            this.YStart = 1;
            this.StartNode = new Node(this.XStart, this.YStart);
            this.GoalNode = new Node(this.XGoal, this.YGoal);
            this.StartDFSNode = new DFSNode(this.XStart, this.YStart);
            this.GoalDFSNode = new DFSNode(this.XGoal, this.YGoal);
            this.ShortestPath = new List<Node>();
            this.VirtualMap = new Map();
            this.DFSNodes = new List<DFSNode>();
            this.priorityNodes = new List<Node>();
            this.CurrentCoverage = 0;
        }


        public void turnLeft()
        {
            switch (this.Dir)
            {
                case 'U':
                    this.Dir = 'L';
                    break;
                case 'D':
                    this.Dir = 'R';
                    break;
                case 'R':
                    this.Dir = 'U';
                    break;
                case 'L':
                    this.Dir = 'D';
                    break;
                default:
                    break;
            }
        }

        public void turnRight()
        {
            switch (this.Dir)
            {
                case 'U':
                    this.Dir = 'R';
                    break;
                case 'D':
                    this.Dir = 'L';
                    break;
                case 'R':
                    this.Dir = 'D';
                    break;
                case 'L':
                    this.Dir = 'U';
                    break;
                default:
                    break;
            }
        }

        public void turnAround()
        {
            switch (this.Dir)
            {
                case 'U':
                    this.Dir = 'D';
                    break;
                case 'D':
                    this.Dir = 'U';
                    break;
                case 'R':
                    this.Dir = 'L';
                    break;
                case 'L':
                    this.Dir = 'R';
                    break;
                default:
                    break;
            }
        }


        public void moveForward(int dis)
        {
            switch (this.Dir)
            {
                case 'U':
                    this.Y += dis;
                    break;
                case 'D':
                    this.Y -= dis;
                    break;
                case 'R':
                    this.X++;
                    break;
                case 'L':
                    this.X -= dis;
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnPositionChanged()
        {
            if (ChangePosition != null)
            {
                ChangePosition(this.X, this.Y);
            }
            else
            {
                Console.WriteLine("Position changed!");
            }
        }

        protected virtual void OnSendingMessage(string s)
        {
            if (SendingMessage != null)
            {
                SendingMessage(s);
            }
            else
            {
                Console.WriteLine(s);
            }
        }

        public void wallfollowerExplore()
        {
            bool isBlockedLeft, isBlockedRight, isBlockedFront;
            //turnRight();
            bool moved = false;
            while (!_shouldStop)
            {
                try
                {
                    do
                    {
                        computeCoverage();
                        OnSendingMessage("Current X = " + this.X + " Y = " + this.Y);
                        isBlockedFront = scanFront();
                        isBlockedLeft = scanLeft();
                        isBlockedRight = scanRight();
                        if (!isBlockedRight)
                        {
                            Console.WriteLine("Turn right");
                            turnRight();
                        }
                        else if (!isBlockedFront)
                        {
                            Console.WriteLine("Move");
                            moved = true;
                        }
                        else if (!isBlockedLeft)
                        {
                            Console.WriteLine("Turn left");
                            turnLeft();
                        }
                        else
                        {
                            Console.WriteLine("Turn around");
                            turnAround();
                        }
                        Thread.Sleep(1000 / UserSetting.Speed);
                        moveForward(1);
                    } while ((this.X != 1 || this.Y != 1) || !moved);
                    this._shouldStop = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e);
                }
            }
            this.isExplored = true;
        }

        public bool scanFront()
        {
            switch (this.Dir)
            {
                case 'R':
                    return checkRightSide();
                case 'L':
                    return checkLeftSide();
                case 'U':
                    return checkTopSide();
                case 'D':
                    return checkBottomSide();
                default:
                    return true;
            }
        }

        public bool scanLeft()
        {
            switch (this.Dir)
            {
                case 'R':
                    return checkTopSide();
                case 'L':
                    return checkBottomSide();
                case 'U':
                    return checkLeftSide();
                case 'D':
                    return checkRightSide();
                default:
                    return true;
            }
        }

        public bool scanRight()
        {
            switch (this.Dir)
            {
                case 'R':
                    return checkBottomSide();
                case 'L':
                    return checkTopSide();
                case 'U':
                    return checkRightSide();
                case 'D':
                    return checkLeftSide();
                default:
                    return true;
            }
        }

        public bool checkTopSide()
        {
            bool isBlocked = false;
            if (this.Y + this.Range + 1 >= this.Env.Grid.GetLength(0))
                return true;
            int y = this.Y + this.Range + 1;
            for (int i = this.X - this.Range; i <= this.X + this.Range; i++)
            {
                if (this.Env.Grid[y, i].Status == 1)
                {
                    isBlocked = true;
                    //explored and has obstacle
                    this.Memory.Grid[y, i].Status = 1;
                }
                else
                {
                    //empty cell
                    this.Memory.Grid[y, i].Status = 2;
                }

            }
            return isBlocked;
        }


        public bool checkRightSide()
        {
            bool isBlocked = false;

            if (this.X + this.Range + 1 >= this.Env.Grid.GetLength(1))
                return true;
            int x = this.X + this.Range + 1;
            for (int i = this.Y - this.Range; i <= this.Y + this.Range; i++)
            {
                if (this.Env.Grid[i, x].Status == 1)
                {
                    isBlocked = true;
                    //explored and has obstacle
                    this.Memory.Grid[i, x].Status = 1;
                }
                else
                {
                    //empty cell
                    this.Memory.Grid[i, x].Status = 2;
                }
            }
            return isBlocked;
        }

        public bool checkLeftSide()
        {
            bool isBlocked = false;
            if (this.X - this.Range - 1 < 0)
                return true;
            int x = this.X - this.Range - 1;
            for (int i = this.Y - this.Range; i <= this.Y + this.Range; i++)
            {
                if (this.Env.Grid[i, x].Status == 1)
                {
                    isBlocked = true;
                    //explored and has obstacle
                    this.Memory.Grid[i, x].Status = 1;
                }
                else
                {
                    //empty cell
                    this.Memory.Grid[i, x].Status = 2;
                }
            }
            return isBlocked;
        }

        public bool checkBottomSide()
        {
            bool isBlocked = false;
            if (this.Y - this.Range - 1 < 0)
                return true;
            int y = this.Y - this.Range - 1;
            for (int i = this.X - this.Range; i <= this.X + this.Range; i++)
            {
                if (this.Env.Grid[y, i].Status == 1)
                {
                    isBlocked = true;
                    //explored and has obstacle
                    this.Memory.Grid[y, i].Status = 1;
                }
                else
                {
                    //empty cell
                    this.Memory.Grid[y, i].Status = 2;
                }
            }
            return isBlocked;
        }

        public void adjust()
        {
            switch (this.Dir)
            {
                case 'U':
                    break;
                case 'L':
                    this.turnRight();
                    break;
                case 'R':
                    this.turnLeft();
                    break;
                case 'D':
                    this.turnAround();
                    break;
                default:
                    break;
            }
        }
        public void fastestRun()
        {
            List<Node> closedSet = new List<Node>();
            List<Node> openSet = new List<Node>();
            List<Node> neighbors = new List<Node>();
            Node currentNode;
            this.adjust();
            this.StartNode.GCost = 0;
            this.StartNode.FCost = this.StartNode.GCost + computeH(this.StartNode);
            openSet.Add(this.StartNode);
            for (int i = 0; i < this.Memory.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < this.Memory.Grid.GetLength(1); j++)
                {
                    // if status is 0 (unexplored), regard it as obstacle
                    if (this.Memory.Grid[i, j].Status == 0)
                    {
                        this.Memory.Grid[i, j].Status = 1;
                    }
                }
            }
            computeVirtualMap();
            while (openSet.Count != 0)
            {
                openSet.Sort();
                Console.WriteLine("====open set=====");
                printListNode(openSet);

                currentNode = openSet[0];
                currentNode.print();
                if (currentNode.Equals(this.GoalNode))
                {
                    //comment out this section when running real time mapping
                    Console.WriteLine("Reached goal");
                    constructPath(currentNode);
                    this.shortestPathComputed = true;
                    break;

                    //this command is for computing fastest path in real time
                    this.ShortestPath.Add(currentNode);
                    this.shortestPathComputed = true;
                    return;
                }
                openSet.RemoveAt(0);
                closedSet.Add(currentNode);
                Node lastNode = currentNode.PreviousNode;
                neighbors = getNeighbors(currentNode, openSet);
                Console.WriteLine("==== neighbors =====");
                printListNode(neighbors);
                foreach (Node neighbor in neighbors)
                {
                    if (checkNodeInSet(neighbor, closedSet))
                        continue;
                    bool check = this.checkPriority(neighbor);
                    int benefit = 0;
                    if (check)
                        benefit = 2;
                    int tentativeGCost = currentNode.GCost + 1 + turningCost(lastNode, neighbor) - benefit;
                    bool inOpenSet = checkNodeInSet(neighbor, openSet);
                    if (!inOpenSet || tentativeGCost < neighbor.GCost)
                    {
                        neighbor.PreviousNode = currentNode;
                        neighbor.GCost = tentativeGCost;
                        neighbor.FCost = neighbor.GCost + neighbor.HCost;
                        if (!inOpenSet)
                            openSet.Add(neighbor);
                    }
                }
            }
            Console.WriteLine("out of while loop");
            if (!this.shortestPathComputed)
            {
                Console.WriteLine("This map has no solution");
                OnSendingMessage("This map has no solution");
            }
        }
        public int turningCost(Node last, Node next)
        {
            if (last == null)
            {
                return 0;
            }
            if (last.XNode != next.XNode && last.YNode != next.YNode)
                return 1;
            else return 0;
        }
        public void printListNode(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                node.print();
            }
        }
        private void constructPath(Node node)
        {
            this.ShortestPath.Add(node);
            Node temp = node.PreviousNode;
            while (!temp.Equals(this.StartNode) && temp != null)
            {
                this.ShortestPath.Add(temp);
                temp = temp.PreviousNode;
            }
            this.ShortestPath.Reverse();
            Console.WriteLine("========ShortestPath=========");
            OnSendingMessage("Conducting fastest run!!!");
            foreach (Node item in this.ShortestPath)
            {
                item.print();
                this.moveToNode(item);
                Thread.Sleep(1000 / UserSetting.Speed);
            }
            OnSendingMessage("Fastest run finished!!!");
        }

        private void moveToNode(Node nextnode)
        {
            this.Dir = nextnode.relativePosition(this.X, this.Y);
            this.X = nextnode.XNode;
            //this.Y = nextnode.YNode;
        }
        private List<Node> getNeighbors(Node currentNode, List<Node> L)
        {
            int x = currentNode.XNode;
            int y = currentNode.YNode;
            List<Node> neighbors = new List<Node>();
            //empty cell
            bool priority = false;
            if (this.VirtualMap.Grid[y, x + 1].Status == 2)
            {
                Node neighbor = new Node(x + 1, y);
                neighbor.HCost = computeH(neighbor);
                Node temp = getNodeInSet(neighbor, L);
                if (temp == null)
                {
                    neighbors.Add(neighbor);
                }
                else
                    neighbors.Add(temp);
            }
            if (this.VirtualMap.Grid[y, x - 1].Status == 2)
            {
                Node neighbor = new Node(x - 1, y);
                neighbor.HCost = computeH(neighbor);
                Node temp = getNodeInSet(neighbor, L);
                if (temp == null)
                {
                    neighbors.Add(neighbor);
                }
                else
                    neighbors.Add(temp);
            }
            if (this.VirtualMap.Grid[y - 1, x].Status == 2)
            {
                Node neighbor = new Node(x, y - 1);
                neighbor.HCost = computeH(neighbor);
                Node temp = getNodeInSet(neighbor, L);
                if (temp == null)
                {
                    neighbors.Add(neighbor);
                }
                else
                    neighbors.Add(temp);
            }
            if (this.VirtualMap.Grid[y + 1, x].Status == 2)
            {
                Node neighbor = new Node(x, y + 1);
                neighbor.HCost = computeH(neighbor);
                Node temp = getNodeInSet(neighbor, L);
                if (temp == null)
                {
                    neighbors.Add(neighbor);
                }
                else
                    neighbors.Add(temp);
            }
            return neighbors;
        }
        private void constructPriorityNodes()
        {

        }
        private bool checkPriority(Node node)
        {
            bool r1, r2, r3, d1, d2, d3;
            try
            {
                r1 = (this.Memory.Grid[node.YNode + 1, node.XNode + 2].Status == 1);
                r2 = (this.Memory.Grid[node.YNode, node.XNode +2].Status == 1);
                r3 = (this.Memory.Grid[node.YNode -1, node.XNode +2].Status == 1);

            }
            catch (Exception)
            {
                r1 = true;
                r2 = true;
                r3 = true;
            }
            try
            {
                d1 = (this.Memory.Grid[node.YNode - 2, node.XNode + 1].Status == 1);
                d2 = (this.Memory.Grid[node.YNode - 2, node.XNode].Status == 1);
                d3 = (this.Memory.Grid[node.YNode - 2, node.XNode - 1].Status == 1);
            }
            catch (Exception)
            {
                d1 = true;
                d2 = true;
                d3 = true;
            }
            
            if (r1 && r2 && r3 || d1 && d2 && d3)
            {
                return true;
            }
            return false;
        }
        private int computeH(Node node)
        {
            return 0;
            return (XGoal - node.XNode) + (YGoal - node.YNode);
        }

        private void computeVirtualMap()
        {
            for (int i = 0; i < Map.height; i++)
            {
                for (int j = 0; j < Map.width; j++)
                {
                    if (i == 0 || j == 0 || i == Map.height - 1 || j == Map.width - 1)
                    {
                        //virtual wall
                        this.VirtualMap.Grid[i, j].Status = 1;
                    }
                    if (this.Memory.Grid[i, j].Status == 1)
                    {
                        for (int m = i - 1; m <= i + 1; m++)
                        {
                            for (int n = j - 1; n <= j + 1; n++)
                            {
                                if (m < 0 || n < 0 || m > Map.height - 1 || n > Map.width - 1)
                                    continue;
                                try
                                {
                                    this.VirtualMap.Grid[m, n].Status = 1;
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine(exc.Message);
                                }
                            }
                        }
                    }
                    if (this.VirtualMap.Grid[i, j].Status == 0)
                        this.VirtualMap.Grid[i, j].Status = this.Memory.Grid[i, j].Status;
                }
            }
            this.VirtualMap.print();
        }

        private bool checkNodeInSet(Node n, List<Node> l)
        {
            foreach (Node item in l)
            {
                if (item.Equals(n))
                    return true;
            }
            return false;
        }

        private Node getNodeInSet(Node n, List<Node> L)
        {
            foreach (Node item in L)
            {
                if (item.Equals(n))
                    return item;
            }
            return null;
        }

        public void exploreWithDFS()
        {
            this.stack = new Stack();
            DFSNode currentNode = StartDFSNode;
            this.DFSNodes.Add(currentNode);
            try
            {
                while (!this._shouldStop)
                {

                    while (stack.Count > 0 || currentNode != null)
                    {
                        if (CurrentCoverage >= UserSetting.CoverageLimit && UserSetting.CoverageLimit != 100)
                        {
                            Console.WriteLine("Coverage limit reached!");
                            OnSendingMessage("Coverage limit reached!!!");
                            this._shouldStop = true;
                            break;
                        }
                        if (currentNode != null)
                        {
                            Console.WriteLine("Visiting node X = {0}, Y = {1}", currentNode.X, currentNode.Y);
                            currentNode.isVisited = true;
                            this.X = currentNode.X;
                            this.Y = currentNode.Y;
                            computeCoverage();
                            if (currentNode.Equals(this.GoalDFSNode))
                            {
                                Console.WriteLine("Reach goal!");
                                OnSendingMessage("Reached goal zone!!!");
                                //OnSendingMessage("X = " + GoalDFSNode.X);
                                //OnSendingMessage("Y = " + GoalDFSNode.Y);
                            }
                            getDFSChildren(currentNode);
                            if (currentNode.BottomChild != null)
                            {
                                stack.Push(currentNode.BottomChild);
                                this.DFSNodes.Add(currentNode.BottomChild);
                            }
                            if (currentNode.LeftChild != null)
                            {
                                stack.Push(currentNode.LeftChild);
                                this.DFSNodes.Add(currentNode.LeftChild);
                            }
                            if (currentNode.TopChild != null)
                            {
                                stack.Push(currentNode.TopChild);
                                this.DFSNodes.Add(currentNode.TopChild);
                            }
                            if (currentNode.RightChild != null)
                            {
                                stack.Push(currentNode.RightChild);
                                this.DFSNodes.Add(currentNode.RightChild);
                            }
                            while (currentNode.allChildrenVisited())
                            {
                                Console.WriteLine("No more children, backtracking!!!");
                                //no more children, backtracking
                                currentNode = currentNode.ParentNode;
                                Thread.Sleep(1000 / UserSetting.Speed);
                                this.X = currentNode.X;
                                this.Y = currentNode.Y;
                                if (currentNode == StartDFSNode)
                                {
                                    break;
                                }
                            }
                            //printStack();
                            if (stack.Count > 0)
                            {
                                currentNode = (DFSNode)stack.Pop();
                            }
                            else
                                break;
                        }
                        else
                        {
                            if (stack.Count > 0)
                            {
                                currentNode = (DFSNode)stack.Pop();
                            }
                            else
                                break;
                        }
                        Thread.Sleep(1000 / UserSetting.Speed);
                    }
                    this.isExplored = true;
                    Console.WriteLine("Exploration finished!!!");
                    OnSendingMessage("Exploration finished!!!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void getDFSChildren(DFSNode node)
        {
            int x = node.X;
            int y = node.Y;
            if (!checkRightSide())
            {
                DFSNode rightChild = new DFSNode(x + 1, y);
                rightChild.ParentNode = node;
                if (!checkDFSNodeInStack(rightChild))
                {
                    node.RightChild = rightChild;
                }
            }

            if (!checkTopSide())
            {
                DFSNode topChild = new DFSNode(x, y + 1);
                topChild.ParentNode = node;
                if (!checkDFSNodeInStack(topChild))
                    node.TopChild = topChild;
            }

            if (!checkLeftSide())
            {
                DFSNode leftChild = new DFSNode(x - 1, y);
                leftChild.ParentNode = node;
                if (!checkDFSNodeInStack(leftChild))
                    node.LeftChild = leftChild;
            }

            if (!checkBottomSide())
            {
                DFSNode bottomChild = new DFSNode(x, y - 1);
                bottomChild.ParentNode = node;
                if (!checkDFSNodeInStack(bottomChild))
                    node.BottomChild = bottomChild;
            }
        }
        public bool checkDFSNodeInStack(DFSNode node)
        {
            foreach (DFSNode item in this.DFSNodes)
            {
                if (item.Equals(node))
                {
                    Console.WriteLine("Node is in stack already u fucking numb");
                    return true;
                }
            }
            return false;
        }

        public void printStack()
        {
            Console.WriteLine("=========Stack===========");
            foreach (DFSNode item in this.stack)
            {
                item.print();
            }
            Console.WriteLine("=======End stack========");
        }

        public void computeCoverage()
        {
            int firstDm = Memory.Grid.GetLength(0);
            int secondDm = Memory.Grid.GetLength(1);
            int explored = 0;
            for (int i = 0; i < firstDm; i++)
            {
                for (int j = 0; j < secondDm; j++)
                {
                    if (Memory.Grid[i, j].Status != 0)
                    {
                        explored++;
                    }
                }
            }
            this.CurrentCoverage = (double)explored / (firstDm * secondDm) * 100;
        }

        public string realTimeShortestPath(string s)
        {
            int[,] data = new int[20, 15];
            int pointer = 5;
            string path;
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        data[i, j] = (int)Char.GetNumericValue(s[pointer++]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The map size may not be 300 chars");
            }

            this.Memory = new Map(data);
            Console.WriteLine("Data");
            this.Memory.print();
            fastestRun();
            if (this.shortestPathComputed)
            {
                path = constructPathForRealTime();
                OnSendingMessage("The mapping has no solution for shortest path");
                return path;
            }
            else
            {
                return null;
            }
        }

        public string constructPathForRealTime()
        {
            this.X = 1;
            this.Y = 1;
            this.Dir = 'U';
            string path = "";
            Console.WriteLine("Size of shortest path" + this.ShortestPath.Count);
            Node temp = this.ShortestPath[this.ShortestPath.Count - 1].PreviousNode;
            while (!temp.Equals(this.StartNode) && temp != null)
            {
                this.ShortestPath.Add(temp);
                temp = temp.PreviousNode;
            }
            this.ShortestPath.Reverse();
            Node currentNode = this.ShortestPath[0];
            foreach (Node item in this.ShortestPath)
            {
                switch (this.Dir)
                {
                    case 'U':
                        if (item.XNode > this.X)
                        {
                            this.turnRight();
                            this.moveForward(1);
                            path += 'R';
                        }
                        else if (item.YNode > this.Y)
                        {
                            this.moveForward(1);
                        }
                        else if (item.XNode < this.X)
                        {
                            this.turnLeft();
                            this.moveForward(1);
                            path += 'L';
                        }
                        break;
                    case 'R':
                        if (item.XNode > this.X)
                        {
                            this.moveForward(1);
                        }
                        else if (item.YNode > this.Y)
                        {
                            this.turnLeft();
                            this.moveForward(1);
                            path += 'L';
                        }
                        else if (item.YNode < this.Y)
                        {
                            this.turnRight();
                            this.moveForward(1);
                            path += 'R';
                        }
                        break;
                    case 'D':
                        if (item.XNode > this.X)
                        {
                            this.turnLeft();
                            this.moveForward(1);
                            path += 'L';
                        }
                        else if (item.YNode < this.Y)
                        {
                            this.moveForward(1);
                        }
                        else if (item.XNode < this.X)
                        {
                            this.turnRight();
                            this.moveForward(1);
                            path += 'R';
                        }
                        break;
                    case 'L':
                        if (item.YNode > this.Y)
                        {
                            this.turnRight();
                            this.moveForward(1);
                            path += 'R';
                        }
                        else if (item.XNode < this.X)
                        {
                            this.moveForward(1);
                        }
                        else if (item.YNode < this.Y)
                        {
                            this.turnLeft();
                            this.moveForward(1);
                            path += 'L';
                        }
                        break;
                    default:
                        break;
                }
                path += 'M';
            }
            return path;
        }

        public void requestStop()
        {
            this._shouldStop = true;
        }
    }
}
