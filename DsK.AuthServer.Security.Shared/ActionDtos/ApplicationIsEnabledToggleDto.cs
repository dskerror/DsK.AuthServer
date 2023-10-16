using System.ComponentModel.DataAnnotations;

namespace DsK.AuthServer.Security.Shared;
public partial class ApplicationIsEnabledToggleDto
{
    [Key]
    public int Id { get; set; }
}
