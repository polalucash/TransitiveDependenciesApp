using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NpmPackage.Models
{
	[Serializable]
	public class PackageNode
	{
		public PackageNode()
		{
		}

		public PackageNode(string name, string description, string version)
		{
			this.Name = name;
			this.Description = description;
			this.Version = version;
			this.Dependencies = new Dictionary<string, string>();
			this.Children = new List<PackageNode>();
		}

		[JsonProperty("id")]
		public string Id=> Name + "_" + Version;

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("dependencies")]
		public Dictionary<string, string> Dependencies { get; set; }

		[JsonProperty("children")]
		public List<PackageNode> Children { get; set; }

		public static string GetVisitedKey(PackageNode package)
		{
			if (package == null || package.Name == null)
				return null;
			return GetVisitedKey(package.Name, package.Version);
		}
		public void AddChild(PackageNode v, PackageNode w)
		{
			var node = this.Search(v.Id);
			if (node != null)
			{
				if (node.Children == null)
				{
					node.Children = new List<PackageNode>();
				}
				node.Children.Add(w);
			}
		}

		public PackageNode TryGetNode(string name, string version)
		{
			var key = GetVisitedKey(name, version);
			var result = this.Search(key);
			return result;
		}

		public PackageNode Search(string id, List<string> visited = null){			
			if (Id == id)
			{
				return this;
			}
			if (visited == null)
			{
				visited = new List<string>();
			}
			visited.Add(Id);
			if(Children == null)
				return null;
			foreach (var node in Children)
			{
				if (!visited.Contains(node.Id))
				{
					var res = node.Search(id, visited);
					if (res != null)
					{
						return res;
					}
				}
			}
			return null;
		}
		public static string GetVisitedKey(string name, string version)
		{
			return name + "_" + version;
		}

		public bool IsThisVersionBigger(string otherVerStr)
		{
			return CompareVersions(otherVerStr) > 0;
		}
		public bool IsThisVersionSmaller(string otherVerStr)
		{
			return CompareVersions(otherVerStr) < 0;
		}
		public int CompareVersions(string otherVerStr)
		{
			if (this.Version == otherVerStr)
			{
				return 0;
			}
			var thisVer = this.Version.Split('.').Select(r => int.Parse(r)).ToArray();
			var otherVer = otherVerStr.Split('.').Select(r => int.Parse(r)).ToArray();
			for (var i = 0; i < this.Version.Length; i++)
			{
				if (thisVer[i] > otherVer[i])
				{
					return 1;
				}
				else if (thisVer[i] < otherVer[i])
				{
					return -1;
				}
			}
			return 0;
		}

		public static PackageNode FromJson(string json) =>
			JsonConvert.DeserializeObject<PackageNode>(json);

		public string ToJson() => JsonConvert.SerializeObject(this);
	}
}