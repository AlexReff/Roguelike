using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Models
{
    internal interface DTObj
    {
        string Name { get; set; }
    }

    internal class DirectionalTree<T> where T : DTObj
    {
        /// <summary>
        /// 'This' root object
        /// </summary>
        private T RootObject;
        /// <summary>
        /// The map of connected body parts
        /// </summary>
        private Dictionary<XYZRelativeDirection, Dictionary<string, DirectionalTree<T>>> ConnectedParts;
        /// <summary>
        /// A List version of all ConnectedParts
        /// </summary>
        private List<T> AllConnectedParts;
        private Dictionary<string, List<DirectionalTree<T>>> ConnectedPartsByName;
        private DirectionalTree<T> Parent;

        public DirectionalTree(T rootObject)
        {
            ConnectedParts = new Dictionary<XYZRelativeDirection, Dictionary<string, DirectionalTree<T>>>(); //new Dictionary<XYZRelativeDirection, List<DirectionalTree<T>>>();
            ConnectedPartsByName = new Dictionary<string, List<DirectionalTree<T>>>();
            AllConnectedParts = new List<T>();
            RootObject = rootObject;
        }

        public void AddPart(XYZRelativeDirection dir, T part)
        {
            if (!ConnectedParts.ContainsKey(dir))
            {
                ConnectedParts.Add(dir, new Dictionary<string, DirectionalTree<T>>());//new List<DirectionalTree<T>>(); //new List<T>();
            }
            if (!ConnectedPartsByName.ContainsKey(part.Name))
            {
                ConnectedPartsByName.Add(part.Name, new List<DirectionalTree<T>>());
            }

            DirectionalTree<T> thisPartTree = new DirectionalTree<T>(part);
            ConnectedParts[dir].Add(part.Name, thisPartTree);
            ConnectedPartsByName[part.Name].Add(thisPartTree);
            AllConnectedParts.Add(part);
        }

        public T GetRootObject()
        {
            return RootObject;
        }

        public List<DirectionalTree<T>> GetTrees(string name)
        {
            return ConnectedPartsByName[name];
        }

        public List<T> GetAllParts()
        {
            return AllConnectedParts.ToList();
        }

        public List<T> GetParts(XYZRelativeDirection dir, bool exact = false)
        {
            List<T> matchedParts = new List<T>();
            foreach (XYZRelativeDirection key in ConnectedParts.Keys)
            {
                bool validDirection = false;
                if (exact)
                {
                    validDirection = key == dir;
                }
                else
                {
                    validDirection = key.HasFlag(dir);
                }
                if (validDirection)
                {
                    var theseParts = ConnectedParts[key];
                    if (theseParts != null && theseParts.Values.Count > 0)
                    {
                        matchedParts.AddRange(theseParts.Values.SelectMany((m) => m.AllConnectedParts));
                    }
                }
            }

            return matchedParts;
        }
    }
}
