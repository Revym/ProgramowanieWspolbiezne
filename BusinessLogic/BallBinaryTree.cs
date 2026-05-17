using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    internal class BallSnapshot
    {
        public IBall Ball { get; }
        public double X { get; }
        public double Y { get; }

        public BallSnapshot (IBall ball)
        {
            Ball = ball;
            X = ball.X;
            Y = ball.Y;
        }
    }
    
    internal class TreeNode
    {
        public BallSnapshot Snapshot { get; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }

        public TreeNode(BallSnapshot snapshot)
        {
            Snapshot = snapshot;
        }
    }

    internal class BallBinaryTree
    {
        private TreeNode? _root;

        public void Build(IEnumerable<IBall> balls)
        {
            var snapshots = balls.Select(b => new BallSnapshot(b)).ToList();
            _root = BuildRecursive(snapshots, 0);
        }

        private TreeNode? BuildRecursive(List<BallSnapshot> snapshots, int depth)
        {
            if (snapshots.Count == 0) return null;

            int axis = depth % 2;

            snapshots.Sort((a, b) => axis == 0 ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

            int medianIndex = snapshots.Count / 2;
            TreeNode node = new TreeNode(snapshots[medianIndex]);

            node.Left = BuildRecursive(snapshots.GetRange(0, medianIndex), depth + 1);
            node.Right = BuildRecursive(snapshots.GetRange(medianIndex + 1, snapshots.Count - (medianIndex + 1)), depth + 1);

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

            if (node.Snapshot.Ball != target)
            {
                double dist = Math.Sqrt(Math.Pow(node.Snapshot.X - target.X, 2) + Math.Pow(node.Snapshot.Y - target.Y, 2));
                if (dist <= searchRadius)
                {
                    result.Add(node.Snapshot.Ball);
                }
            }

            int axis = depth % 2;
            double targetCoord = axis == 0 ? target.X : target.Y;
            double nodeCoord = axis == 0 ? node.Snapshot.X : node.Snapshot.Y;

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
