using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snyk.Models
{
	[Serializable]
	public class NPMPackage
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("dependencies")]
		public Dictionary<string, string> Dependencies { get; set; }

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

		public static NPMPackage FromJson(string json) =>
			JsonConvert.DeserializeObject<NPMPackage>(json);

		public string ToJson() => JsonConvert.SerializeObject(this);
	}
}