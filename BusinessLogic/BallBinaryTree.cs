using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    internal class TreeNode
    {
        public IBall Ball { get; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }

        public TreeNode(IBall ball)
        {
            Ball = ball;
        }
    }

    internal class BallBinaryTree
    {
        private TreeNode? _root;

        public void Build(IEnumerable<IBall> balls)
        {
            _root = BuildRecursive(balls.ToList(), 0);
        }

        private TreeNode? BuildRecursive(List<IBall> balls, int depth)
        {
            if (balls.Count == 0) return null;

            int axis = depth % 2;

            balls.Sort((a, b) => axis == 0 ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

            int medianIndex = balls.Count / 2;
            TreeNode node = new TreeNode(balls[medianIndex]);

            node.Left = BuildRecursive(balls.GetRange(0, medianIndex), depth + 1);
            node.Right = BuildRecursive(balls.GetRange(medianIndex + 1, balls.Count - (medianIndex + 1)), depth + 1);

            return node;
        }

        public List<IBall> FindPotentialCollisions(IBall target, double searchRadius)
        {
            List<IBall> result = new List<IBall>();
            SearchRecursive(_root, target, searchRadius, 0, result);
            return result;
        }

        private void SearchRecursive(TreeNode? node, IBall target, double searchRadius, int depth, List<IBall> result)
        {
            if (node == null) return;

            if (node.Ball != target)
            {
                double dist = Math.Sqrt(Math.Pow(node.Ball.X - target.X, 2) + Math.Pow(node.Ball.Y - target.Y, 2));
                if (dist <= searchRadius)
                {
                    result.Add(node.Ball);
                }
            }

            int axis = depth % 2;
            double targetCoord = axis == 0 ? target.X : target.Y;
            double nodeCoord = axis == 0 ? node.Ball.X : node.Ball.Y;

            if (targetCoord - searchRadius <= nodeCoord)
            {
                SearchRecursive(node.Left, target, searchRadius, depth + 1, result);
            }
            if (targetCoord + searchRadius >= nodeCoord)
            {
                SearchRecursive(node.Right, target, searchRadius, depth + 1, result);
            }
        }
    }
}
