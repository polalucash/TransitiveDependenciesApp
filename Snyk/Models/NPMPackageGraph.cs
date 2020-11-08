using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snyk.Models
{
	[Serializable]
	public class NPMPackageGraph
	{
		private IDictionary<NPMPackage, List<NPMPackage>> arrows;
		private IDictionary<string, NPMPackage> visited;

		public IEnumerable<string> GetArrows()
		{
			return arrows
				.SelectMany(r => r.Value.Select(rr =>
					$"{GetVisitedKey(r.Key)} --> {(rr == null ? null : GetVisitedKey(rr))}"));
		}
		public IEnumerable<NPMPackage> GetVisited()
		{
			return visited.Values;
		}
		public NPMPackageGraph()
		{
			arrows = new Dictionary<NPMPackage, List<NPMPackage>>();
			visited = new Dictionary<string, NPMPackage>();
		}
		public void AddVisited(NPMPackage package)
		{
			if (package == null)
			{
				return;
			}

			var key = GetVisitedKey(package);
			if (!visited.ContainsKey(key))
			{
				visited.Add(key, package);
			}
		}
		public NPMPackage TryGetPackege(string name, string version)
		{
			var key = GetVisitedKey(name, version);
			visited.TryGetValue(key, out var result);
			return result;
		}
		public static string GetVisitedKey(string name, string version)
		{
			return name + "_" + version;
		}

		public static string GetVisitedKey(NPMPackage package)
		{
			if (package == null || package.Name == null)
				return null;
			return GetVisitedKey(package.Name, package.Version);
		}

		//Function to Add an edge into the graph  
		public void Add(NPMPackage v, NPMPackage w = null)
		{

			if (arrows.TryGetValue(v, out var vArrows))
			{
				if (!vArrows.Contains(w))
				{
					arrows[v].Add(w); // Add w to v's list.  
				}
			}
			else
			{

				arrows.Add(v, w == null ? new List<NPMPackage>() : new List<NPMPackage> { w });
				AddVisited(v);
			}
			AddVisited(w);
		}
	}
}
