using System;
using WinSize;
using WinSizeUI;
using Xunit;

namespace WinSizeTests;

public class FeedbackTests
{
    [Fact]
    public void FeedbackUrl_IsValidAbsoluteUri()
    {
        var result = Uri.TryCreate(winsizeMainForm.FeedbackUrl, UriKind.Absolute, out var uri);
        Assert.True(result);
        Assert.NotNull(uri);
        Assert.Equal(Uri.UriSchemeHttps, uri!.Scheme);
    }

    [Fact]
    public void FeedbackUrl_PointsToGitHubIssues()
    {
        Assert.Contains("github.com", winsizeMainForm.FeedbackUrl);
        Assert.Contains("/issues", winsizeMainForm.FeedbackUrl);
    }
}

public class TryResizeWindowTests
{
    [Fact]
    public void TryResizeWindow_ZeroHandle_ReturnsFalse_WithoutCenter()
    {
        bool result = WinSizeShared.TryResizeWindow(IntPtr.Zero, 800, 600, out int err, centerWindow: false);
        Assert.False(result);
        Assert.NotEqual(0, err);
    }

    [Fact]
    public void TryResizeWindow_ZeroHandle_ReturnsFalse_WithCenter()
    {
        bool result = WinSizeShared.TryResizeWindow(IntPtr.Zero, 800, 600, out int err, centerWindow: true);
        Assert.False(result);
        Assert.NotEqual(0, err);
    }

    [Theory]
    [InlineData(0, 600)]
    [InlineData(800, 0)]
    [InlineData(-1, 600)]
    public void TryResizeWindow_InvalidDimensions_ReturnsFalse(int width, int height)
    {
        bool result = WinSizeShared.TryResizeWindow(IntPtr.Zero, width, height, out int err);
        Assert.False(result);
        Assert.NotEqual(0, err);
    }
}

public class DisplaySizeItemTests
{
    [Fact]
    public void ToString_ReturnsTitle()
    {
        var item = new DisplaySizeItem { title = "1920 x 1080 (Full HD / 1080p)", width = 1920, height = 1080 };
        Assert.Equal("1920 x 1080 (Full HD / 1080p)", item.ToString());
    }

    [Fact]
    public void ToString_NullTitle_ReturnsEmptyString()
    {
        var item = new DisplaySizeItem { title = null, width = 800, height = 600 };
        Assert.Equal(string.Empty, item.ToString());
    }

    [Fact]
    public void ToString_EmptyTitle_ReturnsEmptyString()
    {
        var item = new DisplaySizeItem { title = "", width = 800, height = 600 };
        Assert.Equal(string.Empty, item.ToString());
    }
}
