using MetaMerge;
using SyncAttributesTutorial.Definitions;
using System;

namespace SyncAttributesTutorial.Contracts;

public class PersonUpdateCommand
{
	public Guid Id { get; set; }

	[Meta(typeof(PersonDefinitions.Salutation))]
	public string Salutation { get; set; }

	[Meta(typeof(PersonDefinitions.GivenName))]
	public string GivenName { get; set; }

	[Meta(typeof(PersonDefinitions.FamilyName))]
	public string FamilyName { get; set; }
}
