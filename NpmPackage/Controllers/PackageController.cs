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

		public async Task<PackageNode> GetAsync(string name, string version)
		{
			return await GetPackageDependenciesAsync(name, version);
		}

		public static async Task<PackageNode> GetPackageDependenciesAsync(string name, string version)
		{
			PackageNode package = await GetNPMPackageAsync(name, version);
			PackageNode root = package;
			await GetPackageDependenciesAsync(package, root);
			return root;
		}

		public static async Task GetPackageDependenciesAsync(PackageNode package, PackageNode root){
			foreach(var depedency in package.Dependencies){
				var name = depedency.Key;
				//Regex taken from the Semantic Versioning Specification (https://semver.org/)
				//ran into some unexpected versions, fount this very helpful
				//for example "> 1.1.15 <= 2"

				var version = Regex.Match(depedency.Value,@"(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?").Value;
				var childPackage = root.TryGetNode(name, version);
				if(childPackage == null){
					childPackage = await Task.Run(()=> GetNPMPackageAsync(name, version));
					if(childPackage.Dependencies != null){
						await GetPackageDependenciesAsync(childPackage, root);
					}
				}	
				package.AddChild(package, childPackage);		
			}
		}
		public static async Task<PackageNode> GetNPMPackageAsync(PackageNode package){
			return await GetNPMPackageAsync(package.Name, package.Version);
		}

		private const string URL = @"https://registry.npmjs.org/";

		private static async Task<PackageNode> GetNPMPackageAsync(string name, string version)
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
						response = await reader.ReadToEndAsync();
					}
				}
			}

			npmPackage = PackageNode.FromJson(response);
			return npmPackage;
		}
	}
}
