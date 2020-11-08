using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using NpmPackage.Models;

namespace NpmPackage.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PackageController : ControllerBase
	{
		// GET: api/<PackageController>/name/version
		[HttpGet("{name}/{version}")]

		public PackageNode Get(string name, string version)
		{
			return GetPackageDependencies(name, version);
		}

		public static PackageNode GetPackageDependencies(string name, string version)
		{
			PackageNode package = GetNPMPackage(name, version);
			PackageNode root = package;
			GetPackageDependencies(package, root);
			return root;
		}

		public static void GetPackageDependencies(PackageNode package, PackageNode root){
			foreach(var depedency in package.Dependencies){
				var name = depedency.Key;
				var version = Regex.Match(depedency.Value,@"(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?").Value;
				var childPackage = root.TryGetNode(name, version);
				if(childPackage == null){
					childPackage = GetNPMPackage(name, version);
					if(childPackage.Dependencies != null){
						 GetPackageDependencies(childPackage, root);
					}
				}	
				package.AddChild(package, childPackage);		
			}
		}
		public static async Task<PackageNode> GetNPMPackageAsync(PackageNode package){
			return await Task.Run(()=> GetNPMPackage(package.Name, package.Version));
		}

		private const string URL = @"https://registry.npmjs.org/";

		private static PackageNode GetNPMPackage(string name, string version)
		{
			PackageNode npmPackage = null;
			string response = "";
			var path = $"{ URL}/{ name}/{ version}";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
			using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
			{
				using (Stream dataStream = webResponse.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(dataStream))
					{
						response = reader.ReadToEnd();
					}
				}
			}

			npmPackage = PackageNode.FromJson(response);
			return npmPackage;
		}
	}
}
