using Fody;
using Mono.Cecil;
using Morris.MetaMerge.Contracts;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MetaMerge.Fody
{
	public class ModuleWeaver : BaseModuleWeaver
	{
		public override IEnumerable<string> GetAssembliesForScanning()
		{
			yield return "netstandard";
			yield return "mscorlib";
		}

		public override void Execute()
		{
			foreach (var type in ModuleDefinition.Types)
				ScanType(type);

			var contractsReference = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.Name == "MetaMerge.Contacts");
			if (contractsReference is null)
				return;

			ModuleDefinition.AssemblyReferences.Remove(contractsReference);
		}

		private void ScanType(TypeDefinition type)
		{
			foreach (var property in type.Properties)
				ScanProperty(property);
		}

		private void ScanProperty(PropertyDefinition property)
		{
			var metaAttribute = property.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == typeof(MetaAttribute).FullName);
			if (metaAttribute is null)
				return;
			property.CustomAttributes.Remove(metaAttribute);

			CustomAttributeArgument metaType = metaAttribute.ConstructorArguments.First();
			TypeReference metaTypeReference = (TypeReference)metaType.Value;
			TypeDefinition metaTypeDefinition = metaTypeReference.Resolve();

			PropertyDefinition metaTypeTargetProperty = metaTypeDefinition.Properties.FirstOrDefault(x => x.Name == "Target");
			if (metaTypeTargetProperty is null)
			{
				WriteError($"{metaTypeDefinition.FullName} does not have a property \"public object Target {{ get; set; }}\".");
				return;
			}

			var sourceAttributes = metaTypeTargetProperty.CustomAttributes;
			foreach (var currentSourceAttribute in sourceAttributes)
			{
				CustomAttribute localCustomAttribute = CloneAttribute(currentSourceAttribute);
				property.CustomAttributes.Add(localCustomAttribute);
			}
		}

		private CustomAttribute CloneAttribute(CustomAttribute sourceAttribute)
		{
			MethodReference sourceAttributeConstructor = sourceAttribute.Constructor;
			MethodReference localAttributeConstructorReference = ModuleDefinition.ImportReference(sourceAttributeConstructor);

			var localCustomAttribute = new CustomAttribute(localAttributeConstructorReference);
			foreach (var sourceAttributeConstructorArgument in sourceAttribute.ConstructorArguments)
			{
				TypeReference localAttributeTypeReference = sourceAttributeConstructorArgument.Type;
				CustomAttributeArgument localAttributeInstance = new CustomAttributeArgument(localAttributeTypeReference, sourceAttributeConstructorArgument.Value);
				localCustomAttribute.ConstructorArguments.Add(localAttributeInstance);
			}

			return localCustomAttribute;
		}

		private AssemblyDefinition GetLocalAssemblyDefinition(AssemblyNameReference sourceAssemblyNameReference)
		{
			AssemblyDefinition localAssemblyDefinition = ModuleDefinition.AssemblyResolver.Resolve(sourceAssemblyNameReference);
			if (localAssemblyDefinition is null)
			{
				WriteError($"\"{ModuleDefinition.Name}\" does not reference \"{sourceAssemblyNameReference.FullName}\"");
				throw new InvalidOperationException();
			}
			return localAssemblyDefinition;
		}
	}
}