using MetaMerge;
using System.ComponentModel.DataAnnotations;

namespace MetaMergeTests.Models;

internal static class PersonMeta
{
	internal static class RequiredFirstName
	{
		[Required, MinLength(2), MaxLength(12), Display(Name = "First name")]
		public static object Target { get; set; }
	}
}

internal class Person
{
	[Meta(typeof(PersonMeta.RequiredFirstName))]
	public string FirstName { get; set; }
}
