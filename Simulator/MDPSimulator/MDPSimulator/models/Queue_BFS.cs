using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPModel
{
    class Queue_BFS
    {
        private int first;
        private int last;
        public List<Node> queueBFS;

        public Queue_BFS() {
            first = 0;
            last = -1;
            queueBFS = new List<Node>();
        }

        public Node pop_Node(List<Node> queuBFS)
        {
            if (first <= last)
                return queuBFS[first++];
            else
                return null;
        }

        public void push_Node(Node x)
        {
            last++;
            queueBFS.Insert(last, x);
        }
    }
}
