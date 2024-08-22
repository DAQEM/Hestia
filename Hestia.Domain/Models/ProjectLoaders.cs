namespace Hestia.Domain.Models;

[Flags]
public enum ProjectLoaders
{
    None = 0,
    Forge = 1,
    Fabric = 2,
    NeoForge = 4,
    Quilt = 8
}