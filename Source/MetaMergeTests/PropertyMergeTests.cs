using Fody;
using MetaMerge.Fody;
using MetaMergeTests.Models;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace MetaMergeTests
{
	public class PropertyMergeTests
	{

		static Fody.TestResult TestResult;

		static PropertyMergeTests()
		{
			var weavingTask = new ModuleWeaver();
			TestResult = weavingTask.ExecuteTestRun($"{typeof(Person).Assembly.GetName().Name}.dll");
		}


		[Fact]
		public void WhenPropertyIsDecoratedWithAttribute_ThenAttributeShouldBeCopied()
		{
			var personType = TestResult.Assembly.GetType(typeof(Person).FullName);
			PropertyInfo firstNameProperty = personType.GetProperty(nameof(Person.FirstName));

			var requiredAttribute = firstNameProperty.GetCustomAttribute<RequiredAttribute>();
			Assert.NotNull(requiredAttribute);
		}

		[Fact]
		public void WhenPropertyIsDecoratedWithAttribute_AndAttributeHasConstructorArguments_ThenAttributeShouldBeCopiedWithConstructorArguments()
		{
			var personType = TestResult.Assembly.GetType(typeof(Person).FullName);
			PropertyInfo firstNameProperty = personType.GetProperty(nameof(Person.FirstName));

			var minLengthAttribute = firstNameProperty.GetCustomAttribute<MinLengthAttribute>();

			Assert.NotNull(minLengthAttribute);
			Assert.Equal(2, minLengthAttribute.Length);
		}


		[Fact]
		public void WhenPropertyIsDecoratedWithAttribute_AndAttributeHasPropertyValues_ThenAttributeShouldBeCopiedWithPropertyValues()
		{
			var personType = TestResult.Assembly.GetType(typeof(Person).FullName);
			PropertyInfo firstNameProperty = personType.GetProperty(nameof(Person.FirstName));

			var displayAttribute = firstNameProperty.GetCustomAttribute<DisplayAttribute>();

			Assert.NotNull(displayAttribute);
			Assert.Equal("First name", displayAttribute.Name);
		}
	}
}