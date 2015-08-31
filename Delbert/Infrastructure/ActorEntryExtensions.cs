namespace Delbert.Infrastructure
{
    public static class ActorEntryExtensions
    {
        public static string GetPathFor(this ActorEntry entry, ActorPathType pathType)
        {
            switch (pathType)
            {
                case ActorPathType.Absolute:
                    return entry.AbsoluteUrl;
                case ActorPathType.Relative:
                    return entry.RelativeUrl;
                default:
                    return entry.AbsoluteUrl;
            }
        }
    }
}
