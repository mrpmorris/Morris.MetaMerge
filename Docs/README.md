# MetaMerge - documentation

## Purpose
The purpose of ***MetaMerge** is to allow developers to keep sets of
attributes consistent across different classes.

### Problem definition
The following class is an example of a simple Entity named `Person` which
has `Salutation`, `GivenName`, and `FamilyName` properties.  Each of
these properties are decorated with various .net attributes; these
are usually used by frameworks such as `Entity Framework Core`
when generating a database.

**Server entities**

```c#
namespace MyApp.Entities
{
	public class Person
	{
		public Guid Id { get; set; }

		[Required, MinLength(2), MaxLength(8)]
		[Display(Name = "Salutation"]
		public string Salutation { get; set; }

		[Required, MinLength(2), MaxLength(32)]
		[Display(Name = "Given name")]
		public string GivenName { get; set; }

		[Required, MinLength(1), MaxLength(64)]
		[Display(Name = "Family name")]
		public string FamilyName { get; set; }
	}
}
```

And the following class is an example of a data-transfer-object (DTO) that
is transferred via a web API between the server and the client.

**Shared contract classes**
```c#
namespace MyApp.Contracts
{
	public class Person
	{
		public Guid Id { get; set; }

		[Required, MinLength(2), MaxLength(8)]
		[Display(Name = "Salutation"]
		public string Salutation { get; set; }

		[Required, MinLength(1), MaxLength(64)]
		[Display(Name = "Family name")]
		public string FamilyName { get; set; }

		[Required, MinLength(1), MaxLength(32)]
		[Display(Name = "Given name")]
		public string GivenName { get; set; }
	}
}
```

The problem with the code above is that the Contract class defines the
`MinLength` of `FamilyName` as 1 character, whereas the Entity defines
the `MinLength` as 2 characters.

With properly robust validation in both our client and server, we would
expect the client to accept an input of a single character `X` for
a person's `FamilyName`, the API endpoint on the server will also
determine the `DTO` is valid, but then validation on Domain entities
will throw an exception because `X` is not at least 2 characters in length.

### Solution
Instead, we can define a pattern of .net attributes in a single place,
and then apply them to multiple targets.

**Shared definitions**
```c#
namespace MyApp.Definitions
{
	public static class PersonSalutation
	{
		[Required, MinLength(2), MaxLength(8)]
		[Display(Name = "Salutation"]
		public static object Target { get; set; }
	}

	public static class PersonGivenName
	{
		[Required, MinLength(2), MaxLength(32)]
		[Display(Name = "Given name")]
		public static object Target { get; set; }
	}

	public static class PersonFamilyName
	{
		[Required, MinLength(1), MaxLength(8)]
		[Display(Name = "Salutation"]
		public static object Target { get; set; }
	}
}
```

**Server entities**
```c#
namespace MyApp.Entities
{
	public class Person
	{
		public Guid Id { get; set; }

		[Meta(typeof(Definitions.PersonSalutation)]
		public string Salutation { get; set; }

		[Meta(typeof(Definitions.PersonGivenName)]
		public string GivenName { get; set; }

		[Meta(typeof(Definitions.PersonFamilyName)]
		public string FamilyName { get; set; }
	}
}
```

**Shared contract classes**
```c#
namespace MyApp.Contracts
{
	public class Person
	{
		public Guid Id { get; set; }

		[Meta(typeof(Definitions.PersonSalutation)]
		public string Salutation { get; set; }

		[Meta(typeof(Definitions.PersonGivenName)]
		public string GivenName { get; set; }

		[Meta(typeof(Definitions.PersonFamilyName)]
		public string FamilyName { get; set; }
	}
}
```

### Outcome
For each property ***MetaMerge** finds that is decorated with a
`MetaAttribute` it will replace it with the attributes defined on
the `Target` property of the class specified.

**Before**
```c#
[Meta(typeof(Definitions.PersonSalutation)]
public string Salutation { get; set; }
```

**After**
```c#
[Required, MinLength(2), MaxLength(8)]
[Display(Name = "Salutation"]
public string Salutation { get; set; }
```
