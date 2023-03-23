using System.ComponentModel.DataAnnotations;

namespace SyncAttributesTutorial.Definitions;

public static class PersonDefinitions
{
	public static class Salutation
	{
		[Required, MinLength(2), MaxLength(8)]
		[Display(Name = "Salutation")]
		public static object Target { get; set; }
	}

	public static class GivenName
	{
		[Required, MinLength(2), MaxLength(32)]
		[Display(Name = "Given name")]
		public static object Target { get; set; }
	}

	public static class FamilyName
	{
		[Required, MinLength(1), MaxLength(64)]
		[Display(Name = "Family name")]
		public static object Target { get; set; }
	}
}