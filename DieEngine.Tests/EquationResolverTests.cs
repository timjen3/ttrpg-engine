using DieEngine.CustomFunctions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DieEngine.Tests
{
	[TestFixture]
	[TestOf(typeof(EquationResolver))]
	public class EquationResolverTests
	{
		[Test]
		[Ignore("Not implemented yet")]
		public void Test1()
		{
			var runner = new Mock<ICustomFunctionRunner>();
			var resolver = new EquationResolver(runner.Object);

			Assert.Pass();
		}

		// todo: descrie needed tests below
		/*	
			1. throws when missing required args
			2. processes inputs into equation
			3. 
		*/
	}
}
