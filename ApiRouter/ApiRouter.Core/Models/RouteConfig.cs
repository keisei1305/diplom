using System;

namespace ApiRouter.Core.Models;

public class RouteConfig
{
    public string Path { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
    public bool RequiresAuthentication { get; set; }
    public string[] AllowedRoles { get; set; } = Array.Empty<string>();
    public bool IsActive { get; set; } = true;
} 