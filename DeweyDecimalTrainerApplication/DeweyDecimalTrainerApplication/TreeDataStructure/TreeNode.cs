using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeweyDecimalTrainerApplication
{
    class TreeNode<String>
    {
        public string Data { get; set; }

        public TreeNode<String> Parent { get; set; }

        public List<TreeNode<String>> Children { get; set; }

        public int GetHeight()
        {
            int height = 1;
            TreeNode<String> current = this;
            while (current.Parent != null)
            {
                height++;
                current = current.Parent;

            }

            return height;
        }

    }
}
