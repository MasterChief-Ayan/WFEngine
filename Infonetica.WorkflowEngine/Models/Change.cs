namespace Infonetica.WorkflowEngine.Models;

/// <summary>
/// Represents a single workflow transition/change
/// Tracks what happened and when
/// </summary>
public record Change(
    /// <summary>
    /// Name of the action that was performed
    /// </summary>
    string ActionTaken,
    
    /// <summary>
    /// State before the change
    /// </summary>
    string PreviousState,
    
    /// <summary>
    /// State after the change
    /// </summary>
    string NextState,
    
    /// <summary>
    /// When this change occurred (UTC)
    /// </summary>
    DateTime ChangeTimestamp
);