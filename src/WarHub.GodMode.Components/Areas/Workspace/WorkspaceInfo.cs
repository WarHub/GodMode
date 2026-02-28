namespace WarHub.GodMode.Components.Areas.Workspace;

public sealed class WorkspaceInfo : IEquatable<WorkspaceInfo>
{
    private string? repository;
    private string? appRoute;

    private WorkspaceInfo(WorkspaceType type, string? localFsPath, Uri? gitHubUrl)
    {
        Type = type;
        LocalFsPath = localFsPath;
        GitHubUrl = gitHubUrl;
    }

    public WorkspaceType Type { get; }

    public string? LocalFsPath { get; }

    public Uri? GitHubUrl { get; }

    public string GitHubRepository => repository ??= string.Concat(GitHubUrl?.Segments[^2..]);

    public string AppRoute => appRoute ??= Type switch
    {
        WorkspaceType.GitHub => $"gh/{GitHubRepository}",
        WorkspaceType.LocalFilesystem => "",
        _ => ""
    };

    public static WorkspaceInfo Invalid { get; } = new(WorkspaceType.Invalid, null, null);

    public static WorkspaceInfo CreateLocalFs(string path) => new(WorkspaceType.LocalFilesystem, path, null);

    public static WorkspaceInfo CreateGitHub(Uri gitHubUrl) => new(WorkspaceType.GitHub, null, gitHubUrl);

    public bool Equals(WorkspaceInfo? other) =>
        other is not null && Type == other.Type && LocalFsPath == other.LocalFsPath && GitHubUrl == other.GitHubUrl;

    public override bool Equals(object? obj) => Equals(obj as WorkspaceInfo);

    public override int GetHashCode() => HashCode.Combine(Type, LocalFsPath, GitHubUrl);
}
