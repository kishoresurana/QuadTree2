using System;
using System.Collections.Generic;

namespace QuadTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new Quad(new Point(0, 100), new Point(100, 0));
            Node nd;
            Random r = new Random();
            var nodes = new List<Node>();
            for (int i = 0 ; i < 21; ++i){
                nd = new Node(new Point(r.Next(0, 100), r.Next(0, 100)), 1);
                nodes.Add(nd);
                root.Insert(nd);
            }

            foreach (var node in nodes){
                var addr2 = new List<string>() {};
                var found = root.Search(node.pos, ref addr2);
                var addr = found ? (string.Format("found at {0}", addr2.Count == 0 ? "root" : string.Join("_", addr2))) : "not found";
                Console.WriteLine(string.Format("{0} {1}", string.Format("x:{0} y:{1}", node.pos.x, node.pos.y), addr));
            }
        }
    }
}
