# MetaMerge.Fody
![](./Images/MetaMerge-Logo.png)

## Introduction
***MetaMerge*** allows you to create patterns of .net attributes and
apply them to multiple targets.

## Goal
The aim of ***MetaMerge*** is to enable programmers to keep sets of
attributes consistent throughout their application.  For example, if
there is a FirstName property in a business object (server side) and
also in multiple Contract classes (typically API data-transfer-objects)
then we can ensure meta information such as MinLength etc are consistent
across all of them.

```c#
public class Person
{
  [Meta(typeof(PersonFamilyName))]
  public string FamilyName { get; set; }
}

public class PersonDto
{
  [Meta(typeof(PersonFamilyName))]
  public string FamilyName { get; set; }
}

// The meta definition
public static class PersonFamilyName
{
  // The following attributes will be applied to
  // the target properties above.
  [Required, MinLength(2), MaxLength(32), Display(Name = "Family name")]
  public static object Target { get; set; }
}
```

## Getting started
The easiest way to get started is to read the [documentation](./Docs/README.md).
Which includes tutorials that are numbered in an order recommended
for learning ***MetaMerge***.

## Installation
You can download the latest release / pre-release NuGet package from
the official NuGet page.

* [MetaMerge](https://www.nuget.org/packages/MetaMerge.Fody) [![NuGet version (MetaMerge.Fody)](https://img.shields.io/nuget/v/MetaMerge.Fody.svg?style=flat-square)](https://www.nuget.org/packages/MetaMerge.Fody/)

## Release notes
See the [Releases page](./Docs/releases.md) for release history.