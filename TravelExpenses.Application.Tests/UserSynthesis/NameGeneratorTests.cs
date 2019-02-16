using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TravelExpenses.Application.Utilities;
using Xunit;

namespace TravelExpenses.Application.Tests.UserSynthesis
{
    public class NameGeneratorTests
    {
        [Fact]
        public void TryIt()
        {
            var sut = new NameGenerator();

            var sb = new StringBuilder();
            for (int i = 0; i < 50; i++)
            {
                sb.AppendLine(sut.FullName());
            }

            var foo = sb.ToString();
        }
    }
}
