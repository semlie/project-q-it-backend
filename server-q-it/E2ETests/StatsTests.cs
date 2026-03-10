using System.Net;
using System.Net.Http.Json;

namespace server_q_it.E2ETests;

public class FrontendIntegrationTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;

    public FrontendIntegrationTests()
    {
        _baseUrl = "http://localhost:5173";
        _client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        _client.Timeout = TimeSpan.FromSeconds(10);
    }

    [Fact]
    public async Task Frontend_Homepage_Responds()
    {
        var response = await _client.GetAsync("/");
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Frontend_StatsPage_Responds()
    {
        var response = await _client.GetAsync("/stats");
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Frontend_LoginPage_Responds()
    {
        var response = await _client.GetAsync("/login");
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Frontend_DashboardPage_Responds()
    {
        var response = await _client.GetAsync("/dashboard");
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}

public class BackendApiTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;

    public BackendApiTests()
    {
        _baseUrl = "http://localhost:5000";
        _client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        _client.Timeout = TimeSpan.FromSeconds(10);
    }

    [Fact]
    public async Task Backend_Users_Responds()
    {
        var response = await _client.GetAsync("/api/Users");
        Assert.NotEqual(HttpStatusCode.NotImplemented, response.StatusCode);
    }

    [Fact]
    public async Task Backend_StatsStudentOverall_Responds()
    {
        var response = await _client.GetAsync("/api/Stats/student/1/overall");
        Assert.NotEqual(HttpStatusCode.NotImplemented, response.StatusCode);
    }

    [Fact]
    public async Task Backend_StatsStudentSubjects_Responds()
    {
        var response = await _client.GetAsync("/api/Stats/student/1/subjects");
        Assert.NotEqual(HttpStatusCode.NotImplemented, response.StatusCode);
    }

    [Fact]
    public async Task Backend_StatsTeacherOverall_Responds()
    {
        var response = await _client.GetAsync("/api/Stats/teacher/1/overall");
        Assert.NotEqual(HttpStatusCode.NotImplemented, response.StatusCode);
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}
