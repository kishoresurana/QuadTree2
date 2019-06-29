using System;
using System.Collections.Generic;
public class Point{
    public double x;
    public double y;
    public Point (double _x, double _y){
        x = _x;
        y = _y;
    }
}

public class Node{
    public Point pos;
    public int data;
    public Node(Point _pos, int _data){
        pos = _pos;
        data = _data;
    }
}

public class Quad{
    // Hold details of the boundary of this node
    Point topLeft;
    Point botRight;

    // Contains set of nodes. Do not exceed 10 nodes
    List<Node> nodes;

    // Children of this tree
    Quad topLeftTree;
    Quad topRightTree;
    Quad botLeftTree;
    Quad botRightTree;

    public Quad(Point _topLeft, Point _botRight){
        topLeft = _topLeft;
        botRight = _botRight;
        nodes = new List<Node>();        
    }

    // Insert a node into the quad tree
    public void Insert(Node node, bool split = false){
        if (node == null)
            return;
        
        if (!inBoundary(node.pos))
            return;
        
        // if any of the child trees are already initialized
        // recurse to the correct quadtree
        if (topLeftTree != null || topRightTree != null || botLeftTree != null || botRightTree != null || split == true){
            if ((topLeft.x + botRight.x)/2 >= node.pos.x){
                // indicates botLeftTree
                if ((topLeft.y + botRight.y)/2 >= node.pos.y){
                    if (botLeftTree == null){
                        botLeftTree = new Quad(new Point(topLeft.x, (topLeft.y + botRight.y)/2)
                                        , new Point((topLeft.x + botRight.x)/2, botRight.y));
                    }
                    botLeftTree.Insert(node);
                }
                // indicates topLeftTree
                else {
                    if (topLeftTree == null){
                        topLeftTree = new Quad(new Point(topLeft.x, topLeft.y)
                                        , new Point((topLeft.x + botRight.x) / 2, (topLeft.y + botRight.y) / 2)); 
                    }
                    topLeftTree.Insert(node); 
                }
            }
            else{
                // Indicates botRightTree
                if ((topLeft.y + botRight.y) / 2 >= node.pos.y) 
                {
                    if (botRightTree == null){
                        botRightTree = new Quad(new Point((topLeft.x + botRight.x) / 2, (topLeft.y + botRight.y) / 2)
                                        , new Point(botRight.x, botRight.y)); 
                    }
                    botRightTree.Insert(node);
                }        
                // Indicates topRightTree
                else
                { 
                    if (topRightTree == null) {
                        topRightTree = new Quad(new Point((topLeft.x + botRight.x) / 2, topLeft.y)
                                        , new Point(botRight.x, (topLeft.y + botRight.y) / 2)); 
                    }
                    topRightTree.Insert(node); 
                } 
            }
        }
        else if (nodes.Count == 10){
            foreach (var chnode in nodes){
                Insert(chnode, true);
            }
            Insert(node, true);
            nodes.Clear();
        }
        else {
            // if this quad contains less than 10 nodes, insert this node
            nodes.Add(node);
            return;
        }
    }

    public bool Search(Point p, ref List<string> addr){
        if (inBoundary(p)){
            // if any of the child trees are already initialized
            // recurse to the child quadtree
            if (topLeftTree != null || topRightTree != null || botLeftTree != null || botRightTree != null){
                if ((topLeft.x + botRight.x)/2 >= p.x){
                    // indicates botLeftTree
                    if ((topLeft.y + botRight.y)/2 >= p.y){
                        addr.Add("01");
                        return botLeftTree.Search(p, ref addr);
                    }
                    // indicates topLeftTree
                    else {
                        addr.Add("00");
                        return topLeftTree.Search(p, ref addr);
                    }
                }
                else{
                    // Indicates botRightTree
                    if ((topLeft.y + botRight.y) / 2 >= p.y) 
                    {
                        addr.Add("11");
                        return botRightTree.Search(p, ref addr);
                    }        
                    // Indicates topRightTree
                    else
                    { 
                        addr.Add("10");
                        return topRightTree.Search(p, ref addr);
                    } 
                }
            }
            else{
                foreach (var chnode in nodes){
                    if (chnode.pos.x == p.x && chnode.pos.y == p.y)
                        return true;
                }
            }
        }
        return false;
    }

    private bool inBoundary(Point p){
        return ((topLeft.x <= p.x && botRight.x >= p.x)
            && (topLeft.y >= p.y && botRight.y <= p.y));
    }
}