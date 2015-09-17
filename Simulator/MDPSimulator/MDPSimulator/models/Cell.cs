using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPModel
{
    public class Cell
    {
        public int Status { get; set;}

        public Cell()
        {
            this.Status = 0;
        }
        public Cell(int status)
        {
            this.Status = status;
        }
    }
}
