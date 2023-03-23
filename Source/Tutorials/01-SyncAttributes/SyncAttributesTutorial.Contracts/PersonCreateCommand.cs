using MetaMerge;
using SyncAttributesTutorial.Definitions;

namespace SyncAttributesTutorial.Contracts;

public class PersonCreateCommand
{
	[Meta(typeof(PersonDefinitions.Salutation))]
	public string Salutation { get; set; }

	[Meta(typeof(PersonDefinitions.GivenName))]
	public string GivenName { get; set; }

	[Meta(typeof(PersonDefinitions.FamilyName))]
	public string FamilyName { get; set; }
}
