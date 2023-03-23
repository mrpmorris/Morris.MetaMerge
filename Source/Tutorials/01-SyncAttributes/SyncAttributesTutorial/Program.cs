using System;
using System.Reflection;
using System.Text;

namespace SyncAttributesTutorial;

internal class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine(GetClassInfo(typeof(Entities.Person)));
		Console.WriteLine(GetClassInfo(typeof(Contracts.PersonCreateCommand)));
		Console.WriteLine(GetClassInfo(typeof(Contracts.PersonUpdateCommand)));
		Console.ReadLine();
	}

	static string GetClassInfo(Type type)
	{
		var builder = new StringBuilder();
		builder.AppendLine($"{type.FullName}");
		string separator = new string('=', type.FullName.Length);
		builder.AppendLine(separator);
		foreach (PropertyInfo property in type.GetProperties())
		{
			builder.AppendLine($"  Property: {property.Name}");
			foreach (Attribute attribute in property.GetCustomAttributes(true))
				AppendAttribute(builder, attribute);
		}

		return builder.ToString();
	}

	private static void AppendAttribute(StringBuilder builder, Attribute attribute)
	{
		builder.AppendLine($"    Attribute: {attribute.GetType().Name}");
		foreach(PropertyInfo property in attribute.GetType().GetProperties())
		{
			try
			{
				object value = property.GetValue(attribute);
				if (value is not null)
					builder.AppendLine($"      {property.Name} = {value}");
			}
			catch (TargetInvocationException)
			{ }
		}
	}
}