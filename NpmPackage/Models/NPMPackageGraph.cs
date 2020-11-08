using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NpmPackage.Models
{
	[Serializable]
	public class NPMPackageGraph
	{
		private IDictionary<string, PackageNode> visited;
		public PackageNode Root;
		public PackageNode TryGetPackege(string name, string version)
		{
			var key = PackageNode.GetVisitedKey(name, version);
			visited.TryGetValue(key, out var result);
			return result;
		}

	}
}
