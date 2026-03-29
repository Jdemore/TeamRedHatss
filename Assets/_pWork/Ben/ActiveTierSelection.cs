/// <summary>
/// Static state that carries the player's tier selection between scenes.
/// Set before loading a tutorial or gameplay scene, read on Start() in that scene.
/// </summary>
public static class ActiveTierSelection
{
    /// <summary>Which single tier was selected (for tutorial mode).</summary>
    public static int SelectedTier { get; set; } = 0;

    /// <summary>Which tiers are active for gameplay (bit flags, supports multi-select).</summary>
    public static TierFlags ActiveFlags { get; set; } = TierFlags.Vowels;

    /// <summary>Whether the player chose tutorial or gameplay.</summary>
    public static PlayMode Mode { get; set; } = PlayMode.Gameplay;

    public enum PlayMode
    {
        Tutorial,
        Gameplay,
    }
}
