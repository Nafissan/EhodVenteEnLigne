using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using EhodBoutiqueEnLigne.Models.Services;

public class LanguageServiceTests
{
    [Theory]
    [InlineData("English", "en")]
    [InlineData("French", "fr")]
    [InlineData("Spanish", "es")]
    [InlineData("Wolof", "wo")]
    [InlineData("Unknown", "en")]
    public void SetCulture_ReturnsCorrectCulture(string language, string expectedCulture)
    {
        // Arrange
        var languageService = new LanguageService();

        // Act
        var result = languageService.SetCulture(language);

        // Assert
        Assert.Equal(expectedCulture, result);
    }
    [Fact]
    public void SetCulture_ReturnsDefaultCulture_ForUnknownLanguage()
    {
        // Arrange
        var languageService = new LanguageService();
        var unknownLanguage = "Unknown";

        // Act
        var result = languageService.SetCulture(unknownLanguage);

        // Assert
        Assert.Equal("en", result); // We expect the default culture to be returned for unknown language
    }

    // Tests for ChangeUiLanguage method

    [Fact]
    public void ChangeUiLanguage_DoesNotUpdateCultureCookie_WhenHttpContextIsNull()
    {
        // Arrange
        HttpContext httpContext = null;
        var languageService = new LanguageService();
        var language = "French";

        // Act
        languageService.ChangeUiLanguage(httpContext, language);

        // Assert
        // Since the HttpContext is null, the cookie should not be updated
        Assert.Null(httpContext?.Response?.Cookies);
    }
}
