using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

namespace Aurochses.Extensions.Configuration.Tests
{
    public class ProjectHelpersTests
    {
        private readonly string _applicationBasePath;

        public ProjectHelpersTests()
        {
            _applicationBasePath = AppContext.BaseDirectory;
        }

        [Fact]
        public void GetProjectPath_Success()
        {
            // Arrange
            const string solutionName = "Aurochses.Extensions.Configuration";

            var path = new Regex($@"\S+\\{solutionName}\\").Match(_applicationBasePath).Value;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = new Regex(@"\S+\\Aurochses.Extensions.Configuration.Tests").Match(_applicationBasePath).Value;
                path = path.Replace(@"\test\Aurochses.Extensions.Configuration.Tests", string.Empty);
            }

            // Act & Assert
            Assert.Equal(Path.Combine(path, "src", "Aurochses.Extensions.Configuration"), ProjectHelpers.GetProjectPath("src", typeof(ProjectHelpers).GetTypeInfo().Assembly));
        }

        [Fact]
        public void GetProjectPath_ThrowException_WhenSolutionNotFound()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<Exception>(() => ProjectHelpers.GetProjectPath("incorrect", typeof(ProjectHelpers).GetTypeInfo().Assembly));
            Assert.Equal($"Project root could not be located using the application root {_applicationBasePath}.", exception.Message);
        }
    }
}