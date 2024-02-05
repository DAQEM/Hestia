namespace Hestia.Domain.Models;

[Flags]
public enum ProjectLoaders
{
    Forge = 1,
    Fabric = 2,
    NeoForge = 4,
    Quilt = 8
}