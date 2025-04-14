namespace ProjectManager.Domain.Enums;

[Flags]
public enum TransitionValidationType
{
    None = 0,
    Attachment = 1,
    CommitLink = 2,
    All = 3
}