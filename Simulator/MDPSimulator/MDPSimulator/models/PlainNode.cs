using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPModel
{
    public class PlainNode
    {
        public int XNode { get; set; }
        public int YNode { get; set; }
        public int GCost { get; set; }
        public int FCost { get; set; }
        public int HCost { get; set; }
        public PlainNode PreviousNode { get; set; }
        public PlainNode(int x, int y)
        {
            XNode = x;
            YNode = y;
            GCost = 0;
            HCost = 0;
            FCost = 0;
        }

        public override bool Equals(Object other)
        {
            if (other == null || !this.GetType().Equals(other.GetType()))
                return false;
            else
            {
                PlainNode n = (PlainNode)other;
                return (n.XNode == this.XNode) && (n.YNode == this.YNode);
            }

        }

        public void print()
        {
            Console.WriteLine("{0}, {1}", this.XNode, this.YNode);
        }


        public override string ToString()
        {
            return "X: " + this.XNode + " Y:" + this.YNode;
        }

    }
}
